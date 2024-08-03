using CollegeApp.Data;
using CollegeApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly CollegeDBContext _context;
        private readonly ILogger<StudentController> _logger;

        public StudentController(CollegeDBContext context, ILogger<StudentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            _logger.LogInformation("Get Students method started");
            var students = await _context.Students
                .Select(item => new StudentDTO
                {
                    Id = item.Id,
                    StudentName = item.StudentName,
                    Email = item.Email,
                    Address = item.Address,
                    DOB = item.DOB
                })
                .ToListAsync();

            return Ok(students);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> GetStudentById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Bad Request: {id}", id);
                return BadRequest("Dữ liệu không hợp lệ, vui lòng nhập lại");
            }

            var student = await _context.Students
                .Where(s => s.Id == id)
                .Select(s => new StudentDTO
                {
                    Id = s.Id,
                    StudentName = s.StudentName,
                    Email = s.Email,
                    Address = s.Address,
                    DOB = s.DOB
                })
                .FirstOrDefaultAsync();

            if (student == null)
            {
                _logger.LogError("Student not found with Id: {id}", id);
                return NotFound($"Không tìm thấy sinh viên có Id là {id}");
            }

            return Ok(student);
        }

        [HttpGet]
        [Route("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudentByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Dữ liệu không hợp lệ, vui lòng nhập lại");
            }

            name = name.ToLower();

            var students = await _context.Students
                .Where(s => s.StudentName.ToLower().Contains(name))
                .Select(s => new StudentDTO
                {
                    Id = s.Id,
                    StudentName = s.StudentName,
                    Email = s.Email,
                    Address = s.Address,
                    DOB = s.DOB
                })
                .ToListAsync();

            if (!students.Any())
            {
                return NotFound("Không tìm thấy dữ liệu");
            }

            return Ok(students);
        }

        [HttpDelete]
        [Route("{id:int}", Name = "DeleteById")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteStudentById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Dữ liệu không hợp lệ, vui lòng nhập lại");
            }

            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound($"Không tìm thấy sinh viên có Id là {id}");
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> CreateStudent([FromBody] StudentDTO model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.StudentName) || string.IsNullOrWhiteSpace(model.Email))
            {
                return BadRequest("Dữ liệu không hợp lệ, vui lòng nhập lại");
            }

            var student = new Student
            {
                StudentName = model.StudentName,
                Email = model.Email,
                Address = model.Address,
                DOB = model.DOB,
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            model.Id = student.Id;
            return CreatedAtRoute("GetStudentById", new { id = model.Id }, model);
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateStudent([FromBody] StudentDTO model)
        {
            if (model == null || model.Id <= 0)
            {
                return BadRequest("Dữ liệu không hợp lệ, vui lòng nhập lại");
            }

            var existingStudent = await _context.Students.FindAsync(model.Id);

            if (existingStudent == null)
            {
                return NotFound($"Không tìm thấy sinh viên có Id là {model.Id}");
            }

            existingStudent.StudentName = model.StudentName;
            existingStudent.Email = model.Email;
            existingStudent.Address = model.Address;
            existingStudent.DOB = model.DOB;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
