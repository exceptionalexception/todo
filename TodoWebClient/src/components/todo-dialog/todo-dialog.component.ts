import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Todo } from '../../models/todo.model';
import { TodoHttpService } from '../../services/todo-http.service';
import { DateValidationService } from '../../services/date-validation.service';
import { catchError, of } from 'rxjs';

@Component({
  selector: 'todo-dialog',
  templateUrl: './todo-dialog.component.html',
  styleUrls: ['./todo-dialog.component.scss']
})
export class TodoDialogComponent{
  todo: Todo = new Todo('', new Date());
  todoDialogTitle: string = ''
  errorMessage: string = '';

  constructor(
    private todoService: TodoHttpService,
    private dateValidationService: DateValidationService,
    public dialogRef: MatDialogRef<TodoDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public parentTodoId: string | null) 
    { 
        this.todo.parentTodoUId = parentTodoId;
        this.todoDialogTitle = this.todo.parentTodoUId ? 'Add Sub Todo' : 'Add Todo';
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
    this.todoService.addTodo(this.todo).pipe(
      catchError(error => {
        this.errorMessage = 'An error occurred while attempting to add the todo.';
        return of(null);
      })
    ).subscribe((addedTodo: Todo | null) => {
      if (addedTodo) {
        this.dialogRef.close(addedTodo);
      }
    });
  }

  onCloseClick(): void {
    this.dialogRef.close();
  }
}
