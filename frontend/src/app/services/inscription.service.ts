import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_CONFIG } from './api.config';
import { CreateInscription, Inscription, Student, StudentList } from '../models';

@Injectable({
  providedIn: 'root',
})
export class InscriptionService {
  private apiUrl = `${API_CONFIG.baseUrl}/inscriptions`;

  constructor(private http: HttpClient) {}

  inscribeStudent(inscription: CreateInscription): Observable<Student> {
    return this.http.post<Student>(this.apiUrl, inscription);
  }

  unscribeStudent(studentId: string, subjectId: string): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${studentId}/subjects/${subjectId}`
    );
  }

  getStudentInscriptions(studentId: string): Observable<Inscription[]> {
    return this.http.get<Inscription[]>(`${this.apiUrl}/students/${studentId}`);
  }

  getSubjectStudents(subjectId: string): Observable<StudentList[]> {
    return this.http.get<StudentList[]>(
      `${this.apiUrl}/subjects/${subjectId}/students`
    );
  }
}
