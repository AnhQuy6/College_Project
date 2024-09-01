using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp.Data.Config
{
    public class UserTypeConfig : IEntityTypeConfiguration<UserType>
    {
        public void Configure(EntityTypeBuilder<UserType> builder)
        {
            builder.ToTable("UserType");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(X => X.Name).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Description).HasMaxLength(1500);

            builder.HasData(new List<UserType>()
            {
                new UserType
                {
                    Id = 1,
                    Name = "Student",
                    Description = "For Students"
                },
                new UserType
                {
                    Id = 2,
                    Name = "Faculty",
                    Description = "For Faculty"
                },
                new UserType
                {
                    Id = 3,
                    Name = "Supporting Staff",
                    Description = "For Supporting Staff"
                },
                new UserType
                {
                    Id = 4,
                    Name = "Teacher",
                    Description = "For Teachers"
                }
            });

            
        }
    }
}
