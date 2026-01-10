import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { StudentService } from '../../services/student.service';
import { TranslationService } from '../../services/translation.service';
import { AppTexts } from '../../constants/texts';

@Component({
  selector: 'app-student-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './student-form.component.html',
  styleUrl: './student-form.component.scss'
})
export class StudentFormComponent implements OnInit {
  studentForm: FormGroup;
  isEditMode = false;
  studentId: string | null = null;
  loading = false;
  error: string | null = null;

  // Textos del componente
  get t(): AppTexts {
    return this.translationService.texts;
  }

  constructor(
    private fb: FormBuilder,
    private studentService: StudentService,
    private router: Router,
    private route: ActivatedRoute,
    public translationService: TranslationService
  ) {
    this.studentForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  ngOnInit(): void {
    this.studentId = this.route.snapshot.paramMap.get('id');
    if (this.studentId && this.studentId !== 'new') {
      this.isEditMode = true;
      this.loadStudent(this.studentId);
    }
  }

  loadStudent(id: string): void {
    this.loading = true;
    this.studentService.getById(id).subscribe({
      next: (student) => {
        this.studentForm.patchValue({
          firstName: student.firstName,
          lastName: student.lastName,
          email: student.email
        });
        this.loading = false;
      },
      error: (err) => {
        this.error = this.t.studentForm.errorLoading;
        this.loading = false;
        console.error(err);
      }
    });
  }

  onSubmit(): void {
    if (this.studentForm.valid) {
      this.loading = true;
      this.error = null;

      const studentData = this.studentForm.value;

      const operation = this.isEditMode && this.studentId
        ? this.studentService.update(this.studentId, studentData)
        : this.studentService.create(studentData);

      operation.subscribe({
        next: () => {
          this.router.navigate(['/students']);
        },
        error: (err) => {
          this.error = err.error?.message || this.t.studentForm.errorSaving;
          this.loading = false;
          console.error(err);
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/students']);
  }
}
