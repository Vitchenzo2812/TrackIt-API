using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackIt.Infraestructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Ticket_Code_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_TicketCode_CodeId",
                table: "Ticket");

            migrationBuilder.DropTable(
                name: "TicketCode");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_CodeId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "CodeId",
                table: "Ticket");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Ticket",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Ticket");

            migrationBuilder.AddColumn<Guid>(
                name: "CodeId",
                table: "Ticket",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "TicketCode",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Value = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketCode", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_CodeId",
                table: "Ticket",
                column: "CodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_TicketCode_CodeId",
                table: "Ticket",
                column: "CodeId",
                principalTable: "TicketCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
