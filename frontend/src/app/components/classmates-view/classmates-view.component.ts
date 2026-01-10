import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { StudentService } from '../../services/student.service';
import { TranslationService } from '../../services/translation.service';
import { Classmate, Student } from '../../models';
import { AppTexts } from '../../constants/texts';

interface GroupedClassmates {
  subjectName: string;
  classmates: Classmate[];
}

@Component({
  selector: 'app-classmates-view',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './classmates-view.component.html',
  styleUrl: './classmates-view.component.scss'
})
export class ClassmatesViewComponent implements OnInit {
  studentId: string | null = null;
  student: Student | null = null;
  groupedClassmates: GroupedClassmates[] = [];
  loading = false;
  error: string | null = null;

  // Textos del componente
  get t(): AppTexts {
    return this.translationService.texts;
  }

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private studentService: StudentService,
    public translationService: TranslationService
  ) {}

  ngOnInit(): void {
    this.studentId = this.route.snapshot.paramMap.get('id');
    if (this.studentId) {
      this.loadStudentAndClassmates();
    }
  }

  loadStudentAndClassmates(): void {
    this.loading = true;
    this.error = null;

    if (!this.studentId) return;

    this.studentService.getById(this.studentId).subscribe({
      next: (student) => {
        this.student = student;
        this.loadClassmates();
      },
      error: (err) => {
        this.error = this.t.errors.general;
        this.loading = false;
        console.error(err);
      }
    });
  }

  loadClassmates(): void {
    if (!this.studentId) return;

    this.studentService.getClassmates(this.studentId).subscribe({
      next: (classmates) => {
        this.groupClassmatesBySubject(classmates);
        this.loading = false;
      },
      error: (err) => {
        this.error = this.t.errors.general;
        this.loading = false;
        console.error(err);
      }
    });
  }

  groupClassmatesBySubject(classmates: Classmate[]): void {
    const grouped = new Map<string, Classmate[]>();

    classmates.forEach(classmate => {
      if (!grouped.has(classmate.subjectName)) {
        grouped.set(classmate.subjectName, []);
      }
      grouped.get(classmate.subjectName)!.push(classmate);
    });

    this.groupedClassmates = Array.from(grouped.entries()).map(([subjectName, classmates]) => ({
      subjectName,
      classmates
    }));
  }

  goBack(): void {
    this.router.navigate(['/students']);
  }
}
