import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TodosComponent } from '../components/todos/todos.component';
import { TodosListComponent } from '../components/todos-list/todos-list.component';

const routes: Routes = [
  { path: 'todos', component: TodosComponent },
  { path: 'todos-list', component: TodosListComponent },
  { path: '', redirectTo: '/todos', pathMatch: 'full' },
  { path: '**', redirectTo: '/todos' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
