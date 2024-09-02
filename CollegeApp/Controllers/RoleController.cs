using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;
using System.Threading.Tasks.Dataflow;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICollegeRepository<Role> _roleRepository;
        private APIResponse _apiResponse;
        public RoleController(IMapper mapper, ICollegeRepository<Role> roleRepository)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
            _apiResponse = new APIResponse();
        }

        [HttpGet]
        [Route("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetRolesAsync()
        {
            try
            {
                var roles = await _roleRepository.GetAllAsync();
                _apiResponse.Data = _mapper.Map<List<RoleDTO>>(roles);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex) {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetRoleById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetRoleByIdAsync(int id) {
            try
            {
                if (id <= 0)
                    return BadRequest("Du lieu khong hop le, vui long nhap lai");
                var role = await _roleRepository.GetByIdAsync(s => s.Id == id);
                if (role == null)
                    return NotFound($"Khong tim thay sinh vien co id la {id}");
                _apiResponse.Data = _mapper.Map<RoleDTO>(role);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode= HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }   
        
        }

        [HttpGet]
        [Route("{name:alpha}", Name = "GetRoleByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetRoleByNameAsync (string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    return BadRequest("Du lieu khong hop le, vui long nhap lai");
                var role = await _roleRepository.GetByNameAsync(s => s.RoleName.ToUpper().Contains(name.ToUpper()));
                if (role == null)
                    return NotFound("Khong tim thay du lieu");
                _apiResponse.Data = _mapper.Map<List<RoleDTO>>(role);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateRoleAsync([FromBody]RoleDTO model) {
            try
            {
                if (model == null)
                    return BadRequest("Du lieu khong hop le, vui long nhap lai");

                Role role = _mapper.Map<Role>(model);
                role.IsDeleted = false;
                role.CreatedDate = DateTime.Now;
                role.ModifiedDate = DateTime.Now;

                await _roleRepository.CreateAsync(role);

                model.Id = role.Id;

                _apiResponse.Data = model;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                
                return CreatedAtRoute("GetRoleById", new { id = model.Id }, _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }   
        }

        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult<APIResponse>> UpdateRole([FromBody] RoleDTO model)
        {
            try
            {
                if (model == null || model.Id <= 0)
                    return BadRequest("Du lieu khong hop le, vui long nhap lai");
                var existingRole = await _roleRepository.GetByIdAsync(s => s.Id == model.Id, true);
                if (existingRole == null)
                    return NotFound("Khong tim thay du lieu");
                var result = _mapper.Map<Role>(model);
                result.IsDeleted = false;
                result.CreatedDate = DateTime.Now;
                result.ModifiedDate = DateTime.Now;
                await _roleRepository.UpdateAsync(result);
                _apiResponse.Data = result;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode= HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }

        [HttpDelete]
        [Route("Delete/{id:int}")]
        public async Task<ActionResult<APIResponse>> DeleteRoleAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Du lieu khong hop le, vui long nhap lai");
                var role = await _roleRepository.GetByIdAsync(s => s.Id == id);
                if (role == null) 
                    return NotFound();
                await _roleRepository.DeleteAsync(role);
                _apiResponse.Status = true;
                _apiResponse.Data = true;
                _apiResponse.StatusCode =HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.Data = false;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }
    }
}
