using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace MiniProject
{
    public class InventoryManager
    {
        private List<Product> _products = new();
        private const string FilePath = "inventory.json";

        public void LoadInventory()
        {
            if (System.IO.File.Exists(FilePath))
            {
                string json = System.IO.File.ReadAllText(FilePath);
                _products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
            }
        }
        public void AddProduct(Product product)
        {
            if (_products.Any(x => x.Id == product.Id))
            {
                throw new Exception("ID sản phẩm đã tồn tại");
            }
            _products.Add(product);
        }
        public bool RemoveProduct(Product product)
        {
            var productToRemove = _products.FirstOrDefault(x => x.Id == product.Id);
            return productToRemove != null && _products.Remove(product);
        }
        public void UpdateQuantity(int productId, int newQuantity)
        {
            var product = _products.FirstOrDefault(x => x.Id == productId);
            if (product != null)
            {
                product.Quantity = newQuantity;
            }
            else
            {
                throw new Exception("Không tìm thấy sản phẩm với ID đã cho");
            }
        } 
        public async Task SaveInventory()
        {
            Console.WriteLine("Đang lưu dữ liệu...");
            await Task.Delay(2000); // Giả lập độ trễ
            string json = JsonSerializer.Serialize(_products, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(FilePath, json);
            Console.WriteLine("Lưu file JSON thành công!");
        }
        public void DeleteInventory()
        {
            if (System.IO.File.Exists(FilePath))
            {
                System.IO.File.Delete(FilePath);
                _products.Clear();
            }
        }
        public List<Product> SearchProct(string keyword)
        {
            return _products.Where(x => x.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
        }

    }
}
