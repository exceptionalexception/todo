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

        public async Task AddTodo(TodoDto todo)
        {
            var todoToAdd = todo.ToModel();
            await _todoRepo.AddTodo(todoToAdd);
        }

        public async Task CompleteTodo(Guid todoUId)
        {
            var todo = await _todoRepo.GetTodo(todoUId);
            todo.IsComplete = true;
            await _todoRepo.UpdateTodo(todo);

            // this is a parent todo
            if (todo.SubTodos.Count > 0)
            {
                todo.SubTodos.ForEach(async _ => {
                    _.IsComplete = true;
                    await _todoRepo.UpdateTodo(_);
                });
            }

            // check if this is a subtodo
            if (todo.ParentTodoUId != null)
            {
                // check sibling subtodos to see if they are all complete
                var parentTodo = await _todoRepo.GetTodo(todo.ParentTodoUId.GetValueOrDefault());
                var allSubTodosComplete = parentTodo.SubTodos.All(_ => _.IsComplete);

                if (allSubTodosComplete)
                {
                    parentTodo.IsComplete = true;
                    await _todoRepo.UpdateTodo(parentTodo);
                }
            }
        }

        public async Task DeleteTodo(Guid todo)
        {
            try
            {
                var todoToDelete = await _todoRepo.GetTodo(todo) 
                    ?? throw new Exception("Todo was not found.");

                if (todoToDelete.SubTodos.Count != 0)
                {
                    todoToDelete.SubTodos.ForEach(async _ => 
                        await _todoRepo.DeleteTodo(_)
                    );
                }

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
