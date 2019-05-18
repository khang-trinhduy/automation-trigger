using Microsoft.EntityFrameworkCore.Migrations;

namespace Automation.API.Migrations
{
    public partial class Add_allany_conditions_to_trigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Table",
                table: "MetaData",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TriggerId1",
                table: "Condition",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Condition_TriggerId1",
                table: "Condition",
                column: "TriggerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Condition_Trigger_TriggerId1",
                table: "Condition",
                column: "TriggerId1",
                principalTable: "Trigger",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Condition_Trigger_TriggerId1",
                table: "Condition");

            migrationBuilder.DropIndex(
                name: "IX_Condition_TriggerId1",
                table: "Condition");

            migrationBuilder.DropColumn(
                name: "Table",
                table: "MetaData");

            migrationBuilder.DropColumn(
                name: "TriggerId1",
                table: "Condition");
        }
    }
}
