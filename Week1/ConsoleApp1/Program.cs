namespace week1
{
    public class SinhVien
    {
        // 1. Biến đếm dùng chung cho toàn bộ Class (để private)
        private static int _nextId = 1;
        public int Score { get; set; }
        // 2. ID này là của RIÊNG mỗi sinh viên (không có static)
        public int Id { get; private set; }
        public string Name { get; private set; }
        public DateOnly NgayVao { get; set; }

        public SinhVien(string name, int score)
        {
            Name = name;
            // Gán ID hiện tại và tăng biến đếm lên cho người sau
            Id = _nextId++;
            NgayVao = new DateOnly();
            Score = score;
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
            _ => $"Sinh viên {sv.Name} có ID: {sv.Id} Điểm: {sv.Score} Ngày Nhập Học: {sv.NgayVao}" // _ tương đương với default
        };
        public static void Main()
        {
            // Sử dụng Collection Expression (C# 12) để tạo danh sách đối tượng
            List<SinhVien> danhSach = [
                new SinhVien("Nam", 5),
                new SinhVien("Han", 8),
                new SinhVien("Hao", 10)
            ];
            var svGioiNhat = danhSach.MaxBy(sv => sv.Score);
            var grSV = danhSach.GroupBy(sv => sv.Score >= 8 ? "Giỏi" : "Trung Bình");
            var avgScore = danhSach.Average(sv => sv.Score);
            Console.WriteLine("SV giỏi nhất: " + GetSinhVienInfo(svGioiNhat!));
            Console.WriteLine("Nhóm SV: ");
            foreach (var group in grSV)
            {
                Console.WriteLine($"Nhóm {group.Key}:");
                foreach (var sv in group)
                {
                    Console.WriteLine(GetSinhVienInfo(sv));
                }
            }
            Console.WriteLine("Điểm trung bình : " + avgScore);


        }
    }
}