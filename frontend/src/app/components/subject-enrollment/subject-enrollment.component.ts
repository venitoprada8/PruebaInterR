import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { SubjectService } from '../../services/subject.service';
import { InscriptionService } from '../../services/inscription.service';
import { StudentService } from '../../services/student.service';
import { TranslationService } from '../../services/translation.service';
import { SubjectList, Student } from '../../models';
import { AppTexts } from '../../constants/texts';

@Component({
  selector: 'app-subject-enrollment',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './subject-enrollment.component.html',
  styleUrl: './subject-enrollment.component.scss'
})
export class SubjectEnrollmentComponent implements OnInit {
  studentId: string | null = null;
  student: Student | null = null;
  availableSubjects: SubjectList[] = [];
  selectedSubjects: Set<string> = new Set();
  loading = false;
  error: string | null = null;
  success: string | null = null;

  readonly MAX_SUBJECTS = 3;

  // Textos del componente
  get t(): AppTexts {
    return this.translationService.texts;
  }

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private subjectService: SubjectService,
    private inscriptionService: InscriptionService,
    private studentService: StudentService,
    public translationService: TranslationService
  ) {}

  ngOnInit(): void {
    this.studentId = this.route.snapshot.paramMap.get('id');
    if (this.studentId) {
      this.loadStudentAndSubjects();
    }
  }

  loadStudentAndSubjects(): void {
    this.loading = true;
    this.error = null;

    if (!this.studentId) return;

    this.studentService.getById(this.studentId).subscribe({
      next: (student) => {
        this.student = student;
        this.loadAvailableSubjects();
      },
      error: (err) => {
        this.error = this.t.enrollment.errorLoading;
        this.loading = false;
        console.error(err);
      }
    });
  }

  loadAvailableSubjects(): void {
    if (!this.studentId) return;

    this.subjectService.getAvailableForStudent(this.studentId).subscribe({
      next: (subjects) => {
        this.availableSubjects = subjects;
        this.loading = false;
      },
      error: (err) => {
        this.error = this.t.enrollment.errorLoadingSubjects;
        this.loading = false;
        console.error(err);
      }
    });
  }

  toggleSubject(subjectId: string): void {
    if (this.selectedSubjects.has(subjectId)) {
      this.selectedSubjects.delete(subjectId);
    } else {
      if (this.selectedSubjects.size >= this.MAX_SUBJECTS) {
        this.error = `${this.t.enrollment.maxSubjectsReached} ${this.MAX_SUBJECTS} ${this.t.enrollment.selectSubjects.toLowerCase()}`;
        return;
      }

      // Check if teacher is already selected
      const selectedSubject = this.availableSubjects.find(s => s.id === subjectId);
      if (selectedSubject) {
        const teacherAlreadySelected = Array.from(this.selectedSubjects).some(id => {
          const subject = this.availableSubjects.find(s => s.id === id);
          return subject?.teacherId === selectedSubject.teacherId;
        });

        if (teacherAlreadySelected) {
          this.error = this.t.enrollment.cannotSelectSameTeacher;
          return;
        }
      }

      this.selectedSubjects.add(subjectId);
      this.error = null;
    }
  }

  isSelected(subjectId: string): boolean {
    return this.selectedSubjects.has(subjectId);
  }

  canSelectMore(): boolean {
    return this.selectedSubjects.size < this.MAX_SUBJECTS;
  }

  getRemainingSlots(): number {
    return this.MAX_SUBJECTS - (this.student?.inscriptions.length || 0) - this.selectedSubjects.size;
  }

  enroll(): void {
    if (this.selectedSubjects.size === 0) {
      this.error = this.t.enrollment.pleaseSelectOne;
      return;
    }

    if (!this.studentId) return;

    this.loading = true;
    this.error = null;

    const enrollment = {
      studentId: this.studentId,
      subjectIds: Array.from(this.selectedSubjects)
    };

    this.inscriptionService.inscribeStudent(enrollment).subscribe({
      next: () => {
        this.success = this.t.enrollment.successMessage;
        setTimeout(() => {
          this.router.navigate(['/students']);
        }, 2000);
      },
      error: (err) => {
        this.error = err.error?.message || this.t.enrollment.errorEnrolling;
        this.loading = false;
        console.error(err);
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/students']);
  }
}
