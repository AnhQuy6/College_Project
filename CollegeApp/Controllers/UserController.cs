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
        public async Task<ActionResult<APIResponse>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                _apiResponse.Data = users;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            } catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
            
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetUserById")]
        public async Task<ActionResult<APIResponse>> GetUserByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest($"id = ${id} khong hop le, vui long nhap id co gia tri > 0");
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound($"Khong tim thay nguoi dung co id la {id}");
                _apiResponse.Data = user;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);

            } catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }

        [HttpGet]
        [Route("{username:alpha}", Name = "GetUserByName")]
        public async Task<ActionResult<APIResponse>> GetUserByNameAsync(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                    return BadRequest("Du lieu khong hop le, vui long nhap lai");
                var user = await _userService.GetUserByNameAsync(username);
                if (user == null)
                    return NotFound($"Khong tim thay nguoi dung co ten la {username}");
                _apiResponse.Data = user;
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateUserAsync(UserDTO model)
        {
            try
            {
                var userCreated = await _userService.CreateUserAsync(model);

                _apiResponse.Data = userCreated;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            } catch (Exception ex)
            {
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }
    }
}
