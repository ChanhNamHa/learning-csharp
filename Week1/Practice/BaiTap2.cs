//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Practice
//{
//    public static class BaiTap2
//    {
//        public class InsolventException : Exception
//        {
//            public decimal CurrentBalance { get; }
//            public decimal RequestedAmount { get; }

//            // Constructor nhận vào số dư và số tiền yêu cầu
//            public InsolventException(decimal balance, decimal amount)
//                : base($"Giao dịch thất bại: Số dư hiện tại ({balance}) không đủ để rút {amount}.")
//            {
//                CurrentBalance = balance;
//                RequestedAmount = amount;
//            }
//        }
//        public static void Withdraw(decimal balance, decimal amount)
//        {
//            // Kiểm tra tham số đầu vào (System Exception)
//            if (amount <= 0)
//            {
//                throw new ArgumentException("Số tiền rút phải lớn hơn 0.");
//            }

//            // Kiểm tra logic nghiệp vụ (Custom Exception)
//            if (amount > balance)
//            {
//                throw new InsolventException(balance, amount);
//            }

//            Console.WriteLine($"Rút tiền thành công! Số dư còn lại: {balance - amount}");
//        }
//        public static void Main()
//        {
//            decimal myBalance = 1000;
//            decimal drawAmount = 1500; // Thử rút quá số dư

//            try
//            {
//                Console.WriteLine("Bắt đầu thực hiện giao dịch...");
//                Withdraw(myBalance, drawAmount);
//            }
//            catch (InsolventException ex)
//            {
//                // Truy cập vào các thuộc tính riêng của Custom Exception
//                decimal missingAmount = ex.RequestedAmount - ex.CurrentBalance;
//                Console.WriteLine($"Lỗi nghiệp vụ: {ex.Message}");
//                Console.WriteLine($"=> Bạn còn thiếu {missingAmount} để thực hiện giao dịch này.");
//            }
//            catch (ArgumentException ex)
//            {
//                Console.WriteLine($"Lỗi tham số: {ex.Message}");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Lỗi hệ thống không xác định: {ex.Message}");
//            }
//            finally
//            {
//                // Khối này luôn chạy bất kể có lỗi hay không
//                Console.WriteLine("--------------------------------------");
//                Console.WriteLine("Giao dịch kết thúc, cảm ơn bạn đã dùng dịch vụ.");
//            }
//        }
//    }
//}