using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Indspire.Soaring.Engagement.Data.Migrations
{
    public partial class AddedUserRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
#if !DISABLE_UP_MIGRATIONS
            migrationBuilder.CreateIndex(
                name: "IX_RedemptionLog_UserID",
                table: "RedemptionLog",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_AwardLog_UserID",
                table: "AwardLog",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_AwardLog_User_UserID",
                table: "AwardLog",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RedemptionLog_User_UserID",
                table: "RedemptionLog",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
#endif
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AwardLog_User_UserID",
                table: "AwardLog");

            migrationBuilder.DropForeignKey(
                name: "FK_RedemptionLog_User_UserID",
                table: "RedemptionLog");

            migrationBuilder.DropIndex(
                name: "IX_RedemptionLog_UserID",
                table: "RedemptionLog");

            migrationBuilder.DropIndex(
                name: "IX_AwardLog_UserID",
                table: "AwardLog");
        }
    }
}
