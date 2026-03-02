import { Inject, Injectable } from "@angular/core";
import { environment } from "../../environments/environment.prod";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { Command } from "../interfaces/command";

@Injectable({
  providedIn: 'root',
})
export class CommandService {
  private apiUrl = `${environment.apiUrl}/api/Command`;

  constructor(private http: HttpClient) { }
  
  getCommands(): Observable<Command[]> {
    return this.http.get<Command[]>(`${this.apiUrl}`);
  }
}