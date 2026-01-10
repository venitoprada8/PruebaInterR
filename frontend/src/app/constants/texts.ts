// Archivo centralizado de textos para facilitar traducciones y mantenimiento

export interface AppTexts {
  // Common
  common: {
    loading: string;
    error: string;
    success: string;
    cancel: string;
    save: string;
    edit: string;
    delete: string;
    view: string;
    back: string;
    create: string;
    update: string;
    email: string;
    credits: string;
    teacher: string;
  };

  // Student List
  studentList: {
    title: string;
    newStudent: string;
    loadingStudents: string;
    totalCredits: string;
    enroll: string;
    classmates: string;
    noStudentsMessage: string;
    deleteConfirmation: string;
  };

  // Student Form
  studentForm: {
    titleNew: string;
    titleEdit: string;
    firstName: string;
    lastName: string;
    email: string;
    firstNameRequired: string;
    lastNameRequired: string;
    emailRequired: string;
    saving: string;
    creating: string;
    updating: string;
    errorLoading: string;
    errorSaving: string;
  };

  // Subject Enrollment
  enrollment: {
    title: string;
    currentCredits: string;
    currentSubjects: string;
    alreadyEnrolledIn: string;
    selectSubjects: string;
    selectUpTo: string;
    cannotSelectSameTeacher: string;
    maxSubjectsReached: string;
    noSubjectsAvailable: string;
    pleaseSelectOne: string;
    enrolling: string;
    enrollInSelected: string;
    successMessage: string;
    errorEnrolling: string;
    errorLoading: string;
    errorLoadingSubjects: string;
  };

  // Classmates View
  classmates: {
    title: string;
    totalCredits: string;
    loadingClassmates: string;
    classmatesCount: string;
    noClassmatesFound: string;
    notEnrolledYet: string;
    noClassmatesInSubjects: string;
    backToList: string;
  };

  // Error Messages
  errors: {
    general: string;
    network: string;
    notFound: string;
    unauthorized: string;
    serverError: string;
  };
}

// Textos en español
export const TEXTS_ES: AppTexts = {
  common: {
    loading: 'Cargando...',
    error: 'Error',
    success: 'Éxito',
    cancel: 'Cancelar',
    save: 'Guardar',
    edit: 'Editar',
    delete: 'Eliminar',
    view: 'Ver',
    back: 'Volver',
    create: 'Crear',
    update: 'Actualizar',
    email: 'Email',
    credits: 'Créditos',
    teacher: 'Profesor',
  },

  studentList: {
    title: 'Sistema de Registro de Estudiantes',
    newStudent: '+ Nuevo Estudiante',
    loadingStudents: 'Cargando estudiantes...',
    totalCredits: 'Total Créditos',
    enroll: 'Inscribir',
    classmates: 'Compañeros',
    noStudentsMessage: 'No hay estudiantes registrados aún. Haz clic en "Nuevo Estudiante" para crear uno.',
    deleteConfirmation: '¿Estás seguro de que quieres eliminar este estudiante?',
  },

  studentForm: {
    titleNew: 'Nuevo Estudiante',
    titleEdit: 'Editar Estudiante',
    firstName: 'Primer Nombre',
    lastName: 'Apellidos',
    email: 'Email',
    firstNameRequired: 'Primer Nombre es requerido (mínimo 2 caracteres)',
    lastNameRequired: 'Apellidos es requerido (mínimo 2 caracteres)',
    emailRequired: 'Email válido es requerido',
    saving: 'Guardando...',
    creating: 'Crear',
    updating: 'Actualizar',
    errorLoading: 'Error al cargar estudiante',
    errorSaving: 'Error al guardar estudiante',
  },

  enrollment: {
    title: 'Inscribirse en Materias',
    currentCredits: 'Créditos Actuales',
    currentSubjects: 'Materias Actuales',
    alreadyEnrolledIn: 'Ya Inscrito En',
    selectSubjects: 'Seleccionar Materias',
    selectUpTo: 'Selecciona hasta',
    cannotSelectSameTeacher: 'No puedes seleccionar múltiples materias del mismo profesor',
    maxSubjectsReached: 'Solo puedes seleccionar un máximo de',
    noSubjectsAvailable: 'No hay materias disponibles para inscripción.',
    pleaseSelectOne: 'Por favor selecciona al menos una materia',
    enrolling: 'Inscribiendo...',
    enrollInSelected: 'Inscribirse en Materias Seleccionadas',
    successMessage: '¡Inscripción exitosa en las materias!',
    errorEnrolling: 'Error al inscribirse en las materias',
    errorLoading: 'Error al cargar estudiante',
    errorLoadingSubjects: 'Error al cargar materias disponibles',
  },

  classmates: {
    title: 'Compañeros de Clase',
    totalCredits: 'Total Créditos',
    loadingClassmates: 'Cargando compañeros...',
    classmatesCount: 'compañero(s)',
    noClassmatesFound: 'No se encontraron compañeros.',
    notEnrolledYet: 'Aún no estás inscrito en ninguna materia.',
    noClassmatesInSubjects: 'No tienes compañeros en tus materias inscritas.',
    backToList: 'Volver a Lista de Estudiantes',
  },

  errors: {
    general: 'Ocurrió un error inesperado',
    network: 'Error de conexión. Verifica tu internet.',
    notFound: 'Recurso no encontrado',
    unauthorized: 'No autorizado',
    serverError: 'Error del servidor',
  },
};

// Textos en inglés
export const TEXTS_EN: AppTexts = {
  common: {
    loading: 'Loading...',
    error: 'Error',
    success: 'Success',
    cancel: 'Cancel',
    save: 'Save',
    edit: 'Edit',
    delete: 'Delete',
    view: 'View',
    back: 'Back',
    create: 'Create',
    update: 'Update',
    email: 'Email',
    credits: 'Credits',
    teacher: 'Teacher',
  },

  studentList: {
    title: 'Student Registration System',
    newStudent: '+ New Student',
    loadingStudents: 'Loading students...',
    totalCredits: 'Total Credits',
    enroll: 'Enroll',
    classmates: 'Classmates',
    noStudentsMessage: 'No students registered yet. Click "New Student" to create one.',
    deleteConfirmation: 'Are you sure you want to delete this student?',
  },

  studentForm: {
    titleNew: 'New Student',
    titleEdit: 'Edit Student',
    firstName: 'First Name',
    lastName: 'Last Name',
    email: 'Email',
    firstNameRequired: 'First Name is required (minimum 2 characters)',
    lastNameRequired: 'Last Name is required (minimum 2 characters)',
    emailRequired: 'Valid Email is required',
    saving: 'Saving...',
    creating: 'Create',
    updating: 'Update',
    errorLoading: 'Error loading student',
    errorSaving: 'Error saving student',
  },

  enrollment: {
    title: 'Enroll in Subjects',
    currentCredits: 'Current Credits',
    currentSubjects: 'Current Subjects',
    alreadyEnrolledIn: 'Already Enrolled In',
    selectSubjects: 'Select Subjects',
    selectUpTo: 'Select up to',
    cannotSelectSameTeacher: 'You cannot select multiple subjects from the same teacher',
    maxSubjectsReached: 'You can only select a maximum of',
    noSubjectsAvailable: 'No subjects available for enrollment.',
    pleaseSelectOne: 'Please select at least one subject',
    enrolling: 'Enrolling...',
    enrollInSelected: 'Enroll in Selected Subjects',
    successMessage: 'Successfully enrolled in subjects!',
    errorEnrolling: 'Error enrolling in subjects',
    errorLoading: 'Error loading student',
    errorLoadingSubjects: 'Error loading available subjects',
  },

  classmates: {
    title: 'Classmates',
    totalCredits: 'Total Credits',
    loadingClassmates: 'Loading classmates...',
    classmatesCount: 'classmate(s)',
    noClassmatesFound: 'No classmates found.',
    notEnrolledYet: 'You are not enrolled in any subjects yet.',
    noClassmatesInSubjects: "You don't have any classmates in your enrolled subjects.",
    backToList: 'Back to Students List',
  },

  errors: {
    general: 'An unexpected error occurred',
    network: 'Connection error. Check your internet.',
    notFound: 'Resource not found',
    unauthorized: 'Unauthorized',
    serverError: 'Server error',
  },
};

// Tipo para los idiomas disponibles
export type Language = 'es' | 'en';

// Mapeo de idiomas a textos
export const TEXTS_MAP: Record<Language, AppTexts> = {
  es: TEXTS_ES,
  en: TEXTS_EN,
};
