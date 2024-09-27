using AutoMapper;
using CollegeApp.Data.Repository;
using CollegeApp.Data;
using CollegeApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using CollegeApp.Services;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePrivilegeController : ControllerBase
    {
        private readonly IRolePrivilegeService _rolePrivilegeService;
        private APIResponse _apiResponse;
        public RolePrivilegeController(IRolePrivilegeService rolePrivilegeService)
        {
            _rolePrivilegeService = rolePrivilegeService;
            _apiResponse = new APIResponse();
        }
        [HttpGet]
        [Route("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetRolePrivilegesAsync()
        {
            var result = await _rolePrivilegeService.GetRolePrivilegesAsync();
            _apiResponse.Data = result;
            return Ok(_apiResponse);
        }

        [HttpGet]
        [Route("All/{roleId:int}", Name = "GetRolePrivilegesByRoleId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetRolePrivilegesByRoleIdAsync(int roleId)
        {
            var result = await _rolePrivilegeService.GetRolePrivilegesByRoleIdAsync(roleId);
            _apiResponse.Data = result;
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
            var result = await _rolePrivilegeService.GetRolePrivilegeByIdAsync(id);
            _apiResponse.Data = result;
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

            var result = await _rolePrivilegeService.GetRolePrivilegeByNameAsync(name);
            _apiResponse.Data = result;
            return Ok(_apiResponse);
        }
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<APIResponse>> CreateRolePrivilegeAsync(RolePrivilegeDTO model)
        {
            var result = await _rolePrivilegeService.CreateRolePrivilegeAsync(model);
            _apiResponse.Data = result;
            return Ok(_apiResponse);
        }
        
        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult<APIResponse>> UpdateRolePrivilege([FromBody] RolePrivilegeDTO model)
        {
            var result = await _rolePrivilegeService.UpdateRolePrivilegeAsync(model);
            _apiResponse.Data = result;
            return Ok(_apiResponse);
            
        }

        [HttpDelete]
        [Route("Delete/{id:int}", Name = "DeleteRolePrivilege")]
        public async Task<ActionResult<APIResponse>> DeleteRolePrivilegeAsync(int id)
        {
            var result = await _rolePrivilegeService.DeleteRolePrivilegeAsync(id);
            _apiResponse.Data = result;
            return Ok(_apiResponse);
        }

    }
}
