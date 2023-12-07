using Dtos;
using Models;

namespace Utilities.Extensions
{
    public static class TodoExtensions
    {
        public static List<TodoDto> ToDtos(this IEnumerable<Todo> todos)
        {
            if (todos == null) return null;
            return todos.Select(t => t.ToDto()).ToList();
        }

        public static TodoDto ToDto(this Todo todo)
        {
            if (todo == null) return null;

            return new TodoDto {
                TodoUId = todo.TodoUId,
                TodoText = todo.TodoText,
                IsComplete = todo.IsComplete,
                CreatedDate = todo.CreatedDate,
                DueDate = todo.DueDate,
                ParentTodoUId = todo.ParentTodoUId,
                SubTodos = todo.SubTodos.ToDtos()
            };
        }

        public static List<Todo> ToModels(this IEnumerable<TodoDto> todos)
        {
            if (todos == null) return null;
            return todos.Select(t => t.ToModel()).ToList();
        }

        public static Todo ToModel(this TodoDto todo)
        {
            if (todo == null) return null;

            return new Todo
            {
                TodoUId = todo.TodoUId,
                TodoText = todo.TodoText,
                DueDate = todo.DueDate,
                ParentTodoUId = todo.ParentTodoUId,
                SubTodos = todo.SubTodos.ToModels()
            };
        }

    }
}
