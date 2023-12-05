using Interfaces;
using Models;

namespace DataAccessDb
{
    public class TodoDbRepo : ITodoRepo
    {
        public Task<Todo> AddTodo(Todo todo)
        {
            throw new NotImplementedException();
        }

        public Task CompleteTodo(Guid todoUId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTodo(Todo todo)
        {
            throw new NotImplementedException();
        }

        public Task<Todo> GetTodo(Guid todoUId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Todo>> GetTodos()
        {
            throw new NotImplementedException();
        }

        public Task<Todo> UpdateTodo(Todo todo)
        {
            throw new NotImplementedException();
        }
    }
}
