using AutoMapper;
using CollegeApp.Data.Repository;
using CollegeApp.Data;
using CollegeApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePrivilegeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICollegeRepository<RolePrivilege> _rolePrivilegeRepository;
        private APIResponse _apiResponse;
        public RolePrivilegeController(IMapper mapper, ICollegeRepository<RolePrivilege> rolePrivilegeRepository)
        {
            _mapper = mapper;
            _rolePrivilegeRepository = rolePrivilegeRepository;
            _apiResponse = new APIResponse();
        }
        [HttpGet]
        [Route("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetRolePrivilegesAsync()
        {
                var rolePrivileges = await _rolePrivilegeRepository.GetAllAsync();
                _apiResponse.Data = _mapper.Map<List<RolePrivilegeDTO>>(rolePrivileges);
                return Ok(_apiResponse);
        }

        [HttpGet]
        [Route("All/{roleId:int}", Name = "GetRolePrivilegesByRoleId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetRolePrivilegesByRoleIdAsync(int roleId)
        {
                var rolePrivileges = await _rolePrivilegeRepository.GetAllByFilterAsync(s => s.RoleId == roleId);
                _apiResponse.Data = _mapper.Map<List<RolePrivilegeDTO>>(rolePrivileges);
                return Ok(_apiResponse);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetRolePrivilegeById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetRolePrivilegeByIdAsync(int id)
        {
                if (id <= 0)
                    return BadRequest("Du lieu khong hop le, vui long nhap lai");
                var rolePrivilege = await _rolePrivilegeRepository.GetAsync(s => s.Id == id);
                if (rolePrivilege == null)
                    return NotFound($"Khong tim thay dac quyen co id la {id}");
                _apiResponse.Data = _mapper.Map<RolePrivilegeDTO>(rolePrivilege);

                return Ok(_apiResponse);
        }

        [HttpGet]
        [Route("{name:alpha}", Name = "GetRolePrivilegeByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetRolePrivilegeByNameAsync(string name)
        {

                var rolePrivilege = await _rolePrivilegeRepository.GetByNameAsync(s => s.RolePrivilegeName.ToUpper().Contains(name.ToUpper()));
                _apiResponse.Data = _mapper.Map<List<RolePrivilegeDTO>>(rolePrivilege);
                return Ok(_apiResponse);
        }
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<APIResponse>> CreateRolePrivilegeAsync(RolePrivilegeDTO model)
        {
                if (model == null || model.Id < 0)
                    return BadRequest("Du lieu khong hop le, vui long nhap lai");
                var newRolePrivilege = _mapper.Map<RolePrivilege>(model);
                newRolePrivilege.IsDeleted = false;
                newRolePrivilege.CreatedDate = DateTime.Now;
                newRolePrivilege.ModifiedDate = DateTime.Now;
                await _rolePrivilegeRepository.CreateAsync(newRolePrivilege);
                model.Id = newRolePrivilege.Id;
                _apiResponse.Data = model;
                return CreatedAtRoute("GetRolePrivilegeById", new { id = model.Id }, _apiResponse);
            }
        }
        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult<APIResponse>> UpdateRolePrivilege([FromBody] RolePrivilegeDTO model)
        {
            try
            {
                if (model == null || model.Id <= 0)
                    return BadRequest("Du lieu khong hop le, vui long nhap lai");
                var existingRolePrivilege = await _rolePrivilegeRepository.GetAsync(s => s.Id == model.Id, true);
                if (existingRolePrivilege == null)
                    return NotFound("Khong tim thay du lieu");
                var result = _mapper.Map<RolePrivilege>(model);
                result.IsDeleted = false;
                result.CreatedDate = DateTime.Now;
                result.ModifiedDate = DateTime.Now;
                await _rolePrivilegeRepository.UpdateAsync(result);
                _apiResponse.Data = result;
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

        [HttpDelete]
        [Route("Delete/{id:int}", Name = "DeleteRolePrivilege")]
        public async Task<ActionResult<APIResponse>> DeleteRolePrivilegeAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Du lieu khong hop le, vui long nhap lai");
                var role = await _rolePrivilegeRepository.GetAsync(s => s.Id == id);
                if (role == null)
                    return NotFound();
                await _rolePrivilegeRepository.DeleteAsync(role);
                _apiResponse.Status = true;
                _apiResponse.Data = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
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
