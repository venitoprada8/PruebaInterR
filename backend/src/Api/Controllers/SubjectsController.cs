using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Application.DTOs;
using StudentRegistration.Application.Services;

namespace StudentRegistration.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectsController : ControllerBase
{
    private readonly ISubjectService _subjectService;

    public SubjectsController(ISubjectService subjectService)
    {
        _subjectService = subjectService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubjectListDto>>> GetAll(CancellationToken cancellationToken)
    {
        var subjects = await _subjectService.GetAllAsync(cancellationToken);
        return Ok(subjects);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SubjectDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var subject = await _subjectService.GetByIdAsync(id, cancellationToken);

        if (subject == null)
            return NotFound(new { message = $"Asignatura con ID {id} no encontrada." });

        return Ok(subject);
    }

    [HttpGet("available/{studentId}")]
    public async Task<ActionResult<IEnumerable<SubjectListDto>>> GetAvailableForStudent(
        Guid studentId,
        CancellationToken cancellationToken)
    {
        var subjects = await _subjectService.GetAvailableSubjectsForStudentAsync(studentId, cancellationToken);
        return Ok(subjects);
    }
}
