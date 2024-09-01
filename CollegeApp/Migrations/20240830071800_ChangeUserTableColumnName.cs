﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeApp.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserTableColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserType",
                table: "User",
                newName: "UserTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserTypeId",
                table: "User",
                newName: "UserType");
        }
    }
}
