using MiniProject;

class Program
{
    static async Task Main(string[] args)
    {
        InventoryManager manager = new();

        try
        {
            // Demo thêm sản phẩm
            manager.AddProduct(new Product { Id = 1, Name = "Laptop Dell", Quantity = 10, Price = 1500 });
            manager.AddProduct(new Product { Id = 2, Name = "Chuột Logitech", Quantity = 50, Price = 25 });

            // Demo tìm kiếm
            var searchResult = manager.SearchProct("Laptop");
            Console.WriteLine($"Tìm thấy: {searchResult.Count} sản phẩm.");

            // Demo cập nhật & Lưu file
            manager.UpdateQuantity(1, 15);
            await manager.SaveInventory();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi: {ex.Message}");
        }

        Console.WriteLine("Nhấn phím bất kỳ để thoát...");
        Console.ReadKey();
    }
}