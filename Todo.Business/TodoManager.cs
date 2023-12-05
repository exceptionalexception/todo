using Dtos;
using Interfaces;
using Utilities.Extensions;

namespace Business
{
    public class TodoManager : ITodoManager
    {
        private readonly ITodoRepo _todoRepo;

        public TodoManager(ITodoRepo todoRepo)
        {
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

        public async Task CompleteTodo(Guid todoUId)
        {
            await _todoRepo.CompleteTodo(todoUId);
        }

        public async Task DeleteTodo(Guid todo)
        {
            var todoToDelete = await _todoRepo.GetTodo(todo);
            if (todoToDelete == null) return;
            await _todoRepo.DeleteTodo(todoToDelete);
        }

        public async Task<TodoDto> UpdateTodo(TodoDto todo)
        {
            var todoDb = await _todoRepo.UpdateTodo(todo.ToModel());
            return todoDb.ToDto();
        }
    }
}
