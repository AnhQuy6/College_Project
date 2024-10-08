﻿using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Expressions;
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

        public async Task<List<UserReadonlyDTO>> GetUsersAsync()
        {
            var users = await _userRepository.GetAllByFilterAsync(s => !s.IsDeleted);
            return _mapper.Map<List<UserReadonlyDTO>>(users);
        }

        public async Task<UserReadonlyDTO> GetUserByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException("Id phai le hon 0", nameof(id));
            }
            var user = await _userRepository.GetAsync(s => s.Id == id && !s.IsDeleted);

            if (user == null)
            {
                throw new KeyNotFoundException($"Khong tim thay nguoi dung co Id la {id}");
            }

            return _mapper.Map<UserReadonlyDTO>(user);
        }

        public async Task<List<UserReadonlyDTO>> GetUserByNameAsync(string username)
        {
            if (string.IsNullOrEmpty(username) || username.Contains(" "))
            {
                throw new ArgumentException("Ten nguoi dung khong duoc de trong hoac chua ki tu khoang trang",nameof(username));
            }
            var user = await _userRepository.GetByNameAsync(s => s.Username.ToLower().Contains(username.ToLower()) && !s.IsDeleted);

            if (user == null)
            {
                throw new KeyNotFoundException("Khong tim thay du lieu");
            }
            return _mapper.Map<List<UserReadonlyDTO>>(user);
        }

        public async Task<bool> CreateUserAsync(UserDTO model)
        {
            if (model == null)
            {
                throw new ArgumentNullException($"Du lieu nhap khong duoc phep chua gia tri {null}" ,nameof(model));
            }

            if(string.IsNullOrEmpty(model.Username) || model.Username.Contains(" "))
            {
                throw new ArgumentException("Ten nguoi dung khong duoc de trong hoac chua ki tu khoang trang", nameof(model.Username));
            }

            if (string.IsNullOrEmpty(model.Password) || model.Password.Contains(" "))
            {
                throw new ArgumentException("Mat khau khong duoc de trong hoac chua ki tu khoang trang", nameof(model.Password));
            }

            var existingUser = await _userRepository.GetAsync(u => u.Username.Equals(model.Username));

            if (existingUser != null)
            {
                throw new InvalidOperationException("Ten tai khoan da ton tai");
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
                                                                    
        public async Task<bool> UpdateUserAsync(UserDTO model)
        {
            if (model == null)
            {
                throw new ArgumentNullException($"Du lieu nhap khong duoc phep chua gia tri {null}", nameof(model));
            }

            if (string.IsNullOrEmpty(model.Username) || model.Username.Contains(" "))
            {
                throw new ArgumentException("Ten nguoi dung khong duoc de trong hoac chua khoang trang", nameof(model.Username));
            }

            if (string.IsNullOrEmpty(model.Password) || model.Password.Contains(" "))
            {
                throw new ArgumentException("Mat khau khong duoc de trong hoac chua khoang trang", nameof(model.Password));
            }


            var existingUser = await _userRepository.GetAsync(s => s.Id == model.Id, true);

            if (existingUser == null)
                throw new KeyNotFoundException($"Khong tim thay nguoi dung co id la {model.Id}");

            var userToUpdate = _mapper.Map<User>(model);

            userToUpdate.CreatedDate = DateTime.Now;
            userToUpdate.ModifiedDate = DateTime.Now;
            
            if (!string.IsNullOrEmpty(model.Password))
            {
                var passwordHash = CreatePasswordHashWithSalt(model.Password);
                userToUpdate.Password = passwordHash.PasswordHash;
                userToUpdate.PasswordSalt = passwordHash.Salt;
            }

            await _userRepository.UpdateAsync(userToUpdate);

            return true;

        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("ID phai lon hon khong", nameof(id));

            User existingUser = await _userRepository.GetAsync(s => s.Id == id, true);

            if (existingUser == null)
                throw new KeyNotFoundException($"Không tìm thấy người dùng có id là {id}");

            existingUser.IsDeleted = true;
            existingUser.ModifiedDate = DateTime.Now;

            await _userRepository.UpdateAsync(existingUser);
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
