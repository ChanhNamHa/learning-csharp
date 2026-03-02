using System;
using System.Collections.Generic;
using System.Text;

namespace MiniProject
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public override string ToString() =>
            $"ID: {Id} | Tên: {Name} | SL: {Quantity} | Giá: {Price:C}";
    }
}
