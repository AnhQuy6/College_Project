using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
