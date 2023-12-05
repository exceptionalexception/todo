import { Component } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material';
import { TodoDialogComponent } from '../todo-dialog/todo-dialog.component';

@Component({
  selector: 'todos',
  templateUrl: './todos.component.html',
  styleUrl: './todos.component.scss'
})
export class TodosComponent {

  constructor(public dialog: MatDialog) {}

  todoColumns = ['todo', 'dueDate'];

   todos = [
    { text: 'Jump', dueDate: '', subTodos: [] },
    { text: 'Read', dueDate: '', subTodos: [] },
    { text: 'Shower', dueDate: '', subTodos: [] },
    { text: 'Think', dueDate: '', subTodos: [] },
    { text: 'Breathe', dueDate: '', subTodos: [] },
  ];

  openTodoDialog(todoId: number): void {
    let dialogRef = this.dialog.open(TodoDialogComponent, {
      width: '500px',
      data: { todoId: todoId }
    });

    dialogRef.afterClosed().subscribe(newTodo => {
      console.log('The dialog was closed');
      this.todos.push(newTodo);
    });
  }
}

