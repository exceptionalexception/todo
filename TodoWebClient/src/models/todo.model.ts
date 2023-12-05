export class Todo {
    todoUId?: string | null = null;
    todoText?: string | null = null;
    dueDate?: Date | null = null;
    createdDate?: Date | null = null;
    parentTodoUId?: string | null = null;
    parentTodo?: Todo | null = null;
    subTodos: Todo[] = [];
    isComplete: boolean = false;

    constructor(
        todoText: string, 
        dueDate?: Date, 
        todoUId?: string, 
        createdDate?: Date, 
        parentTodoUId?: string, 
        parentTodo?: Todo, 
        subTodos?: Todo[], 
        isComplete?: boolean) 
    {
        this.todoText = todoText;
        this.dueDate = dueDate;
        this.todoUId = todoUId;
        this.createdDate = createdDate;
        this.parentTodoUId = parentTodoUId;
        this.parentTodo = parentTodo;
        this.subTodos = subTodos || [];
        this.isComplete = isComplete || false;
    }
}
