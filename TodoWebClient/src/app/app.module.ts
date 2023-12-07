import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
import { SharedModule } from '../shared/shared.module';
import { TodoTilesComponent } from '../components/todo-tiles/todo-tiles.component';
import { TodoDialogComponent } from '../components/todo-dialog/todo-dialog.component';
import { HttpClientModule, provideHttpClient, withFetch } from '@angular/common/http';
import { TodoListComponent } from '../components/todos-list/todo-list.component';


@NgModule({
  declarations: [
    AppComponent,
    TodoDialogComponent,
    TodoTilesComponent,
    TodoListComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    SharedModule,
    HttpClientModule
  ],
  providers: [
    provideClientHydration(),
    provideHttpClient(withFetch())
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
