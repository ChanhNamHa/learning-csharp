using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO; // Thêm thư viện này để dùng File

namespace Practice
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class TodoRepository
    {
        private readonly string _filePath = "todos.json";
        private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

        public void Save(List<TodoItem> items)
        {
            string json = JsonSerializer.Serialize(items, _options);
            File.WriteAllText(_filePath, json);
        }

        public List<TodoItem> Load()
        {
            if (!File.Exists(_filePath)) return new List<TodoItem>();
            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();
        }
    } // Kết thúc TodoRepository

    // Bổ sung class Program bao quanh Main
    public class Program
    {
        public static void Main()
        {
            TodoRepository repository = new TodoRepository();

            List<TodoItem> myTasks = repository.Load();
            Console.WriteLine($"Dang co {myTasks.Count} cong viec trong danh sach.");

            myTasks.Add(new TodoItem
            {
                Id = myTasks.Count + 1,
                TaskName = "Hoc C# Basic Architecture",
                IsCompleted = false
            });
            myTasks.Add(new TodoItem
            {
                Id = myTasks.Count + 1,
                TaskName = "Hoc C++ Basic Architecture",
                IsCompleted = true
            });

            repository.Save(myTasks);
            var list = repository.Load();

            foreach (var item in list)
            {
                Console.WriteLine($"Id: {item.Id}, Task: {item.TaskName}, Completed: {item.IsCompleted}");
            }

            Console.WriteLine("Da luu cong viec moi vao file todos.json!");
        }
    }
}