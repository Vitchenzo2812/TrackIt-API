using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackIt.Infraestructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Refactor_Activity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_ActivityGroup_ActivityGroupId",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_ActivityGroup_User_UserId",
                table: "ActivityGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_SubActivity_Activity_ActivityId",
                table: "SubActivity");

            migrationBuilder.DropTable(
                name: "Expense");

            migrationBuilder.DropTable(
                name: "MonthlyExpenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubActivity",
                table: "SubActivity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivityGroup",
                table: "ActivityGroup");

            migrationBuilder.DropIndex(
                name: "IX_ActivityGroup_UserId",
                table: "ActivityGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activity",
                table: "Activity");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "ActivityGroup");

            migrationBuilder.RenameTable(
                name: "SubActivity",
                newName: "SubActivities");

            migrationBuilder.RenameTable(
                name: "ActivityGroup",
                newName: "ActivityGroups");

            migrationBuilder.RenameTable(
                name: "Activity",
                newName: "Activities");

            migrationBuilder.RenameIndex(
                name: "IX_SubActivity_ActivityId",
                table: "SubActivities",
                newName: "IX_SubActivities_ActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_Activity_ActivityGroupId",
                table: "Activities",
                newName: "IX_Activities_ActivityGroupId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "SubActivities",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SubActivities",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "SubActivities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "Activities",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Activities",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Activities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubActivities",
                table: "SubActivities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivityGroups",
                table: "ActivityGroups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activities",
                table: "Activities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_ActivityGroups_ActivityGroupId",
                table: "Activities",
                column: "ActivityGroupId",
                principalTable: "ActivityGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubActivities_Activities_ActivityId",
                table: "SubActivities",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_ActivityGroups_ActivityGroupId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_SubActivities_Activities_ActivityId",
                table: "SubActivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubActivities",
                table: "SubActivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActivityGroups",
                table: "ActivityGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activities",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "SubActivities");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SubActivities");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "SubActivities");

            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Activities");

            migrationBuilder.RenameTable(
                name: "SubActivities",
                newName: "SubActivity");

            migrationBuilder.RenameTable(
                name: "ActivityGroups",
                newName: "ActivityGroup");

            migrationBuilder.RenameTable(
                name: "Activities",
                newName: "Activity");

            migrationBuilder.RenameIndex(
                name: "IX_SubActivities_ActivityId",
                table: "SubActivity",
                newName: "IX_SubActivity_ActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_ActivityGroupId",
                table: "Activity",
                newName: "IX_Activity_ActivityGroupId");

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "ActivityGroup",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubActivity",
                table: "SubActivity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActivityGroup",
                table: "ActivityGroup",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activity",
                table: "Activity",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MonthlyExpenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyExpenses", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Expense",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Amount = table.Column<double>(type: "double", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MonthlyExpensesId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PaymentFormat = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expense", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expense_MonthlyExpenses_MonthlyExpensesId",
                        column: x => x.MonthlyExpensesId,
                        principalTable: "MonthlyExpenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityGroup_UserId",
                table: "ActivityGroup",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_MonthlyExpensesId",
                table: "Expense",
                column: "MonthlyExpensesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_ActivityGroup_ActivityGroupId",
                table: "Activity",
                column: "ActivityGroupId",
                principalTable: "ActivityGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityGroup_User_UserId",
                table: "ActivityGroup",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubActivity_Activity_ActivityId",
                table: "SubActivity",
                column: "ActivityId",
                principalTable: "Activity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
