using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Indspire.Soaring.Engagement.Data.Migrations
{
    public partial class AwardRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
#if !DISABLE_UP_MIGRATIONS
            migrationBuilder.CreateIndex(
                name: "IX_AwardLog_AwardID",
                table: "AwardLog",
                column: "AwardID");

            migrationBuilder.AddForeignKey(
                name: "FK_AwardLog_Award_AwardID",
                table: "AwardLog",
                column: "AwardID",
                principalTable: "Award",
                principalColumn: "AwardID",
                onDelete: ReferentialAction.Cascade);
#endif
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AwardLog_Award_AwardID",
                table: "AwardLog");

            migrationBuilder.DropIndex(
                name: "IX_AwardLog_AwardID",
                table: "AwardLog");
        }
    }
}
