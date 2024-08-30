using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp.Data.Config
{
    public class UserRoleMappingConfig : IEntityTypeConfiguration<UserRoleMapping>
    {
        public void Configure(EntityTypeBuilder<UserRoleMapping> builder)
        {
            builder.ToTable("UserRoleMapping");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasIndex(n => new {n.UserId, n.RoleId}, "UK_UserRoleMapping").IsUnique();

            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.RoleId).IsRequired();

            builder.HasOne(x => x.Role)
                .WithMany(x => x.UserRoleMappings)
                .HasForeignKey(x => x.RoleId)
                .HasConstraintName("FK_UserRoleMapping_Role");

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserRoleMappings)
                .HasForeignKey(x => x.UserId)
                .HasConstraintName("FK_UserRoleMapping_User");
        }
    }
}
