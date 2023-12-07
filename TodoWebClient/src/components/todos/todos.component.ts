import { Component, Inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TodoDialogComponent } from '../todo-dialog/todo-dialog.component';
import { TodoHttpService } from '../../services/todo-http.service';
import { DateValidationService } from '../../services/date-validation.service';
import { Todo } from '../../models/todo.model';
import { Subject, catchError, debounceTime, of } from 'rxjs';

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
    private todoService: TodoHttpService,
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
  today = new Date();

  ngOnInit() {
    this.getTodos();
  }
  
  getTodos() {
    this.todoService.getTodos().pipe(
      catchError(error => {
        console.error('An error occurred while attempting to get todos:', error);
        return of([]);
      })
    ).subscribe((todos: Todo[]) => {
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

  isPastDue(todo: Todo): any {
    if(todo.isComplete) return;  
    if(!this.dateValidationService.isValidDate(todo.dueDate)) return;
    return new Date(todo.dueDate!) < new Date();
  }
}

