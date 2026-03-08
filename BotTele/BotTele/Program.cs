using System.Text;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.Data.SqlClient;
using Dapper;
using DotNetEnv;

Env.Load();

var botClient = new TelegramBotClient(Environment.GetEnvironmentVariable("BOT_TOKEN"));
string dbConn = Environment.GetEnvironmentVariable("DB_CONN");

// Lưu trạng thái mua hàng của User (Tạm thời trong bộ nhớ)
var userState = new Dictionary<long, PurchaseSession>();

using var cts = new CancellationTokenSource();

Console.WriteLine("Bot is starting...");

botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, new ReceiverOptions { AllowedUpdates = Array.Empty<UpdateType>() }, cts.Token);

Console.ReadLine();

async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
{
    // 1. Xử lý Message văn bản
    if (update.Message is { Text: { } messageText })
    {
        long chatId = update.Message.Chat.Id;
        string username = update.Message.From?.FirstName ?? "User";
        if (messageText.StartsWith("/"))
        {
            userState.Remove(chatId);
        }
        // Kiểm tra nếu User đang trong luồng nhập số lượng
        else if(userState.ContainsKey(chatId) && userState[chatId].Step == "WaitingForQty")
        {
            // Kiểm tra xem nội dung nhập vào có phải là số nguyên dương hay không
            if (int.TryParse(messageText, out int qty) && qty > 0)
            {
                // Nếu đúng là số, tiến hành xử lý giao dịch
                await ProcessQuantityInput(bot, update.Message, userState[chatId]);
            }
            else
            {
                // Nếu không phải kiểu số, thông báo lỗi và HỦY trạng thái chờ để giải phóng User
                await bot.SendMessage(chatId, "❌ Không phải kiểu số. Hủy nhập số lượng!");
                userState.Remove(chatId);
            }
        }

        switch (messageText.Split(' ')[0].ToLower())
        {
            case "/start":
                string startMsg = $"👋 Xin chào {username}!\n\nChào mừng bạn đến với **X(Twitter) Account Store** 🎉\n\nĐể bắt đầu sử dụng ngay, hãy nhấn vào các nút menu bên dưới nhé!\n\n📋 Các lệnh nhanh:\n• /price — Xem Báo Giá\n• /count — Số Lượng Hàng Hiện Có Trong Kho\n• /buy — Tôi Muốn Mua Hàng\n\nBạn cũng chỉ cần gõ dấu / là Telegram sẽ hiện đầy đủ danh sách lệnh.";
                await bot.SendMessage(chatId, startMsg, parseMode: ParseMode.Markdown);
                break;

            case "/price":
                string priceMsg = "📊 **BÁO GIÁ TÀI KHOẢN X (Twitter)**\n\n" +
                                 "🆕 **X New (24h)**\n• Dưới 150 acc: 999 ₫\n• 150 - 4.999 acc: 819 ₫\n• Trên 5.000 acc: 779 ₫\n\n" +
                                 "📅 **X Tuổi trên 7 ngày**\n• Dưới 150 acc: 1.199 ₫\n• 150 - 4.999 acc: 899 ₫\n• Trên 5.000 acc: 859 ₫";
                await bot.SendMessage(chatId, priceMsg, parseMode: ParseMode.Markdown);
                break;

            case "/count":
                using (var db = new SqlConnection(dbConn)) {
                    int cNew = db.ExecuteScalar<int>("SELECT COUNT(*) FROM X_Acc WHERE AccNew=1 AND IsSold=0");
                    int cOld = db.ExecuteScalar<int>("SELECT COUNT(*) FROM X_Acc WHERE AccNew=0 AND IsSold=0");
                    await bot.SendMessage(chatId, $"📦 **CẬP NHẬT KHO HÀNG X**\n\n🆕 X new (24h): {cNew} tài khoản\n📅 X 7 Days: {cOld} tài khoản", parseMode: ParseMode.Markdown);
                }
                break;

            case "/buy":
                var inlineKeyboard = new InlineKeyboardMarkup(new[] {
                    new [] { InlineKeyboardButton.WithCallbackData("Xnew", "buy_1"), InlineKeyboardButton.WithCallbackData("X7days", "buy_0") }
                });
                await bot.SendMessage(chatId, "Chọn Sản Phẩm Muốn Mua:", replyMarkup: inlineKeyboard);
                break;
        }
    }

    // 2. Xử lý Callback từ nút bấm
    if (update.CallbackQuery is { Data: { } data })
    {
        long chatId = update.CallbackQuery.From.Id;
        bool isNew = data == "buy_1";
        userState[chatId] = new PurchaseSession { AccNew = isNew, Step = "WaitingForQty" };
        await bot.SendMessage(chatId, $"Bạn chọn mua **{(isNew ? "X New" : "X 7 Days")}**. Vui lòng nhập số lượng cần mua (Ví dụ: 10):", parseMode: ParseMode.Markdown);
    }
}

async Task ProcessQuantityInput(ITelegramBotClient bot, Message msg, PurchaseSession session)
{
    if (int.TryParse(msg.Text, out int qty) && qty > 0)
    {
        long amount = CalculatePrice(qty, session.AccNew);
        string transId = "TX" + DateTime.Now.Ticks.ToString().Substring(10);
        
        await bot.SendMessage(msg.Chat.Id, "⏳ Đang tạo mã QR thanh toán, vui lòng chờ...");

        // Tạo Link VietQR
        string qrUrl = $"https://img.vietqr.io/image/{Environment.GetEnvironmentVariable("BANK_BIN")}-{Environment.GetEnvironmentVariable("BANK_ACC")}-compact.jpg?amount={amount}&addInfo={transId}&accountName={Environment.GetEnvironmentVariable("BANK_NAME")}";

        await bot.SendPhoto(msg.Chat.Id, InputFile.FromUri(qrUrl), 
            caption: $"✅ Đơn hàng: {transId}\n💰 Tổng tiền: {amount:N0} ₫\n📌 Nội dung ck: `{transId}`\n\n(Hệ thống sẽ tự động gửi file sau khi nhận tiền)", parseMode: ParseMode.Markdown);
        
        // Luồng logic: Ở đây bạn nên có một Background Task check ngân hàng. 
        // Giả lập sau 5s thanh toán thành công:
        _ = Task.Run(async () => {
            await Task.Delay(5000); 
            await DeliverProduct(bot, msg.Chat.Id, transId, qty, session.AccNew);
        });

        userState.Remove(msg.Chat.Id);
    }
    else {
        await bot.SendMessage(msg.Chat.Id, "❌ Số lượng không hợp lệ. Vui lòng nhập số nguyên dương:");
    }
}

long CalculatePrice(int qty, bool isNew) {
    if (isNew) {
        if (qty < 150) return qty * 999;
        if (qty < 5000) return qty * 819;
        return qty * 779;
    } else {
        if (qty < 150) return qty * 1199;
        if (qty < 5000) return qty * 899;
        return qty * 859;
    }
}

async Task DeliverProduct(ITelegramBotClient bot, long chatId, string transId, int qty, bool isNew)
{
    using var db = new SqlConnection(dbConn);
    var accs = (await db.QueryAsync<X_Acc>("SELECT TOP (@qty) * FROM X_Acc WHERE AccNew=@isNew AND IsSold=0", new { qty, isNew })).ToList();

    if (accs.Count < qty) {
        await bot.SendMessage(chatId, "😔 Rất tiếc, kho hàng vừa hết. Vui lòng liên hệ Admin để hoàn tiền.");
        return;
    }

    StringBuilder sb = new StringBuilder();
    foreach (var acc in accs) {
        sb.AppendLine($"{acc.TK}|{acc.MK}");
        db.Execute("UPDATE X_Acc SET IsSold=1 WHERE ID=@ID", new { acc.ID });
        db.Execute("INSERT INTO X_Acc_Sold (ID_X_Acc, Transaction_ID) VALUES (@ID, @transId)", new { acc.ID, transId });
    }

    string fileName = $"Accounts_{transId}.txt";
    System.IO.File.WriteAllText(fileName, sb.ToString());

    using var stream = System.IO.File.OpenRead(fileName);
    await bot.SendDocument(chatId, InputFile.FromStream(stream, fileName), caption: "✅ Thanh toán thành công! Cảm ơn bạn đã ủng hộ.");
}

Task HandleErrorAsync(ITelegramBotClient bot, Exception ex, CancellationToken ct) { Console.WriteLine(ex); return Task.CompletedTask; }

public class PurchaseSession {
    public bool AccNew { get; set; }
    public string Step { get; set; }
}