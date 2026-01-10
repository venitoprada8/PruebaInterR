using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Application.DTOs;
using StudentRegistration.Application.Services;
using StudentRegistration.Domain.Exceptions;

namespace StudentRegistration.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InscriptionsController : ControllerBase
{
    private readonly IInscriptionService _inscriptionService;

    public InscriptionsController(IInscriptionService inscriptionService)
    {
        _inscriptionService = inscriptionService;
    }

    [HttpPost]
    public async Task<ActionResult<StudentDto>> InscribeStudent(CreateInscriptionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var student = await _inscriptionService.InscribeStudentAsync(dto, cancellationToken);
            return Ok(student);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (DomainException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{studentId}/subjects/{subjectId}")]
    public async Task<IActionResult> UnscribeStudent(Guid studentId, Guid subjectId, CancellationToken cancellationToken)
    {
        try
        {
            await _inscriptionService.UnscribeStudentAsync(studentId, subjectId, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("students/{studentId}")]
    public async Task<ActionResult<IEnumerable<InscriptionDto>>> GetStudentInscriptions(
        Guid studentId,
        CancellationToken cancellationToken)
    {
        var inscriptions = await _inscriptionService.GetStudentInscriptionsAsync(studentId, cancellationToken);
        return Ok(inscriptions);
    }

    [HttpGet("subjects/{subjectId}/students")]
    public async Task<ActionResult<IEnumerable<StudentListDto>>> GetSubjectStudents(
        Guid subjectId,
        CancellationToken cancellationToken)
    {
        var students = await _inscriptionService.GetSubjectStudentsAsync(subjectId, cancellationToken);
        return Ok(students);
    }
}
