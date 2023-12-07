namespace Models
{
    public class Todo
    {
        public Guid TodoUId { get; set; }
        public required string TodoText { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsComplete { get; set; }
        public Guid? ParentTodoUId { get; set; }

        public List<Todo> SubTodos { get; set; } = new List<Todo>();
    }
}
