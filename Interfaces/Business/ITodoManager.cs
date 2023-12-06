
using Dtos;

namespace Interfaces
{
    public interface ITodoManager
    {
        Task<List<TodoDto>> GetTodos();
        Task<TodoDto> GetTodo(Guid todoId);
        Task<TodoDto> AddTodo(TodoDto todo);
        Task<TodoDto> AddSubTodo(Guid parentTodoUId, TodoDto subTodo);
        Task<TodoDto> UpdateTodo(TodoDto todo);
        Task CompleteTodo(Guid todoId);
        Task DeleteTodo(Guid todoId);
    }
}
