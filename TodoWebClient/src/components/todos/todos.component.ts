import { Component, Inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TodoDialogComponent } from '../todo-dialog/todo-dialog.component';
import { TodoService } from '../../services/todo.service';
import { DateValidationService } from '../../services/date-validation.service';
import { Todo } from '../../models/todo.model';
import { Subject, debounceTime } from 'rxjs';

@Component({
  selector: 'todos',
  templateUrl: './todos.component.html',
  styleUrl: './todos.component.scss'
})
export class TodosComponent {
  updateDueDate$ = new Subject<{ newValue: Date, todo: any }>();
  updateTodoText$ = new Subject<{ newValue: string, todo: any }>();


  constructor(
    @Inject(MatDialog) public dialog: MatDialog,
    private todoService: TodoService,
    private dateValidationService: DateValidationService) { 

      this.updateDueDate$.pipe(debounceTime(500))
        .subscribe(({ newValue, todo }) => {
          var todoToUpdate = this.todos.find(t => t.todoUId === todo.todoUId);
          if(!todoToUpdate) return;
          todoToUpdate.dueDate = newValue;
          this.updateTodo(todoToUpdate);
        });

      this.updateTodoText$.pipe(debounceTime(500))
        .subscribe(({ newValue, todo }) => {
          var todoToUpdate = this.todos.find(t => t.todoUId === todo.todoUId);
          if(!todoToUpdate) return;
          todoToUpdate.todoText = newValue;
          this.updateTodo(todoToUpdate);
        });
    }

    
  todoColumns = ['todo', 'dueDate', 'createdDate', 'isComplete'];
  todos: Todo[] = [];
  errorMessage: string = '';

  ngOnInit() {
    this.getTodos();
  }
  
  getTodos() {
    this.todoService.getTodos().subscribe((todos: Todo[]) => {
      this.todos = todos || [];
    });
  }

  openTodoDialog(todo?: Todo | null): void {
    let dialogRef = this.dialog.open(TodoDialogComponent, {
      width: '30%',
      data: todo?.todoUId || null
    });

    dialogRef.afterClosed().subscribe((newTodo: Todo) => {
      this.getTodos();
    });
  }

  deleteTodo(todo: Todo) {
    if(!todo?.todoUId) return;
    this.todoService.deleteTodo(todo.todoUId).subscribe(() => this.getTodos());
  }

  completeTodo(todo: Todo) {
    if(!todo?.todoUId) return;
    this.todoService.completeTodo(todo.todoUId).subscribe(() => this.getTodos());
  }

  updateTodo(todo: Todo) {
    this.todoService.updateTodo(todo).subscribe((todo: Todo) => {});
  }

  emitUpdateDueDate(event: any, todo: any) {
    const newValue = event.target.value;
    if(!this.dateValidationService.isValidDate(newValue)) return;
    this.updateDueDate$.next({ newValue, todo });
  }

  emitUpdateTodoText(event: any, todo: any) {
    const newValue = event.target.value;
    this.updateTodoText$.next({ newValue, todo });
  }
}

