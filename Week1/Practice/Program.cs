//using System;
//using System.Collections.Generic;
//using System.Linq; // Quan trọng: Phải có cái này để dùng .Where, .GroupBy

//public static class Program
//{
//    // Khai báo record ở đây là hợp lệ
//    public record Transaction(int Id, string AccountNumber, decimal Amount, DateTime Date, string Category);

//    public static void Main()
//    {
//        var transactions = new List<Transaction> {
//            new(1, "AC-10042", -150.00m, new DateTime(2026, 2, 1, 8, 30, 0), "Grocery"),
//            new(2, "AC-20511", 2500.00m, new DateTime(2026, 2, 1, 10, 0, 0), "Salary"),
//            new(3, "AC-10042", -45.50m, new DateTime(2026, 2, 2, 12, 15, 0), "Dining"),
//            new(4, "AC-30922", -1200.00m, new DateTime(2026, 2, 3, 15, 45, 0), "Rent"),
//            new(5, "AC-10042", -30.00m, new DateTime(2026, 2, 5, 9, 20, 0), "Transport"),
//            new(6, "AC-20511", -89.99m, new DateTime(2026, 2, 6, 19, 30, 0), "Shopping"),
//            new(7, "AC-40118", 500.00m, new DateTime(2026, 2, 7, 11, 0, 0), "Transfer"),
//            new(8, "AC-30922", -65.00m, new DateTime(2026, 2, 8, 14, 10, 0), "Utilities"),
//            new(9, "AC-10042", -12.00m, new DateTime(2026, 2, 10, 7, 45, 0), "Coffee"),
//            new(10, "AC-20511", -210.00m, new DateTime(2026, 2, 12, 16, 20, 0), "Healthcare"),
//            new(11, "AC-40118", -55.00m, new DateTime(2026, 2, 13, 18, 0, 0), "Entertainment"),
//            new(12, "AC-10042", -105.25m, new DateTime(2026, 2, 15, 13, 30, 0), "Grocery"),
//            new(13, "AC-30922", 150.00m, new DateTime(2026, 2, 16, 9, 0, 0), "Refund"),
//            new(14, "AC-20511", -40.00m, new DateTime(2026, 2, 18, 20, 15, 0), "Transport"),
//            new(15, "AC-40118", -300.00m, new DateTime(2026, 2, 19, 10, 50, 0), "Education"),
//            new(16, "AC-10042", -25.00m, new DateTime(2026, 2, 21, 14, 0, 0), "Dining"),
//            new(17, "AC-30922", -75.50m, new DateTime(2026, 2, 23, 11, 20, 0), "Shopping"),
//            new(18, "AC-20511", -50.00m, new DateTime(2026, 2, 24, 15, 40, 0), "Utilities"),
//            new(19, "AC-10042", -18.00m, new DateTime(2026, 2, 26, 8, 10, 0), "Coffee"),
//            new(20, "AC-40118", 120.00m, new DateTime(2026, 2, 27, 17, 0, 0), "Interest")
//        };

//        var nhomTheoCategory = transactions
//            .Where(t => t.Amount > 10)
//            .GroupBy(t => t.Category)
//            .Select(g => new { // Đổi tên biến t thành g (group) cho dễ phân biệt
//                CategoryName = g.Key,
//                TotalAmount = g.Sum(x => x.Amount),
//                TransactionCount = g.Count()
//            })
//            .OrderByDescending(res => res.TotalAmount);

//        foreach (var item in nhomTheoCategory)
//        {
//            Console.WriteLine($"Loai: {item.CategoryName,-15} | Tong: {item.TotalAmount,10} | So GD: {item.TransactionCount}");
//        }
//    }
//}