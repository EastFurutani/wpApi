using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaStore.Migrations
{
    public partial class Seeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WordInfos",
                columns: new[] { "id", "wordName" },
                values: new object[] { 1, "AME" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WordInfos",
                keyColumn: "id",
                keyValue: 1);
        }
    }
}
