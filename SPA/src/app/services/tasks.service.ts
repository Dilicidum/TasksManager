import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Tasks } from '../models/tasks';
@Injectable({
  providedIn: 'root',
})
export class TasksService {
  baseUrl: string = 'http://localhost:5292/Tasks';
  constructor(private http: HttpClient) {}

  getTasks(status: string = '', taskType: string = ''): Observable<Tasks[]> {
    return this.http.get<Tasks[]>(
      this.baseUrl + '/?Status=' + status + '&TaskType=' + taskType
    );
  }

  deleteTask(id: number) {
    return this.http.delete(this.baseUrl + '/' + id);
  }

  createTask(task: Tasks): Observable<any> {
    return this.http.post(this.baseUrl, task);
  }

  updateTask(task: Tasks): Observable<any> {
    return this.http.put(this.baseUrl + '/' + task.id, task);
  }

  getTaskById(id: number): Observable<Tasks> {
    return this.http.get<Tasks>(this.baseUrl + '/' + id);
  }
}