using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Models;

namespace CollegeApp.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Student, StudentDTO>().ReverseMap(); //.AddTransform<string>(n => string.IsNullOrEmpty(n)? "khong tim thay du lieu" : n);
        }
    }
}
