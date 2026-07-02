import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Task, TaskCreateRequest, TaskUpdateRequest, Category } from '../models/task.model';
export interface PagedResponse<T> {
  items: T[];
  totalCount: number;
}
@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private readonly tasksUrl = '/api/TasksApi';
  private readonly categoriesUrl = '/api/CategoriesApi';

  constructor(private http: HttpClient) {}
// --- tasks-----
 getTasks(page: number = 1, pageSize: number = 10): Observable<any> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    // Результат: /api/TasksApi?page=1&pageSize=10
    return this.http.get<any>(`${this.tasksUrl}`, { params });
  }

  getTask(id: number): Observable<Task> {
    return this.http.get<Task>(`${this.tasksUrl}/${id}`);
  }
 getTasksByCategory(categoryId: number): Observable<any> {
  return this.http.get<Task[]>(`${this.tasksUrl}/category/${categoryId}`);
}
  createTask(task: TaskCreateRequest): Observable<Task> {
    return this.http.post<Task>(this.tasksUrl, task);
  }

  updateTask(id: number, task: TaskUpdateRequest): Observable<Task> {
    return this.http.put<Task>(`${this.tasksUrl}/${id}`, task);
  }

  deleteTask(id: number): Observable<void> {
    return this.http.delete<void>(`${this.tasksUrl}/${id}`);
  }
// --- Categories-----
getCategories(page: number = 1, pageSize: number = 16): Observable<any> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    // БУЛО: `${this.apiUrl}/CategoriesApi/categories`
    // МАЄ БУТИ: /api/CategoriesApi?page=1&pageSize=16
    return this.http.get<any>(`${this.categoriesUrl}`, { params });
  }

  createCategory(name: string): Observable<Category> {
    return this.http.post<Category>(this.categoriesUrl, { name });
  }

  deleteCategory(id: number): Observable<void> {
    return this.http.delete<void>(`${this.categoriesUrl}/${id}`);
  }
  // --- Додайте цей метод до TaskService ---
searchTasksByText(searchText: string): Observable<Task[]> {
  const params = new HttpParams().set('query', searchText);
  
  return this.http.get<Task[]>(`${this.tasksUrl}/search/${searchText}`,);
}
 
}
