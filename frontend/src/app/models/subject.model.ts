import { Teacher } from './teacher.model';

export interface Subject {
  id: string;
  code: string;
  name: string;
  credits: number;
  teacher: Teacher;
}

export interface SubjectList {
  id: string;
  code: string;
  name: string;
  credits: number;
  teacherId: string;
  teacherName: string;
}
