import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_CONFIG } from './api.config';
import { Student, StudentList, CreateStudent, UpdateStudent, Classmate } from '../models';

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  private apiUrl = `${API_CONFIG.baseUrl}/students`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<StudentList[]> {
    return this.http.get<StudentList[]>(this.apiUrl);
  }

  getById(id: string): Observable<Student> {
    return this.http.get<Student>(`${this.apiUrl}/${id}`);
  }

  create(student: CreateStudent): Observable<Student> {
    return this.http.post<Student>(this.apiUrl, student);
  }

  update(id: string, student: UpdateStudent): Observable<Student> {
    return this.http.put<Student>(`${this.apiUrl}/${id}`, student);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getClassmates(id: string): Observable<Classmate[]> {
    return this.http.get<Classmate[]>(`${this.apiUrl}/${id}/classmates`);
  }
}
