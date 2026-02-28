using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Practice
{
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
}
