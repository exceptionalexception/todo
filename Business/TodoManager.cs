using Dtos;
using Interfaces;
using Microsoft.Extensions.Logging;
using Utilities.Extensions;

namespace Business
{
    public class TodoManager : ITodoManager
    {
        private readonly ITodoRepo _todoRepo;
        private readonly ILogger<TodoManager> _logger;
        public TodoManager(
            ITodoRepo todoRepo, 
            ILogger<TodoManager> logger)
        {
            _logger = logger;
            _todoRepo = todoRepo;
        }

        public async Task<List<TodoDto>> GetTodos()
        {
            var todosDb = await _todoRepo.GetTodos();
            return todosDb.ToDtos();
        }

        public async Task<TodoDto> GetTodo(Guid todoUId)
        {
            var todoDb = await _todoRepo.GetTodo(todoUId);
            return todoDb.ToDto();
        }

        public async Task<TodoDto> AddTodo(TodoDto todo)
        {
            var todoToAdd = todo.ToModel();
            todoToAdd.CreatedDate = DateTime.Now;
            todoToAdd.IsComplete = false;
            var todoDb = await _todoRepo.AddTodo(todoToAdd);
            return todoDb.ToDto();
        }

        public async Task<TodoDto> AddSubTodo(Guid parentTodoUId, TodoDto subTodo)
        {
            var parentTodo = await _todoRepo.GetTodo(parentTodoUId);
            if (parentTodo == null)
            {
                throw new Exception("Parent TODO not found");
            }

            if (parentTodo.SubTodos.Count >= 2)
            {
                throw new Exception("Maximum depth of 2 reached");
            }

            var subTodoToAdd = subTodo.ToModel();
            subTodoToAdd.CreatedDate = DateTime.Now;
            subTodoToAdd.IsComplete = false;
            parentTodo.SubTodos.Add(subTodoToAdd);

            var todoDb = await _todoRepo.UpdateTodo(parentTodo);
            return todoDb.ToDto();
        }

        public async Task CompleteTodo(Guid todoUId)
        {
            await _todoRepo.CompleteTodo(todoUId);
        }

        public async Task DeleteTodo(Guid todo)
        {
            try
            {
                var todoToDelete = await _todoRepo.GetTodo(todo) 
                    ?? throw new Exception("Todo was not found.");

                await _todoRepo.DeleteTodo(todoToDelete);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting todo.");
                throw;
            }
        }

        public async Task<TodoDto> UpdateTodo(TodoDto todo)
        {
            try
            {
                var todoDb = await _todoRepo.UpdateTodo(todo.ToModel())
                    ?? throw new Exception("Todo was not found.");
                return todoDb.ToDto();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating todo.");
                throw;
            }
        }
    }
}
