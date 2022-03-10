using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketShop.Repository.Migrations
{
    public partial class addedemailmessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "34db5492-0cbb-4a0a-b516-083083e2da4c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "464492a3-2396-4ffd-b8c3-97f419dccb86");

            migrationBuilder.CreateTable(
                name: "EmailMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MailTo = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailMessages", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e2c37e7c-e26b-40fb-8111-67d0c900bf8c", "e9a1d690-d7d9-483d-8411-fc6973c16855", "STANDARD_USER", "STANDARD_USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "31b0dfdf-bf69-491f-8d04-b0e08f3b334e", "0d8b9841-a514-4052-a80a-5d1f7714cdbe", "ADMIN", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailMessages");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31b0dfdf-bf69-491f-8d04-b0e08f3b334e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e2c37e7c-e26b-40fb-8111-67d0c900bf8c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "34db5492-0cbb-4a0a-b516-083083e2da4c", "b20d51da-a473-4716-b9a8-b9db22bb165d", "STANDARD_USER", "STANDARD_USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "464492a3-2396-4ffd-b8c3-97f419dccb86", "e304b85a-d617-4cc8-80f9-941df01c2a07", "ADMIN", "ADMIN" });
        }
    }
}
