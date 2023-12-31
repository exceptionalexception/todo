﻿using Models;

namespace Interfaces
{
    public interface ITodoRepo
    {
        Task<IEnumerable<Todo>> GetTodos();
        Task<Todo> GetTodo(Guid todoUId);
        Task AddTodo(Todo todo);
        Task<Todo> UpdateTodo(Todo todo);
        Task DeleteTodo(Todo todo);
    }
}
