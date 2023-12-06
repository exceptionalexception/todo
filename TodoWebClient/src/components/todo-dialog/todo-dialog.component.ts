import { Component, Inject, Input } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Todo } from '../../models/todo.model';

@Component({
  selector: 'todo-dialog',
  templateUrl: './todo-dialog.component.html',
  styleUrls: ['./todo-dialog.component.scss']
})
export class TodoDialogComponent{
  todo: Todo = new Todo('', new Date());
  todoDialogTitle: string = !this.todo?.todoUId ? 'Add Todo' : 'Edit Todo';


  constructor(
    public dialogRef: MatDialogRef<TodoDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Todo) { 
      todo: this.data;
    }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
