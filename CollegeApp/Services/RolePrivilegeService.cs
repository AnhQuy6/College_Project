using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Models;

namespace CollegeApp.Services
{
    public class RolePrivilegeService : IRolePrivilegeService
    {
        private readonly IMapper _mapper;
        private readonly ICollegeRepository<RolePrivilege> _rolePrivilegeRepository;
        public RolePrivilegeService(IMapper mapper, ICollegeRepository<RolePrivilege> rolePrivilegeRepository)
        {
            _mapper = mapper;
            _rolePrivilegeRepository = rolePrivilegeRepository;
        }
        public async Task<bool> CreateRolePrivilegeAsync(RolePrivilegeDTO? model)
        {
            if (model == null)
            {
                throw new ArgumentNullException($"Du lieu nhap khong duoc phep chua gia tri {null}", nameof(model));
            }

            if (string.IsNullOrEmpty(model.RolePrivilegeName))
            {
                throw new ArgumentException("Ten dac quyen khong duoc de trong", nameof(model.RolePrivilegeName));
            }

           
            var rolePrivilege = _mapper.Map<RolePrivilege>(model);
            rolePrivilege.CreatedDate = DateTime.Now;
            rolePrivilege.ModifiedDate = DateTime.Now;


            await _rolePrivilegeRepository.CreateAsync(rolePrivilege);

            return true;
        }

        public async Task<bool> DeleteRolePrivilegeAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("ID phai lon hon khong", nameof(id));

            var existingRolePrivilege = await _rolePrivilegeRepository.GetAsync(s => s.Id == id, true);

            if (existingRolePrivilege == null)
                throw new KeyNotFoundException($"Không tìm thấy người dùng có id là {id}");

            existingRolePrivilege.IsDeleted = true;
            existingRolePrivilege.ModifiedDate = DateTime.Now;

            await _rolePrivilegeRepository.UpdateAsync(existingRolePrivilege);
            return true;
        }

        public async Task<RolePrivilegeDTO> GetRolePrivilegeByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id phai lon hon 0", nameof(id));
            }
            var rolePrivilege = await _rolePrivilegeRepository.GetAsync(s => s.Id == id);

            if (rolePrivilege == null)
            {
                throw new KeyNotFoundException($"Khong tim dac quyen dung co Id la {id}");
            }

            return _mapper.Map<RolePrivilegeDTO>(rolePrivilege);
        }

        public async Task<List<RolePrivilegeDTO>> GetRolePrivilegeByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Ten ndac quyen khong duoc de trong", nameof(name));
            }
            var rolePrivilege = await _rolePrivilegeRepository.GetByNameAsync(s => s.RolePrivilegeName.ToLower().Contains(name.ToLower()));

            if (rolePrivilege == null)
            {
                throw new KeyNotFoundException("Khong tim thay du lieu");
            }
            return _mapper.Map<List<RolePrivilegeDTO>>(rolePrivilege);
        }

        public async Task<List<RolePrivilegeDTO>> GetRolePrivilegesAsync()
        {
            var rolePrivileges = await _rolePrivilegeRepository.GetAllAsync();
            return _mapper.Map<List<RolePrivilegeDTO>>(rolePrivileges);
        }

        public async Task<RolePrivilegeDTO> GetRolePrivilegesByRoleIdAsync(int? roleId)
        {
            if (roleId is null or < 0)
            {
                throw new ArgumentException("Du lieu nhap khong hop le", nameof(roleId));
            }
            var rolePrivilege = await _rolePrivilegeRepository.GetAsync(s => s.RoleId == roleId);

            if (rolePrivilege is null)
            {
                throw new KeyNotFoundException("Khong tim thay thong tin");
            }

            return _mapper.Map<RolePrivilegeDTO>(rolePrivilege);

        }

        public async Task<bool> UpdateRolePrivilegeAsync(RolePrivilegeDTO? model)
        {
            if (model is null || model.Id < 0)
            {
                throw new ArgumentNullException($"Du lieu nhap khong hop le {null}", nameof(model));
            }

            if (model.RoleId < 0)
            {
                throw new ArgumentException($"Du lieu nhap khong hop le {null}", nameof(model));
            }

            if (string.IsNullOrEmpty(model.RolePrivilegeName));
            {
                throw new ArgumentException("Ten dac quyen khong duoc de trong", nameof(model.RolePrivilegeName));
            }



            var existingUser = await _rolePrivilegeRepository.GetAsync(s => s.Id == model.Id, true);

            if (existingUser == null)
                throw new KeyNotFoundException($"Khong tim thay nguoi dung co id la {model.Id}");

            var userToUpdate = _mapper.Map<RolePrivilege>(model);

            userToUpdate.CreatedDate = DateTime.Now;
            userToUpdate.ModifiedDate = DateTime.Now;

            return true;

        }
    }
}
