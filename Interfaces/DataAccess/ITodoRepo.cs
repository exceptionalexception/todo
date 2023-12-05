using Models;
using System.Collections;

namespace Interfaces
{
    public interface ITodoRepo
    {
        Task<IEnumerable<Todo>> GetTodos();
        Task<Todo> GetTodo(Guid todoUId);
        Task<Todo> AddTodo(Todo todo);
        Task<Todo> UpdateTodo(Todo todo);
        Task CompleteTodo(Guid todoUId);
        Task DeleteTodo(Todo todo);
    }
}
