﻿using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Models;

namespace CollegeApp.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Student, StudentDTO>().ReverseMap(); //.AddTransform<string>(n => string.IsNullOrEmpty(n)? "khong tim thay du lieu" : n);
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<RolePrivilege, RolePrivilegeDTO>().ReverseMap();
            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<UserReadonlyDTO, User>().ReverseMap();
            CreateMap<RolePrivilegeDTO, RolePrivilege>().ReverseMap();
        }
    }
}
