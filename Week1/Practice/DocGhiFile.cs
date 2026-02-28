
namespace Practice
{ 

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