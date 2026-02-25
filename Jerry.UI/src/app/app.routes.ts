import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { TasksComponent } from './pages/tasks/tasks.component';
import { UsersComponent } from './pages/users/users.component';
import { TaskDetailsComponent } from './pages/task-details/task-details.component';

export const routes: Routes = [
    { path: '', component: HomeComponent, title: 'Jerry - Home' },
    { path: 'tasks', component: TasksComponent, title: 'Jerry - Tasks' },
    { path: 'tasks/:id', component: TaskDetailsComponent, title: 'Jerry - Task Details' },
    { path: 'users', component: UsersComponent, title: 'Jerry - Users' }
];
