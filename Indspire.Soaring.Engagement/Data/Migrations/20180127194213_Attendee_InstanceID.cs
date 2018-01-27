using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Indspire.Soaring.Engagement.Data.Migrations
{
    public partial class Attendee_InstanceID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstanceID",
                table: "Attendee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Attendee_InstanceID",
                table: "Attendee",
                column: "InstanceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendee_Instance_InstanceID",
                table: "Attendee",
                column: "InstanceID",
                principalTable: "Instance",
                principalColumn: "InstanceID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendee_Instance_InstanceID",
                table: "Attendee");

            migrationBuilder.DropIndex(
                name: "IX_Attendee_InstanceID",
                table: "Attendee");

            migrationBuilder.DropColumn(
                name: "InstanceID",
                table: "Attendee");
        }
    }
}
