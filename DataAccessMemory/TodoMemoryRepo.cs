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

            var todo1 = new Todo { 
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

            Todos = [todo1, todo2, todo3];
        }

        private List<Todo> Todos { get; set; } = [];

        public async Task<IEnumerable<Todo>> GetTodos()
        {
            return Todos;
        }

        public async Task<Todo> GetTodo(Guid todoUId)
        {
            return Todos.FirstOrDefault(t => t.TodoUId == todoUId);
        }

        public async Task<Todo> AddTodo(Todo todo)
        {
            Todos.Add(todo);
            return todo;
        }

        public async Task DeleteTodo(Todo todo) 
        { 
            Todos.Remove(todo);
        }

        public async Task<Todo> UpdateTodo(Todo todo)
        {
            try
            {
                var todoToUpdate = Todos.FirstOrDefault(t => t.TodoUId == todo.TodoUId) 
                    ?? throw new Exception("Todo was not found.");

                todoToUpdate.TodoText = todo.TodoText;
                todoToUpdate.DueDate = todo.DueDate;
                todoToUpdate.CreatedDate = todo.CreatedDate;
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

        public async Task CompleteTodo(Guid todoUId)
        {
            try
            {
                var todoToComplete = Todos.FirstOrDefault(t => t.TodoUId == todoUId)
                    ?? throw new Exception("Error updating todo. Todo was not found.");

                todoToComplete.IsComplete = true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error completing todo.");
                throw;
            }
        }
    }
}
