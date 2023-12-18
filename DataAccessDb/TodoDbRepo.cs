using Dapper;
using DataAccess;
using Interfaces;
using Microsoft.Extensions.Logging;
using Models;
using System.Data;


namespace DataAccessDb
{
    public class TodoDbRepo : ITodoRepo
    {
        private readonly TodoDb _todoDb;
        private readonly ILogger<TodoDbRepo> _logger;

        public TodoDbRepo(TodoDb db, ILogger<TodoDbRepo> logger)
        {
            _logger = logger;
            _todoDb = db;
        }

        public async Task<IEnumerable<Todo>> GetTodos()
        {
            try
            {
                using IDbConnection conn = _todoDb.Connection;
                var query = 
                    @$"SELECT 
                        {nameof(Todo.TodoUId)}, 
                        {nameof(Todo.TodoText)}, 
                        {nameof(Todo.DueDate)},
                        {nameof(Todo.CreatedDate)},
                        {nameof(Todo.IsComplete)},
                        {nameof(Todo.ParentTodoUId)}
                    FROM Todos
                    WHERE {nameof(Todo.ParentTodoUId)} IS NULL;

                    SELECT 
                        {nameof(Todo.TodoUId)}, 
                        {nameof(Todo.TodoText)}, 
                        {nameof(Todo.DueDate)},
                        {nameof(Todo.CreatedDate)},
                        {nameof(Todo.IsComplete)},
                        {nameof(Todo.ParentTodoUId)}
                    FROM Todos 
                    WHERE {nameof(Todo.ParentTodoUId)} IS NOT NULL;";

                conn.Open();
                using var multi = await conn.QueryMultipleAsync(query);

                var parentTodos = multi.Read<Todo>() ?? [];
                var subTodos = multi.Read<Todo>() ?? [];
                foreach (var todo in parentTodos)
                {
                    todo.SubTodos = subTodos
                        .Where(st => st.ParentTodoUId == todo.TodoUId)
                        .ToList();
                }
                return parentTodos.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured getting todos.");
                throw;
            }
        }

        public async Task AddTodo(Todo todo)
        {
            try
            {
                using IDbConnection conn = _todoDb.Connection;
                var query =
                    @$"INSERT INTO Todos (
                        {nameof(Todo.TodoText)}, 
                        {nameof(Todo.DueDate)},
                        {nameof(Todo.ParentTodoUId)}
                    ) 
                    VALUES (
                        @{nameof(Todo.TodoText)}, 
                        @{nameof(Todo.DueDate)},
                        @{nameof(Todo.ParentTodoUId)}
                    );";

                conn.Open();
                await conn.ExecuteAsync(query, todo);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured adding a todo.");
                throw;
            }
        }

        public async Task DeleteTodo(Todo todo)
        {
            try
            {
                using IDbConnection conn = _todoDb.Connection;
                var query = 
                    @$"DELETE FROM Todos
                    WHERE {nameof(Todo.TodoUId)} = @{nameof(Todo.TodoUId)}";

                conn.Open();
                await conn.ExecuteAsync(query, new { todo.TodoUId });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured deleting a todo.");
                throw;
            }
        }


        public async Task<Todo> GetTodo(Guid todoUId)
        {
            try
            {
                using IDbConnection conn = _todoDb.Connection;
                var query =
                    @$"SELECT 
                        {nameof(Todo.TodoUId)}, 
                        {nameof(Todo.TodoText)}, 
                        {nameof(Todo.DueDate)},
                        {nameof(Todo.CreatedDate)},
                        {nameof(Todo.IsComplete)},
                        {nameof(Todo.ParentTodoUId)}
                    FROM Todos
                    WHERE {nameof(Todo.TodoUId)} = @{nameof(Todo.TodoUId)};
            
                    SELECT 
                        {nameof(Todo.TodoUId)}, 
                        {nameof(Todo.TodoText)}, 
                        {nameof(Todo.DueDate)},
                        {nameof(Todo.CreatedDate)},
                        {nameof(Todo.IsComplete)},
                        {nameof(Todo.ParentTodoUId)}
                    FROM Todos
                    WHERE {nameof(Todo.ParentTodoUId)} = @{nameof(Todo.TodoUId)}";

                conn.Open();
                using var multi = await conn.QueryMultipleAsync(query, new { todoUId });
                var todo = await multi.ReadFirstOrDefaultAsync<Todo>();
                if (todo != null)
                {
                    todo.SubTodos = (await multi.ReadAsync<Todo>()).ToList();
                }
                return todo;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured getting a todo.");
                throw;
            }
        }


        public async Task<Todo> UpdateTodo(Todo todo)
        {
            try
            {
                using IDbConnection conn = _todoDb.Connection;
                var query = $@"
                    UPDATE Todos SET 
                        {nameof(Todo.TodoText)} = @{nameof(Todo.TodoText)}, 
                        {nameof(Todo.DueDate)} = @{nameof(Todo.DueDate)}, 
                        {nameof(Todo.IsComplete)} = @{nameof(Todo.IsComplete)}, 
                        {nameof(Todo.ParentTodoUId)} = @{nameof(Todo.ParentTodoUId)} 
                    WHERE {nameof(Todo.TodoUId)} = @{nameof(Todo.TodoUId)};
                       
                    SELECT 
                        {nameof(Todo.TodoUId)},
                        {nameof(Todo.TodoText)},
                        {nameof(Todo.DueDate)},
                        {nameof(Todo.CreatedDate)},
                        {nameof(Todo.IsComplete)},
                        {nameof(Todo.ParentTodoUId)} 
                    FROM Todos 
                    WHERE {nameof(Todo.TodoUId)} = @{nameof(Todo.TodoUId)};";

                conn.Open();
                var result = await conn.QuerySingleAsync<Todo>(query, todo);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured updating a todo.");
                throw;
            }
        }
    }
}
