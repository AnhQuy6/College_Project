using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp.Data.Config
{
    public class RolePrivilegeConfig : IEntityTypeConfiguration<RolePrivilege>
    {
        public void Configure(EntityTypeBuilder<RolePrivilege> builder)
        {
            builder.ToTable("RolePrivilege");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).HasDatabaseName("index-RolePrivilege-Id");

            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.RolePrivilegeName).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.RoleId).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();

            builder.HasOne(x => x.Role)
                .WithMany(x => x.RolePrivileges)
                .HasForeignKey(x => x.RoleId)
                .HasConstraintName("FK_RolePrivilege_Role");
        }
    }
}
