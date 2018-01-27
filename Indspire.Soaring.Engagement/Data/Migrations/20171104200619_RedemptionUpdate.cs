using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Indspire.Soaring.Engagement.Data.Migrations
{
    public partial class RedemptionUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
#if !DISABLE_UP_MIGRATIONS
            migrationBuilder.CreateTable(
                name: "RedemptionLog",
                columns: table => new
                {
                    RedemptionLogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RedemptionID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedemptionLog", x => x.RedemptionLogID);
                    table.ForeignKey(
                        name: "FK_RedemptionLog_Redemption_RedemptionID",
                        column: x => x.RedemptionID,
                        principalTable: "Redemption",
                        principalColumn: "RedemptionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RedemptionLog_RedemptionID",
                table: "RedemptionLog",
                column: "RedemptionID");
#endif
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RedemptionLog");
        }
    }
}
