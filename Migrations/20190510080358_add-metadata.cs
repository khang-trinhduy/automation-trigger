using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Automation.API.Migrations
{
    public partial class addmetadata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Field",
                table: "Condition");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Condition");

            migrationBuilder.DropColumn(
                name: "Field",
                table: "Action");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Action",
                newName: "Type");

            migrationBuilder.AddColumn<string>(
                name: "Table",
                table: "Trigger",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MetaDataId",
                table: "Condition",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MetaDataId",
                table: "Action",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MetaData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Field = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Condition_MetaDataId",
                table: "Condition",
                column: "MetaDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Action_MetaDataId",
                table: "Action",
                column: "MetaDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Action_MetaData_MetaDataId",
                table: "Action",
                column: "MetaDataId",
                principalTable: "MetaData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Condition_MetaData_MetaDataId",
                table: "Condition",
                column: "MetaDataId",
                principalTable: "MetaData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Action_MetaData_MetaDataId",
                table: "Action");

            migrationBuilder.DropForeignKey(
                name: "FK_Condition_MetaData_MetaDataId",
                table: "Condition");

            migrationBuilder.DropTable(
                name: "MetaData");

            migrationBuilder.DropIndex(
                name: "IX_Condition_MetaDataId",
                table: "Condition");

            migrationBuilder.DropIndex(
                name: "IX_Action_MetaDataId",
                table: "Action");

            migrationBuilder.DropColumn(
                name: "Table",
                table: "Trigger");

            migrationBuilder.DropColumn(
                name: "MetaDataId",
                table: "Condition");

            migrationBuilder.DropColumn(
                name: "MetaDataId",
                table: "Action");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Action",
                newName: "Value");

            migrationBuilder.AddColumn<string>(
                name: "Field",
                table: "Condition",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Condition",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Field",
                table: "Action",
                nullable: true);
        }
    }
}
