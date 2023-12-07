import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Todo } from '../../models/todo.model';
import { TodoService } from '../../services/todo.service';
import { DateValidationService } from '../../services/date-validation.service';

@Component({
  selector: 'todo-dialog',
  templateUrl: './todo-dialog.component.html',
  styleUrls: ['./todo-dialog.component.scss']
})
export class TodoDialogComponent{
  todo: Todo = new Todo('', new Date());
  todoDialogTitle: string = 'Add Todo';
  errorMessage: string = '';

  constructor(
    private todoService: TodoService,
    private dateValidationService: DateValidationService,
    public dialogRef: MatDialogRef<TodoDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public existingTodo: Todo) 
    { 
      if(existingTodo) {
        this.todo = existingTodo;
      }
    }

  addTodo() {
    if(!this.todo?.todoText) {
      this.errorMessage = 'Please enter a todo description.';
      return;
    }
    if(!this.dateValidationService.isValidDate(this.todo.dueDate)) {
      this.errorMessage = 'Please enter a valid date.';
      return;
    }
    this.todoService.addTodo(this.todo).subscribe((addedTodo: Todo) => {
      this.dialogRef.close(addedTodo);
    });
  }

  onCloseClick(): void {
    this.dialogRef.close(this.todo);
  }
}
