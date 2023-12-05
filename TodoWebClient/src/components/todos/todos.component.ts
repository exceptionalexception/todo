import { Component, Inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TodoDialogComponent } from '../todo-dialog/todo-dialog.component';
import { TodoService } from '../../services/todo.service';
import { Todo } from '../../models/todo.model';

@Component({
  selector: 'todos',
  templateUrl: './todos.component.html',
  styleUrl: './todos.component.scss'
})
export class TodosComponent {

  constructor(
    @Inject(MatDialog) public dialog: MatDialog,
    private todoService: TodoService) {}

  todoColumns = ['todo', 'dueDate', 'createdDate', 'isComplete'];
  todos: Todo[] = [];

  ngOnInit() {
    this.todoService.getTodos().subscribe((todos: Todo[]) => {
      this.todos = todos || [];
    });
  }

  openTodoDialog(todoId?: number): void {
    let dialogRef = this.dialog.open(TodoDialogComponent, {
      width: '500px',
      data: { todoId: todoId }
    });

    dialogRef.afterClosed().subscribe((newTodo: Todo) => {
      console.log('The dialog was closed');
      this.todos.push(newTodo);
    });
  }
}

