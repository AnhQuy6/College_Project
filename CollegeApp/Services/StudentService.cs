//using CollegeApp.Data.Repository;
//using CollegeApp.Data;
//using AutoMapper;
//using Microsoft.Identity.Client;
//using CollegeApp.Models;
//using Microsoft.IdentityModel.Tokens;

//namespace CollegeApp.Services
//{
//    public class StudentService : IStudentService
//    {
//        private readonly ICollegeRepository<Student> _studentRepository;
//        private readonly IMapper _mapper;
//        private readonly ILogger<StudentService> _logger;
//        public StudentService(ICollegeRepository<Student> studentRepository, IMapper mapper, ILogger<StudentService> logger)
//        {
//            _studentRepository = studentRepository;
//            _mapper = mapper;
//            _logger = logger;
//        }

//        public async Task<StudentDTO> GetStudentsAsync()
//        {
//            var student = _studentRepository.GetAllAsync();
//            return _mapper.Map<StudentDTO>(student);
//        }
//        public async Task<StudentDTO> GetStudentByIdAsync(int id)
//        {
//            if (id <= 0)
//            {
//                throw new ArgumentOutOfRangeException("Id phai le hon 0", nameof(id));
//            }
//            var student = await _studentRepository.GetAsync(s => s.Id == id);

//            if (student == null)
//            {
//                throw new KeyNotFoundException($"Khong tim thay nguoi dung co Id la {id}");
//            }

//            return _mapper.Map<StudentDTO>(student);
//        }

//        public async Task<List<StudentDTO>> GetStudentsByNameAsync(string name)
//        {
//            if (string.IsNullOrEmpty(name))
//            {
//                throw new ArgumentException("Ten sinh vien khong duoc de trong", nameof(name));
//            }
//            var student = await _studentRepository.GetByNameAsync(s => s.StudentName.ToLower().Contains(name.ToLower()) && !s.IsDeleted);

//            if (student == null)
//            {
//                throw new KeyNotFoundException("Khong tim thay du lieu");
//            }
//            return _mapper.Map<List<StudentDTO>>(student);
//        }

//        public async Task<bool> CreateStudentAsync(StudentDTO model)
//        {
//            if (model == null)
//            {
//                throw new ArgumentNullException($"Du lieu nhap khong duoc phep chua gia tri {null}", nameof(model));
//            }

//            if (string.IsNullOrEmpty(model.StudentName))
//            {
//                throw new ArgumentException("Ten sinh vien khong duoc de trong", nameof(model.StudentName));
//            }
//            if (string.IsNullOrEmpty(model.Email))
//            {
//                throw new ArgumentException("Email khong duoc de trong", nameof(model.Email));
//            }
//            if (string.IsNullOrEmpty(model.Address))
//            {
//                throw new ArgumentException("Dia chi khong duoc de trong", nameof(model.Address));
//            }

//            var existingUser = await _studentRepository.GetAsync(u => u.StudentName.Equals(model.StudentName));

//            if (existingUser != null)
//            {
//                throw new InvalidOperationException("Ten tai khoan da ton tai");
//            }
//            Student student = _mapper.Map<Student>(model);

//            await _studentRepository.CreateAsync(student);

//            return true;
//        }

//        public async Task<bool> UpdateUserAsync(UserDTO model)
//        {
//            if (model == null)
//            {
//                throw new ArgumentNullException($"Du lieu nhap khong duoc phep chua gia tri {null}", nameof(model));
//            }

//            if (string.IsNullOrEmpty(model.Username) || model.Username.Contains(" "))
//            {
//                throw new ArgumentException("Ten nguoi dung khong duoc de trong hoac chua khoang trang", nameof(model.Username));
//            }

//            if (string.IsNullOrEmpty(model.Password) || model.Password.Contains(" "))
//            {
//                throw new ArgumentException("Mat khau khong duoc de trong hoac chua khoang trang", nameof(model.Password));
//            }


//            var existingUser = await _userRepository.GetAsync(s => s.Id == model.Id, true);

//            if (existingUser == null)
//                throw new KeyNotFoundException($"Khong tim thay nguoi dung co id la {model.Id}");

//            var userToUpdate = _mapper.Map<User>(model);

//            userToUpdate.CreatedDate = DateTime.Now;
//            userToUpdate.ModifiedDate = DateTime.Now;

//            if (!string.IsNullOrEmpty(model.Password))
//            {
//                var passwordHash = CreatePasswordHashWithSalt(model.Password);
//                userToUpdate.Password = passwordHash.PasswordHash;
//                userToUpdate.PasswordSalt = passwordHash.Salt;
//            }

//            await _userRepository.UpdateAsync(userToUpdate);

//            return true;

//        }

//        public async Task<bool> DeleteUserAsync(int id)
//        {
//            if (id <= 0)
//                throw new ArgumentOutOfRangeException("ID phai lon hon khong", nameof(id));

//            User existingUser = await _userRepository.GetAsync(s => s.Id == id, true);

//            if (existingUser == null)
//                throw new KeyNotFoundException($"Không tìm thấy người dùng có id là {id}");

//            existingUser.IsDeleted = true;
//            existingUser.ModifiedDate = DateTime.Now;

//            await _userRepository.UpdateAsync(existingUser);
//            return true;
//        }
//    }
//}
