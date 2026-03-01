import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../interfaces/user';
import { SaltTask } from '../interfaces/salt-task';
import { TaskUpdateForOneUser } from '../interfaces/request-interfaces/task-update-for-one-user';

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  private apiUrl = `${environment.apiUrl}/api/SaltTask`;

  constructor(private http: HttpClient) {}

  getTasks(): Observable<SaltTask[]> {
    return this.http.get<SaltTask[]>(this.apiUrl);
  }
  
  getTaskById(id: number): Observable<SaltTask> {
    return this.http.get<SaltTask>(`${this.apiUrl}/${id}`);
  }
  
  taskUpdateForOneUser(req: TaskUpdateForOneUser): Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}/TaskUpdateForOneUser`, req);
  }
}
