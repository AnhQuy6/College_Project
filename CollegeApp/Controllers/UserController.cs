using CollegeApp.Models;
using CollegeApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private APIResponse _apiResponse;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _apiResponse = new APIResponse();
            _logger = logger;
        }

        [HttpGet]
        [Route("All", Name = "GetAllUsers")]
        public async Task<ActionResult<APIResponse>> GetUsersAsync()
        {
            var users = await _userService.GetUsersAsync();
            _apiResponse.Data = users;
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetUserById")]
        public async Task<ActionResult<APIResponse>> GetUserByIdAsync(int id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            _apiResponse.Data = result;
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        [HttpGet]
        [Route("{username:alpha}", Name = "GetUserByName")]
        public async Task<ActionResult<APIResponse>> GetUserByNameAsync(string username)
        {
            var result = await _userService.GetUserByNameAsync(username);
            _apiResponse.Data = result;
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateUserAsync(UserDTO model)
        {
            var userCreated = await _userService.CreateUserAsync(model);
            _apiResponse.Data = userCreated;
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> UpdateUserAsync(UserDTO model)
        {
            var result = await _userService.UpdateUserAsync(model);
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.Data = result;
            return Ok(_apiResponse);
        }

        [HttpDelete]
        [Route("{id:int}", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> DeleteUserAsync(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            _apiResponse.Data = result;
            return Ok(_apiResponse);
        }
    }
}
