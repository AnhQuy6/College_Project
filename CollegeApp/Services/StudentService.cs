using CollegeApp.Data.Repository;
using CollegeApp.Data;
using AutoMapper;
using Microsoft.Identity.Client;
using CollegeApp.Models;
using Microsoft.IdentityModel.Tokens;

namespace CollegeApp.Services
{
    public class StudentService : IStudentService
    {
        private readonly ICollegeRepository<Student> _studentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentService> _logger;
        public StudentService(ICollegeRepository<Student> studentRepository, IMapper mapper, ILogger<StudentService> logger)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<StudentDTO> GetStudentsAsync()
        {
            var student = _studentRepository.GetAllAsync();
            return _mapper.Map<StudentDTO>(student);
        }
        public async Task<StudentDTO> GetStudentByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id phai lon hon 0", nameof(id));
            }
            var student = await _studentRepository.GetAsync(s => s.Id == id);

            if (student == null)
            {
                throw new KeyNotFoundException($"Khong tim thay nguoi dung co Id la {id}");
            }

            return _mapper.Map<StudentDTO>(student);
        }

        public async Task<List<StudentDTO>> GetStudentsByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Ten sinh vien khong duoc de trong", nameof(name));
            }
            var student = await _studentRepository.GetByNameAsync(s => s.StudentName.ToLower().Contains(name.ToLower()));

            if (student == null)
            {
                throw new KeyNotFoundException("Khong tim thay du lieu");
            }
            return _mapper.Map<List<StudentDTO>>(student);
        }

        public async Task<bool> CreateStudentAsync(StudentDTO model)
        {
            if (string.IsNullOrEmpty(model.StudentName))
            {
                throw new ArgumentException("Ten sinh vien khong duoc de trong", nameof(model.StudentName));
            }
            if (string.IsNullOrEmpty(model.Email))
            {
                throw new ArgumentException("Email khong duoc de trong", nameof(model.Email));
            }
            if (string.IsNullOrEmpty(model.Address))
            {
                throw new ArgumentException("Dia chi khong duoc de trong", nameof(model.Address));
            }
           
            Student student = _mapper.Map<Student>(model);

            await _studentRepository.CreateAsync(student);

            return true;
        }

        public async Task<bool> UpdateStudentAsync(StudentDTO model)
        {
            if (string.IsNullOrEmpty(model.StudentName))
            {
                throw new ArgumentException("Ten sinh vien khong duoc de trong", nameof(model.StudentName));
            }

            if (string.IsNullOrEmpty(model.Email))
            {
                throw new ArgumentException("Email khong duoc de trong", nameof(model.Email));
            }


            var existingStudent = await _studentRepository.GetAsync(s => s.Id == model.Id, true);

            if (existingStudent == null)
                throw new KeyNotFoundException($"Khong tim thay sinh vien co id la {model.Id}");

            var studentToUpdate = _mapper.Map<Student>(model);

            studentToUpdate.StudentName = model.StudentName;
            studentToUpdate.Email = model.Email;
            studentToUpdate.Address = model.Address;
            studentToUpdate.DOB = model.DOB;

            await _studentRepository.UpdateAsync(studentToUpdate);

            return true;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("ID phai lon hon khong", nameof(id));

            var existingStudent = await _studentRepository.GetAsync(s => s.Id == id, true);

            if (existingStudent == null)
                throw new KeyNotFoundException($"Khong tim thay sinh vien co Id la {id}");

            await _studentRepository.UpdateAsync(existingStudent);
            return true;
        }
    }
}
