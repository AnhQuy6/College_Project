using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp.Data.Config
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).HasDatabaseName("index-User-Id");

            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Username).IsRequired();
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.PasswordSalt).IsRequired();
            builder.Property(x => x.UserType).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();
            builder.Property(x => x.ModifiedDate).IsRequired();


        }
    }
}
