import { Routes } from '@angular/router';
import { StudentListComponent } from './components/student-list/student-list.component';
import { StudentFormComponent } from './components/student-form/student-form.component';
import { SubjectEnrollmentComponent } from './components/subject-enrollment/subject-enrollment.component';
import { ClassmatesViewComponent } from './components/classmates-view/classmates-view.component';

export const routes: Routes = [
  { path: '', redirectTo: '/students', pathMatch: 'full' },
  { path: 'students', component: StudentListComponent },
  { path: 'students/new', component: StudentFormComponent },
  { path: 'students/:id', component: StudentFormComponent },
  { path: 'students/:id/edit', component: StudentFormComponent },
  { path: 'students/:id/enroll', component: SubjectEnrollmentComponent },
  { path: 'students/:id/classmates', component: ClassmatesViewComponent },
  { path: '**', redirectTo: '/students' }
];
