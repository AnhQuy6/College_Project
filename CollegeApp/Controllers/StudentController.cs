using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Models;
using Microsoft.AspNetCore.Cors;
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
    //[EnableCors(PolicyName = "AllowOnlyLocalhost")]
    public class StudentController : ControllerBase
    {
        
        private readonly ILogger<StudentController> _logger;
        private readonly IMapper _mapper;
        private readonly ICollegeRepository<Student> _studentRepository;
        //private readonly ICollegeRepository<Student> _studentRepository;

        public StudentController(ILogger<StudentController> logger, IMapper mapper, ICollegeRepository<Student> studentRepository)
        {
            _studentRepository = studentRepository;
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
            var students = await _studentRepository.GetAllAsync();
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

            var student = await _studentRepository.GetByIdAsync(student => student.Id == id, false);

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

            var students = await _studentRepository.GetByNameAsync(student => student.StudentName.ToLower().Contains(name.ToLower()));

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
        public async Task<ActionResult<bool>> DeleteStudentByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Dữ liệu không hợp lệ, vui lòng nhập lại");
            }

            var student = await _studentRepository.GetByIdAsync(student => student.Id == id, false);

            if (student == null)
            {
                return NotFound($"Không tìm thấy sinh viên có Id là {id}");
            }

            await _studentRepository.DeleteAsync(student);
            

            return Ok(true);
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

            var studentAfterCreation =  await _studentRepository.CreateAsync(student);

            model.Id = studentAfterCreation.Id;
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

            var existingStudent = await _studentRepository.GetByIdAsync(student => student.Id == model.Id, true);

            if (existingStudent == null)
            {
                return NotFound($"Không tìm thấy sinh viên có Id là {model.Id}");
            }

            var newRecord = _mapper.Map<Student>(model);
            
            await _studentRepository.UpdateAsync(newRecord);

            return NoContent();
        }
    }
}
