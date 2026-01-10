export interface Inscription {
  id: string;
  subjectId: string;
  subjectName: string;
  subjectCode: string;
  credits: number;
  teacherName: string;
  inscriptionDate: Date;
}

export interface CreateInscription {
  studentId: string;
  subjectIds: string[];
}

export interface Classmate {
  studentId: string;
  firstName: string;
  lastName: string;
  subjectId: string;
  subjectName: string;
}
