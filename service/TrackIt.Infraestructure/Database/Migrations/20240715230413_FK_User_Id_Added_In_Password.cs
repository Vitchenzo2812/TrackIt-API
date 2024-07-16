using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackIt.Infraestructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class FK_User_Id_Added_In_Password : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Password_PasswordId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_PasswordId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PasswordId",
                table: "User");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Password",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Password_UserId",
                table: "Password",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Password_User_UserId",
                table: "Password",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Password_User_UserId",
                table: "Password");

            migrationBuilder.DropIndex(
                name: "IX_Password_UserId",
                table: "Password");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Password");

            migrationBuilder.AddColumn<Guid>(
                name: "PasswordId",
                table: "User",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_User_PasswordId",
                table: "User",
                column: "PasswordId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Password_PasswordId",
                table: "User",
                column: "PasswordId",
                principalTable: "Password",
                principalColumn: "Id");
        }
    }
}
