namespace StudentRegistration.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }

    public DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

public class MaxSubjectsExceededException : DomainException
{
    public MaxSubjectsExceededException()
        : base("Un estudiante solo puede inscribirse en un máximo de 3 asignaturas.")
    {
    }
}

public class DuplicateTeacherException : DomainException
{
    public DuplicateTeacherException()
        : base("Un estudiante no puede tener múltiples asignaturas con el mismo profesor.")
    {
    }
}

public class StudentAlreadyEnrolledException : DomainException
{
    public StudentAlreadyEnrolledException(string subjectName)
        : base($"El estudiante ya está inscrito en la asignatura: {subjectName}")
    {
    }
}

public class MaxCreditsExceededException : DomainException
{
    public MaxCreditsExceededException()
        : base("Un estudiante solo puede acumular un máximo de 9 créditos.")
    {
    }
}
