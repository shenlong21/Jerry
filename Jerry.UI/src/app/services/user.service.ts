import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface ApiUser {
  id: string; // UUID
  name: string | null;
  hostname: string | null;
  project: string | null;
  ipAddress: string | null;
  password: string | null;
  grubPassword: string | null;
  lastConnected: string; // date-time string
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = `${environment.apiUrl}/api/Users`;

  constructor(private http: HttpClient) { }

  getUsers(): Observable<ApiUser[]> {
    return this.http.get<ApiUser[]>(this.apiUrl);
  }
}
