using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cenconApi.Migrations
{
    /// <inheritdoc />
    public partial class initialdatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReqOpenClose",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NOWO = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    WSID = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    JReqOpen = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    JReqClose = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    KDClose = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tanggal = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Jam = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdat = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updatedat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReqOpenClose", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReqOpenClose_NOWO",
                table: "ReqOpenClose",
                column: "NOWO");

            migrationBuilder.CreateIndex(
                name: "IX_ReqOpenClose_Tanggal",
                table: "ReqOpenClose",
                column: "Tanggal");

            migrationBuilder.CreateIndex(
                name: "IX_ReqOpenClose_WSID",
                table: "ReqOpenClose",
                column: "WSID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReqOpenClose");
        }
    }
}
