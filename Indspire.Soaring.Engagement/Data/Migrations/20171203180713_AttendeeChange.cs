using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Indspire.Soaring.Engagement.Data.Migrations
{
    public partial class AttendeeChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AwardLog_User_UserID",
                table: "AwardLog");

            migrationBuilder.DropForeignKey(
                name: "FK_RedemptionLog_User_UserID",
                table: "RedemptionLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Attendee");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attendee",
                table: "Attendee",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_AwardLog_Attendee_UserID",
                table: "AwardLog",
                column: "UserID",
                principalTable: "Attendee",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RedemptionLog_Attendee_UserID",
                table: "RedemptionLog",
                column: "UserID",
                principalTable: "Attendee",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AwardLog_Attendee_UserID",
                table: "AwardLog");

            migrationBuilder.DropForeignKey(
                name: "FK_RedemptionLog_Attendee_UserID",
                table: "RedemptionLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Attendee",
                table: "Attendee");

            migrationBuilder.RenameTable(
                name: "Attendee",
                newName: "User");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
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
        }
    }
}
