using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CollegeApp.Migrations
{
    /// <inheritdoc />
    public partial class AddingUserTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleMapping_Role",
                table: "UserRoleMapping");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleMapping_User",
                table: "UserRoleMapping");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoleMapping",
                table: "UserRoleMapping");

            migrationBuilder.DropIndex(
                name: "UK_UserRoleMapping",
                table: "UserRoleMapping");

            migrationBuilder.RenameTable(
                name: "UserRoleMapping",
                newName: "UserRoleMappings");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoleMapping_RoleId",
                table: "UserRoleMappings",
                newName: "IX_UserRoleMappings_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoleMappings",
                table: "UserRoleMappings",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Usertype",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usertype", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Usertype",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "For Students", "Student" },
                    { 2, "For Faculty", "Faculty" },
                    { 3, "For Supporting Staff", "Supporting Staff" },
                    { 4, "For Teachers", "Teacher" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_UserTypeId",
                table: "User",
                column: "UserTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleMappings_UserId",
                table: "UserRoleMappings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_UserType",
                table: "User",
                column: "UserTypeId",
                principalTable: "Usertype",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleMappings_Role_RoleId",
                table: "UserRoleMappings",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleMappings_User_UserId",
                table: "UserRoleMappings",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_UserType",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleMappings_Role_RoleId",
                table: "UserRoleMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleMappings_User_UserId",
                table: "UserRoleMappings");

            migrationBuilder.DropTable(
                name: "Usertype");

            migrationBuilder.DropIndex(
                name: "IX_User_UserTypeId",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoleMappings",
                table: "UserRoleMappings");

            migrationBuilder.DropIndex(
                name: "IX_UserRoleMappings_UserId",
                table: "UserRoleMappings");

            migrationBuilder.RenameTable(
                name: "UserRoleMappings",
                newName: "UserRoleMapping");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoleMappings_RoleId",
                table: "UserRoleMapping",
                newName: "IX_UserRoleMapping_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoleMapping",
                table: "UserRoleMapping",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "UK_UserRoleMapping",
                table: "UserRoleMapping",
                columns: new[] { "UserId", "RoleId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleMapping_Role",
                table: "UserRoleMapping",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleMapping_User",
                table: "UserRoleMapping",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
