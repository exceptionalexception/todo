import { Injectable } from '@angular/core';
import { Todo } from '../models/todo.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AppConstants } from '../constants/app-constants';


@Injectable({
  providedIn: 'root'
})
export class TodoService {
 
  constructor(private httpClient: HttpClient) { }

  getTodos(): Observable<Todo[]> {
    return this.httpClient.get<Todo[]>(`${AppConstants.ApiRoot}/api/todo`);
  }

}
