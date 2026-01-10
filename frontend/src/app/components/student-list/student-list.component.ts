import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { StudentService } from '../../services/student.service';
import { TranslationService } from '../../services/translation.service';
import { StudentList } from '../../models';
import { AppTexts } from '../../constants/texts';

@Component({
  selector: 'app-student-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './student-list.component.html',
  styleUrl: './student-list.component.scss'
})
export class StudentListComponent implements OnInit {
  students: StudentList[] = [];
  loading = false;
  error: string | null = null;

  // Textos del componente
  get t(): AppTexts {
    return this.translationService.texts;
  }

  constructor(
    private studentService: StudentService,
    private router: Router,
    public translationService: TranslationService
  ) {}

  ngOnInit(): void {
    this.loadStudents();
  }

  loadStudents(): void {
    this.loading = true;
    this.error = null;
    this.studentService.getAll().subscribe({
      next: (data) => {
        this.students = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = this.t.errors.general;
        this.loading = false;
        console.error(err);
      }
    });
  }

  viewStudent(id: string): void {
    this.router.navigate(['/students', id]);
  }

  editStudent(id: string): void {
    this.router.navigate(['/students', id, 'edit']);
  }

  enrollStudent(id: string): void {
    this.router.navigate(['/students', id, 'enroll']);
  }

  viewClassmates(id: string): void {
    this.router.navigate(['/students', id, 'classmates']);
  }

  deleteStudent(id: string): void {
    if (confirm(this.t.studentList.deleteConfirmation)) {
      this.studentService.delete(id).subscribe({
        next: () => {
          this.loadStudents();
        },
        error: (err) => {
          this.error = this.t.errors.general;
          console.error(err);
        }
      });
    }
  }

  navigateToCreate(): void {
    this.router.navigate(['/students/new']);
  }
}
