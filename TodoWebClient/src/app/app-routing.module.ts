import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TodoTilesComponent as TodoTilesComponent } from '../components/todo-tiles/todo-tiles.component';
import { TodoListComponent } from '../components/todos-list/todo-list.component';

const routes: Routes = [
  { path: 'todo-list', component: TodoListComponent },
  { path: 'todo-tiles', component: TodoTilesComponent },
  { path: '', redirectTo: '/todo-list', pathMatch: 'full' },
  { path: '**', redirectTo: '/todo-list' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
