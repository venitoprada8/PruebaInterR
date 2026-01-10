import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_CONFIG } from './api.config';
import { Subject, SubjectList } from '../models';

@Injectable({
  providedIn: 'root'
})
export class SubjectService {
  private apiUrl = `${API_CONFIG.baseUrl}/subjects`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<SubjectList[]> {
    return this.http.get<SubjectList[]>(this.apiUrl);
  }

  getById(id: string): Observable<Subject> {
    return this.http.get<Subject>(`${this.apiUrl}/${id}`);
  }

  getAvailableForStudent(studentId: string): Observable<SubjectList[]> {
    return this.http.get<SubjectList[]>(`${this.apiUrl}/available/${studentId}`);
  }
}
