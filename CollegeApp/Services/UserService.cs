using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace CollegeApp.Services
{
    public class UserService : IUserService
    {
        private readonly ICollegeRepository<User> _userRepository;
        private readonly IMapper _mapper;
        public UserService(ICollegeRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserReadonlyDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<List<UserReadonlyDTO>>(users);
        }

        public async Task<UserReadonlyDTO> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetAsync(s => s.Id == id);
            return _mapper.Map<UserReadonlyDTO>(user);
        }

        public async Task<UserReadonlyDTO> GetUserByNameAsync(string username)
        {
            var user = await _userRepository.GetAsync(s => s.Username.Equals(username));
            return _mapper.Map<UserReadonlyDTO>(user);
        }

        public async Task<bool> CreateUserAsync(UserDTO model)
        {
            //if (model == null)
            //    throw new ArgumentNullException(nameof(model));

            ArgumentNullException.ThrowIfNull(model, "Du lieu khong hop le, vui long nhap lai");

            var existingUser = await _userRepository.GetAsync(u => u.Username.Equals(model.Username));

            if (existingUser != null)
            {
                throw new Exception("Ten tai khoan da ton tai");
            }
            User user = _mapper.Map<User>(model);
            user.IsDeleted = false;
            user.CreatedDate = DateTime.Now;
            user.ModifiedDate = DateTime.Now;

            if (!string.IsNullOrEmpty(model.Password)) {
                var passwordHash = CreatePasswordHashWithSalt(model.Password);
                user.Password = passwordHash.PasswordHash;
                user.PasswordSalt = passwordHash.Salt;
            }
            
            await _userRepository.CreateAsync(user);

            return true;
        }

        public (string PasswordHash, string Salt) CreatePasswordHashWithSalt(string password)
        {
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
                ));

            return (hash, Convert.ToBase64String(salt));
        }
    }
}
