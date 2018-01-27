using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Indspire.Soaring.Engagement.Data.Migrations
{
    public partial class Redemption_InstanceID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstanceID",
                table: "Redemption",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Redemption_InstanceID",
                table: "Redemption",
                column: "InstanceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Redemption_Instance_InstanceID",
                table: "Redemption",
                column: "InstanceID",
                principalTable: "Instance",
                principalColumn: "InstanceID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Redemption_Instance_InstanceID",
                table: "Redemption");

            migrationBuilder.DropIndex(
                name: "IX_Redemption_InstanceID",
                table: "Redemption");

            migrationBuilder.DropColumn(
                name: "InstanceID",
                table: "Redemption");
        }
    }
}
