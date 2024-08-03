using CollegeApp.Data;

namespace CollegeApp.Models
{
    public class DepartmentDTO
    {
        public string Id { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public ICollection<StudentDTO> StudentDTOs { get; set; }
    }
}
