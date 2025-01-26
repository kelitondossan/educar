using Microsoft.AspNetCore.Mvc;
using AcademicApp.Domain.Services;
using AcademicApp.API.Dtos;
using AcademicApp.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace AcademicApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly StudentService _studentService;

        public StudentsController(StudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetAll()
        {
            var students = await _studentService.GetAllAsync();
            return Ok(students);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Student>> GetById(Guid id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null) return NotFound();
            return Ok(student);
        }

        [HttpPost]
        public async Task<ActionResult<Student>> Create([FromBody] StudentCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newStudent = new Student
            {
                Name = dto.Name!,
                Email = dto.Email!,
                RA = dto.RA!,
                CPF = dto.CPF!
            };

            try
            {
                var created = await _studentService.CreateStudentAsync(newStudent);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] StudentUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _studentService.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.Name = dto.Name!;
            existing.Email = dto.Email!;

            try
            {
                var updated = await _studentService.UpdateStudentAsync(existing);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _studentService.DeleteStudentAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
