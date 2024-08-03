using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CollegeApp.Migrations
{
    /// <inheritdoc />
    public partial class CreateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(10)", nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "ntext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartmentId = table.Column<string>(type: "varchar(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.Id);
                    table.ForeignKey(
                        name: "fk_Student_Department",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "Id", "DepartmentName", "Description" },
                values: new object[,]
                {
                    { "CNTT001", "Công nghệ thông tin", "Sinh viên thuộc nghành công nghệ thông tin" },
                    { "KHMT001", "Khoa học máy tính", "Sinh viên thuộc nghành khoa học máy tính" },
                    { "Luat001", "Luật học", "Sinh viên thuộc luật học" }
                });

            migrationBuilder.InsertData(
                table: "Student",
                columns: new[] { "Id", "Address", "DOB", "DepartmentId", "Email", "StudentName" },
                values: new object[,]
                {
                    { 3, "Hà Nội", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "tramy@gmail.com", "Trà My" },
                    { 1, "Hà Tĩnh", new DateTime(2003, 2, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "KHMT001", "hoangquy3125@gmail.com", "Anh Quý" },
                    { 2, "Ninh Bình", new DateTime(2003, 6, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Luat001", "vanthao2306@gmail.com", "Vân Thảo" }
                });

            migrationBuilder.CreateIndex(
                name: "index-Department-Id",
                table: "Department",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "index-Student-Id",
                table: "Student",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Student_DepartmentId",
                table: "Student",
                column: "DepartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Department");
        }
    }
}
