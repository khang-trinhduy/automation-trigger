using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Automation.API.Migrations
{
    public partial class addtrigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TriggerId",
                table: "Action",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Trigger",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsNotActive = table.Column<bool>(nullable: false),
                    Position = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trigger", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Condition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Field = table.Column<string>(nullable: true),
                    Operator = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    TriggerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Condition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Condition_Trigger_TriggerId",
                        column: x => x.TriggerId,
                        principalTable: "Trigger",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Action_TriggerId",
                table: "Action",
                column: "TriggerId");

            migrationBuilder.CreateIndex(
                name: "IX_Condition_TriggerId",
                table: "Condition",
                column: "TriggerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Action_Trigger_TriggerId",
                table: "Action",
                column: "TriggerId",
                principalTable: "Trigger",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Action_Trigger_TriggerId",
                table: "Action");

            migrationBuilder.DropTable(
                name: "Condition");

            migrationBuilder.DropTable(
                name: "Trigger");

            migrationBuilder.DropIndex(
                name: "IX_Action_TriggerId",
                table: "Action");

            migrationBuilder.DropColumn(
                name: "TriggerId",
                table: "Action");
        }
    }
}
