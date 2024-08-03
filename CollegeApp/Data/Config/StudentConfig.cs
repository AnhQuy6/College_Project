using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp.Data.Config
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Student");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).HasDatabaseName("index-Student-Id");

            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.StudentName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Address).IsRequired().HasMaxLength(50);
            builder.Property(x => x.DOB).IsRequired();

            builder.HasOne(x => x.Department)
                .WithMany(x => x.Students)
                .HasForeignKey(x => x.DepartmentId)
                .HasConstraintName("fk_Student_Department");

            builder.HasData(new List<Student>()
            {
                new Student
                {
                    Id = 1,
                    StudentName = "Anh Quý",
                    Email = "hoangquy3125@gmail.com",
                    Address = "Hà Tĩnh",
                    DOB = new DateTime(2003,02,23),
                    DepartmentId = "KHMT001"
                },
                new Student
                {
                    Id = 2,
                    StudentName = "Vân Thảo",
                    Email = "vanthao2306@gmail.com",
                    Address = "Ninh Bình",
                    DOB = new DateTime(2003,06,23),
                    DepartmentId = "Luat001"
                },
                new Student
                {
                    Id = 3,
                    StudentName = "Trà My",
                    Email = "tramy@gmail.com",
                    Address = "Hà Nội",
                    DOB = new DateTime(2026,01,01),
                    
                }
            });
        }
    }
}
