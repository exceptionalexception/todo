namespace Dtos
{
    public class TodoDto
    {
        public int TodoId { get; set; }
        public string TodoText { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime EditedDate { get; set; }
        public int? ParentTodo { get; set; }
    }
}
