import { Inscription } from './inscription.model';

export interface Student {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  registrationDate: Date;
  totalCredits: number;
  inscriptions: Inscription[];
}

export interface StudentList {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  totalCredits: number;
}

export interface CreateStudent {
  firstName: string;
  lastName: string;
  email: string;
}

export interface UpdateStudent {
  firstName: string;
  lastName: string;
  email: string;
}
