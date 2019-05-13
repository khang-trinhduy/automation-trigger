using Microsoft.EntityFrameworkCore.Migrations;

namespace Automation.API.Migrations
{
    public partial class changemetadata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "MetaData");

            migrationBuilder.AddColumn<string>(
                name: "Threshold",
                table: "Condition",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Condition",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Action",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Threshold",
                table: "Condition");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Condition");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Action");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "MetaData",
                nullable: true);
        }
    }
}
