using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackIt.Infraestructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Relationship_Between_Entities_And_Config : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PaymentFormatId",
                table: "PaymentFormatConfigs",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "CategoryConfigs",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentFormatConfigs_PaymentFormatId",
                table: "PaymentFormatConfigs",
                column: "PaymentFormatId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryConfigs_CategoryId",
                table: "CategoryConfigs",
                column: "CategoryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryConfigs_Categories_CategoryId",
                table: "CategoryConfigs",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentFormatConfigs_PaymentFormats_PaymentFormatId",
                table: "PaymentFormatConfigs",
                column: "PaymentFormatId",
                principalTable: "PaymentFormats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryConfigs_Categories_CategoryId",
                table: "CategoryConfigs");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentFormatConfigs_PaymentFormats_PaymentFormatId",
                table: "PaymentFormatConfigs");

            migrationBuilder.DropIndex(
                name: "IX_PaymentFormatConfigs_PaymentFormatId",
                table: "PaymentFormatConfigs");

            migrationBuilder.DropIndex(
                name: "IX_CategoryConfigs_CategoryId",
                table: "CategoryConfigs");

            migrationBuilder.DropColumn(
                name: "PaymentFormatId",
                table: "PaymentFormatConfigs");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "CategoryConfigs");
        }
    }
}
