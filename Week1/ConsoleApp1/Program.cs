namespace week1
{
    public class SinhVien
    {
        // 1. Biến đếm dùng chung cho toàn bộ Class (để private)
        private static int _nextId = 1;

        // 2. ID này là của RIÊNG mỗi sinh viên (không có static)
        public int Id { get; private set; }
        public string Name { get; private set; }

        public SinhVien(string name)
        {
            Name = name;
            // Gán ID hiện tại và tăng biến đếm lên cho người sau
            Id = _nextId++;
        }

        public void UpdateName(string name)
        {
            Name = name;
        }
    }
    
    // Cách sử dụng thực tế
    public class Program
    {
        public static string GetSinhVienInfo(SinhVien sv) => sv switch
        {
            null => "Sinh viên không tồn tại.",
            { Name: "Nam" } => $"Đặc biệt: Sinh viên Nam có ID: {sv.Id}",
            { Name: "Han" } => $"Đặc biệt: Sinh viên Han có ID: {sv.Id}",
            _ => $"Sinh viên {sv.Name} có ID: {sv.Id}" // _ tương đương với default
        };
        public static void Main()
        {
            // Sử dụng Collection Expression (C# 12) để tạo danh sách đối tượng
            List<SinhVien> danhSach = [
                new SinhVien("Nam"),
                new SinhVien("Han"),
                new SinhVien("Hao")
            ];

            foreach (var sv in danhSach)
            {
                Console.WriteLine(GetSinhVienInfo(sv));
            }
            
        }
    }
}