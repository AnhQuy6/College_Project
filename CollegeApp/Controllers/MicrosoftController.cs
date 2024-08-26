using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "LoginForMicrosoftUser")]
    public class MicrosoftController : ControllerBase
    {
        [HttpGet]
        [Route("Index")]
        public string Print()
        {
            return "Anh Quy";
        }
    }
}
