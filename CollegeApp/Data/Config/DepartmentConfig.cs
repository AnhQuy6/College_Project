using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp.Data.Config
{
    public class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Department");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).HasDatabaseName("index-Department-Id");

            builder.Property(x => x.Id).HasColumnType("varchar(10)");
            builder.Property(X => X.DepartmentName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Description).HasColumnType("ntext");

            builder.HasData(new List<Department>()
            {
                new Department
                {
                    Id = "KHMT001",
                    DepartmentName = "Khoa học máy tính",
                    Description = "Sinh viên thuộc nghành khoa học máy tính",
                },
                new Department
                {
                    Id = "Luat001",
                    DepartmentName = "Luật học",
                    Description = "Sinh viên thuộc luật học",
                },
                new Department
                {
                    Id = "CNTT001",
                    DepartmentName = "Công nghệ thông tin",
                    Description = "Sinh viên thuộc nghành công nghệ thông tin",
                }
            });
        }
    }
}
