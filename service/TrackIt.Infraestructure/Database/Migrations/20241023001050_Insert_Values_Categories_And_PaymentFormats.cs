using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace TrackIt.Infraestructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Insert_Values_Categories_And_PaymentFormats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "PaymentFormatId",
                table: "PaymentFormatConfigs",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "CategoryConfigs",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            var creditCardId = Guid.NewGuid();
            var debitCardId = Guid.NewGuid();
            var paymentSlipId = Guid.NewGuid();
            var moneyId = Guid.NewGuid();
            var pixId = Guid.NewGuid();
            var ticketId = Guid.NewGuid();
            
            migrationBuilder.InsertData(
                table: "PaymentFormats",
                columns: new[] { "Id", "Key", "Title" },
                values: new object[,]
                { 
                    { creditCardId, 0, "Cartão de Crédito" },
                    { debitCardId, 1, "Cartão de Débito" },
                    { paymentSlipId, 2, "Boleto" },
                    { moneyId, 3, "Dinheiro" },
                    { pixId, 4, "Pix" },
                    { ticketId, 5, "Ticket" },
                });

            var educationId = Guid.NewGuid();
            var foodId = Guid.NewGuid();
            var transportationId = Guid.NewGuid();
            var entertainmentId = Guid.NewGuid();
            var investmentId = Guid.NewGuid();
            var emergencyId = Guid.NewGuid();
            var othersId = Guid.NewGuid();
            
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Title", "Description" },
                values: new object[,]
                {
                    { educationId, "Educação", "Gasto referente à educação. Ex: Cursos, faculdade etc." },
                    { foodId, "Alimentação", "Gasto referente à alimentação. Ex: IFood, almoço, doces etc." },
                    { transportationId, "Transporte", "Gasto referente à transporte. Ex: Uber, transporte público etc." },
                    { entertainmentId, "Entretenimento", "Gasto referente à entretenimento. Ex: Cinema, jogos etc." },
                    { investmentId, "Investimento", "Gasto referente à investimento. Ex: Reserva de emergência, viagens etc." },
                    { emergencyId, "Emergência", "Gasto emergenciais. Ex: Saúde, acidentes etc." },
                    { othersId, "Outros", "Qualquer gasto não esperado." }
                });
            
            migrationBuilder.InsertData(
                table: "PaymentFormatConfigs",
                columns: new[] { "Id", "PaymentFormatId", "Icon", "IconColor", "BackgroundIconColor" },
                values: new object[,]
                {
                    { Guid.NewGuid(), creditCardId, "credit_card.svg", "#F4F4F4", "#9100D6" },
                    { Guid.NewGuid(), debitCardId, "debit_card.svg", "#F4F4F4", "#120DF1" },
                    { Guid.NewGuid(), paymentSlipId, "payment_slip.svg", "#F4F4F4", "#2E2E2E" },
                    { Guid.NewGuid(), moneyId, "money.svg", "#F4F4F4", "#37A413" },
                    { Guid.NewGuid(), pixId, "pix.svg", "", "" },
                    { Guid.NewGuid(), ticketId, "ticket.svg", "", "" }
                });
            
            migrationBuilder.InsertData(
                table: "CategoryConfigs",
                columns: new[] { "Id", "CategoryId", "Icon", "IconColor", "BackgroundIconColor" },
                values: new object[,]
                {
                    { Guid.NewGuid(), educationId, "education.svg", "#F4F4F4", "#120DF1" },
                    { Guid.NewGuid(), foodId, "food.svg", "#F4F4F4", "#6FD354" },
                    { Guid.NewGuid(), transportationId, "transportation.svg", "#F4F4F4", "#2E88FF" },
                    { Guid.NewGuid(), entertainmentId, "entertainment.svg", "#F4F4F4", "#FF8324" },
                    { Guid.NewGuid(), investmentId, "investment.svg", "#F4F4F4", "#31CC07" },
                    { Guid.NewGuid(), emergencyId, "emergency.svg", "#F4F4F4", "#E51C1C" },
                    { Guid.NewGuid(), othersId, "others.svg", "#F4F4F4", "#B408E9" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "PaymentFormatId",
                table: "PaymentFormatConfigs",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "CategoryConfigs",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");
            
            migrationBuilder.Sql("DELETE FROM PaymentFormatConfigs");
            migrationBuilder.Sql("DELETE FROM CategoryConfigs");
            
            migrationBuilder.Sql("DELETE FROM PaymentFormats");
            migrationBuilder.Sql("DELETE FROM Categories");
            
        }
    }
}
