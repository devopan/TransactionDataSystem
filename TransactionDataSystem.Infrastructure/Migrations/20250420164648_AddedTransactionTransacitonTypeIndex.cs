using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransactionDataSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedTransactionTransacitonTypeIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransactionType",
                table: "Transactions",
                column: "TransactionType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_TransactionType",
                table: "Transactions");
        }
    }
}
