using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Application.DTOs;
using StudentRegistration.Application.Services;

namespace StudentRegistration.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentListDto>>> GetAll(CancellationToken cancellationToken)
    {
        var students = await _studentService.GetAllAsync(cancellationToken);
        return Ok(students);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var student = await _studentService.GetByIdAsync(id, cancellationToken);

        if (student == null)
            return NotFound(new { message = $"Estudiante con ID {id} no encontrado." });

        return Ok(student);
    }

    [HttpPost]
    public async Task<ActionResult<StudentDto>> Create(CreateStudentDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var student = await _studentService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<StudentDto>> Update(Guid id, UpdateStudentDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var student = await _studentService.UpdateAsync(id, dto, cancellationToken);
            return Ok(student);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _studentService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{id}/classmates")]
    public async Task<ActionResult<IEnumerable<ClassmateDto>>> GetClassmates(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var classmates = await _studentService.GetStudentClassmatesAsync(id, cancellationToken);
            return Ok(classmates);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
