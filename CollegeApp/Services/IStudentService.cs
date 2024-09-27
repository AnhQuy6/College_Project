using CollegeApp.Models;

namespace CollegeApp.Services
{
    public interface IStudentService
    {
        Task<StudentDTO> GetStudentsAsync();
        Task<StudentDTO> GetStudentByIdAsync(int id);
        Task<List<StudentDTO>> GetStudentsByNameAsync(string name);
        Task<bool> CreateStudentAsync(StudentDTO model);
        Task<bool> UpdateStudentAsync(StudentDTO model);
        Task<bool> DeleteStudentAsync(int id);
    }
}
