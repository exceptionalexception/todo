import { Injectable } from '@angular/core';
import { Todo } from '../models/todo.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AppConstants } from '../constants/app-constants';


@Injectable({
  providedIn: 'root'
})
export class TodoHttpService {
  constructor(private httpClient: HttpClient) { }

  getTodos(): Observable<Todo[]> {
    return this.httpClient.get<Todo[]>(`${AppConstants.ApiRoot}/todo`);
  }

  addTodo(todo: Todo): Observable<Todo> {
    return this.httpClient.post<Todo>(`${AppConstants.ApiRoot}/todo/add`, todo);
  }

  completeTodo(todoUId: string): Observable<any> {
    return this.httpClient.put(`${AppConstants.ApiRoot}/todo/${todoUId}/complete`, { todoUId });
  }
  
  deleteTodo(todoUId: string): Observable<any> {
    return this.httpClient.delete(`${AppConstants.ApiRoot}/todo/${todoUId}`);
  }

  updateTodo(todo: Todo): Observable<Todo> {
    return this.httpClient.put<Todo>(`${AppConstants.ApiRoot}/todo/update`, todo);
  }
}
