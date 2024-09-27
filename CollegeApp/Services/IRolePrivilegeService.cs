using CollegeApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Services
{
    public interface IRolePrivilegeService
    {
        Task<bool> CreateRolePrivilegeAsync(RolePrivilegeDTO? model);
        Task<List<RolePrivilegeDTO>> GetRolePrivilegesAsync();
        Task<RolePrivilegeDTO> GetRolePrivilegeByIdAsync(int id);
        Task<RolePrivilegeDTO> GetRolePrivilegesByRoleIdAsync(int? roleId);
        Task<List<RolePrivilegeDTO>> GetRolePrivilegeByNameAsync(string username);
        Task<bool> UpdateRolePrivilegeAsync(RolePrivilegeDTO? model);
        Task<bool> DeleteRolePrivilegeAsync(int id);
    }
}
