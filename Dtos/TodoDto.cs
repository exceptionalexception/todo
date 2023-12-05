namespace Dtos
{
    public class TodoDto
    {
        public Guid TodoUId { get; set; }
        public required string TodoText { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public Guid? ParentTodoUId { get; set; }
        public TodoDto? ParentTodo { get; set; }

        public IEnumerable<TodoDto> SubTodos { get; set; } = new List<TodoDto>();
        public bool IsComplete { get; set; }
    }
}
