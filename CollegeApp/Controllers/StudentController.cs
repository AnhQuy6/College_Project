using AutoMapper;
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
        private readonly IMapper _mapper;

        public StudentController(CollegeDBContext context, ILogger<StudentController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudentsAsync()
        {
            _logger.LogInformation("Get Students method started");
            var students = await _context.Students.ToListAsync();
            var studentDTOData = _mapper.Map<List<StudentDTO>>(students);
            return Ok(studentDTOData);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> GetStudentByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Bad Request: {id}", id);
                return BadRequest("Dữ liệu không hợp lệ, vui lòng nhập lại");
            }

            var student = await _context.Students.Where(s => s.Id == id).FirstOrDefaultAsync();

            if (student == null)
            {
                _logger.LogError("Student not found with Id: {id}", id);
                return NotFound($"Không tìm thấy sinh viên có Id là {id}");
            }

            var studentDTO = _mapper.Map<StudentDTO>(student);

            return Ok(studentDTO);
        }

        [HttpGet]
        [Route("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudentByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Dữ liệu không hợp lệ, vui lòng nhập lại");
            }

            name = name.ToLower();

            var students = await _context.Students.Where(s => s.StudentName.ToLower().Contains(name)).ToListAsync();

            if (!students.Any())
            {
                return NotFound("Không tìm thấy dữ liệu");
            }

            var studentDTOData = _mapper.Map<List<StudentDTO>>(students);

            return Ok(studentDTOData);
        }

        [HttpDelete]
        [Route("{id:int}", Name = "DeleteById")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteStudentByIdAsync(int id)
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
        public async Task<ActionResult<StudentDTO>> CreateStudentAsync([FromBody] StudentDTO model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.StudentName) || string.IsNullOrWhiteSpace(model.Email))
            {
                return BadRequest("Dữ liệu không hợp lệ, vui lòng nhập lại");
            }

            var student = _mapper.Map<Student>(model);

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
        public async Task<ActionResult> UpdateStudentAsync([FromBody] StudentDTO model)
        {
            if (model == null || model.Id <= 0)
            {
                return BadRequest("Dữ liệu không hợp lệ, vui lòng nhập lại");
            }

            var existingStudent = await _context.Students.AsNoTracking().Where(s => s.Id == model.Id).FirstOrDefaultAsync();

            if (existingStudent == null)
            {
                return NotFound($"Không tìm thấy sinh viên có Id là {model.Id}");
            }

            var newRecord = _mapper.Map<Student>(model);
            _context.Students.Update(newRecord);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
