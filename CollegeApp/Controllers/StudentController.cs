using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "LoginForLocalUser", Roles = "Supperadmin, Admin")]
    //[EnableCors(PolicyName = "AllowOnlyLocalhost")]
    public class StudentController : ControllerBase
    {

        private readonly ILogger<StudentController> _logger;
        private readonly IMapper _mapper;
        private readonly ICollegeRepository<Student> _studentRepository;
        private APIResponse _apiResponse;
        //private readonly ICollegeRepository<Student> _studentRepository;

        public StudentController(ILogger<StudentController> logger, IMapper mapper, ICollegeRepository<Student> studentRepository)
        {
            _studentRepository = studentRepository;
            _logger = logger;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }

        [HttpGet]
        [Route("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]


        public async Task<ActionResult<APIResponse>> GetStudentsAsync()
        {
            try
            {
                _logger.LogInformation("Get All Students method started");
                var students = await _studentRepository.GetAllAsync();
                _apiResponse.Data = _mapper.Map<List<StudentDTO>>(students);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }

        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetStudentByIdAsync(int id)
        {

            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Bad Request: {id}", id);
                    return BadRequest("Dữ liệu không hợp lệ, vui lòng nhập lại");
                }

                var student = await _studentRepository.GetAsync(student => student.Id == id, false);

                if (student == null)
                {
                    _logger.LogError("Student not found with Id: {id}", id);
                    return NotFound($"Không tìm thấy sinh viên có Id là {id}");
                }

                _apiResponse.Data = _mapper.Map<StudentDTO>(student);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;

            }


        }

        [HttpGet]
        [Route("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetStudentByNameAsync(string name)
        {
            try
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

                _apiResponse.Data = _mapper.Map<List<StudentDTO>>(students);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }


        }

        [HttpDelete]
        [Route("{id:int}", Name = "DeleteById")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> DeleteStudentByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Dữ liệu không hợp lệ, vui lòng nhập lại");
                }

                var student = await _studentRepository.GetAsync(student => student.Id == id, false);

                if (student == null)
                {
                    return NotFound($"Không tìm thấy sinh viên có Id là {id}");
                }

                await _studentRepository.DeleteAsync(student);

                _apiResponse.Data = true;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;


                return Ok(true);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }


        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateStudentAsync([FromBody] StudentDTO model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.StudentName) || string.IsNullOrWhiteSpace(model.Email))
                {
                    return BadRequest("Dữ liệu không hợp lệ, vui lòng nhập lại");
                }

                var student = _mapper.Map<Student>(model);

                var studentAfterCreation = await _studentRepository.CreateAsync(student);

                model.Id = studentAfterCreation.Id;

                _apiResponse.Data = model;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.Created;


                return CreatedAtRoute("GetStudentById", new { id = model.Id }, _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }


        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateStudentAsync([FromBody] StudentDTO model)
        {
            if (model == null || model.Id <= 0)
            {
                return BadRequest("Dữ liệu không hợp lệ, vui lòng nhập lại");
            }

            var existingStudent = await _studentRepository.GetAsync(student => student.Id == model.Id, true);

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
