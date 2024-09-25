using System.Net;

namespace CollegeApp.Models
{
    public class APIResponse
    {
        public bool Status { get; set; } = true;
        public dynamic Data { get; set; }
        public string Message { get; set; } = "Operation completed successfully";

    }
}
