
using Dtos;

namespace Interfaces
{
    public interface ITodoManager
    {
        Task<List<TodoDto>> GetTodos();
        Task<TodoDto> GetTodo(Guid todoId);
        Task AddTodo(TodoDto todo);
        Task<TodoDto> UpdateTodo(TodoDto todo);
        Task CompleteTodo(Guid todoId);
        Task DeleteTodo(Guid todoId);
    }
}
