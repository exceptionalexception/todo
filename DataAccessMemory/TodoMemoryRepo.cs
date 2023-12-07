using Interfaces;
using Microsoft.Extensions.Logging;
using Models;

namespace DataAccessMemory
{
    public class TodoMemoryRepo : ITodoRepo
    {
        private readonly ILogger<TodoMemoryRepo> _logger;
        public TodoMemoryRepo(ILogger<TodoMemoryRepo> logger)
        {
            _logger = logger;
            InitializeTodos();
        }

        private static List<Todo>? AllTodos;


        public async Task<IEnumerable<Todo>> GetTodos()
        {
            return AllTodos?
                .Where(t => t.ParentTodoUId == null)
                .Select(_ => {
                    _.SubTodos = AllTodos?.Where(t => t.ParentTodoUId == _.TodoUId).ToList() ?? [];
                    return _;
                }).ToList() ?? Enumerable.Empty<Todo>();
        }

        public async Task<Todo> GetTodo(Guid todoUId)
        {
            var todo = AllTodos.FirstOrDefault(t => t.TodoUId == todoUId);

            if (todo != null)
            {
                todo.SubTodos = AllTodos.Where(t => t.ParentTodoUId == todo.TodoUId).ToList();
            }

            return todo;
        }

        public async Task<Todo> AddTodo(Todo todo)
        {
            todo.TodoUId = Guid.NewGuid();
            AllTodos.Add(todo);
            return todo;
        }

        public async Task DeleteTodo(Todo todo)
        {
            AllTodos = AllTodos.Where(_ => _.TodoUId != todo.TodoUId).ToList();
        }

        public async Task<Todo> UpdateTodo(Todo todo)
        {
            try
            {
                var todoToUpdate = AllTodos.FirstOrDefault(t => t.TodoUId == todo.TodoUId)
                    ?? throw new Exception("Todo was not found.");

                todoToUpdate.TodoText = todo.TodoText;
                todoToUpdate.DueDate = todo.DueDate;
                todoToUpdate.ParentTodoUId = todo.ParentTodoUId;
                todoToUpdate.ParentTodo = todo.ParentTodo;
                todoToUpdate.SubTodos = todo.SubTodos;
                return todoToUpdate;

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating todo.");
                throw;
            }
        }

        private void InitializeTodos()
        {
            var todo1 = new Todo
            {
                TodoUId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                TodoText = "Replace old outlets with GFCI.",
                DueDate = DateTime.Now.AddDays(-2),
                CreatedDate = DateTime.Now.AddDays(-5)
            };
            var todo1a = new Todo { TodoUId = Guid.NewGuid(), TodoText = "Go to Lowes and buy GFCI outlet.", DueDate = DateTime.Now.AddDays(-2), CreatedDate = DateTime.Now.AddDays(-5), ParentTodoUId = todo1.TodoUId, IsComplete = true };
            var todo1b = new Todo { TodoUId = Guid.NewGuid(), TodoText = "Find your tools.", DueDate = DateTime.Now.AddDays(-2), CreatedDate = DateTime.Now.AddDays(-5), ParentTodoUId = todo1.TodoUId };
            var todo1c = new Todo { TodoUId = Guid.NewGuid(), TodoText = "Replace the outlets.", DueDate = DateTime.Now.AddDays(-2), CreatedDate = DateTime.Now.AddDays(-5), ParentTodoUId = todo1.TodoUId };
            todo1.SubTodos = new List<Todo> { todo1a, todo1b, todo1c };

            var todo2 = new Todo { TodoUId = Guid.NewGuid(), TodoText = "Clean the house.", DueDate = DateTime.Now.AddDays(-2), CreatedDate = DateTime.Now.AddDays(-3), IsComplete = true };
            var todo3 = new Todo { TodoUId = Guid.NewGuid(), TodoText = "Do the laundry.", DueDate = DateTime.Now.AddHours(-5), CreatedDate = DateTime.Now };
            var todo4 = new Todo { TodoUId = Guid.NewGuid(), TodoText = "Sleep.", DueDate = DateTime.Now.AddHours(-5), CreatedDate = DateTime.Now };
            var todo5 = new Todo { TodoUId = Guid.NewGuid(), TodoText = "Eat.", DueDate = DateTime.Now.AddHours(-5), CreatedDate = DateTime.Now };
            var todo6 = new Todo { TodoUId = Guid.NewGuid(), TodoText = "Eat.", DueDate = DateTime.Now.AddHours(-10), CreatedDate = DateTime.Now };
            var todo7 = new Todo { TodoUId = Guid.NewGuid(), TodoText = "Eat.", DueDate = DateTime.Now.AddHours(-20), CreatedDate = DateTime.Now };
            var todo8 = new Todo { TodoUId = Guid.NewGuid(), TodoText = "Eat.", DueDate = DateTime.Now.AddHours(-50), CreatedDate = DateTime.Now };

            AllTodos = new List<Todo> { todo1, todo2, todo3, todo4, todo5, todo6, todo7, todo8, todo1a, todo1b, todo1c };   }
    }
}
