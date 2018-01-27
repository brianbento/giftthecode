using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Indspire.Soaring.Engagement.Data.Migrations
{
    public partial class RenamedEventNumberToAwardNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
#if !DISABLE_UP_MIGRATIONS
            migrationBuilder.DropColumn(
                name: "EventNumber",
                table: "Award");

            migrationBuilder.AddColumn<string>(
                name: "AwardNumber",
                table: "Award",
                type: "nvarchar(max)",
                nullable: true);
#endif
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwardNumber",
                table: "Award");

            migrationBuilder.AddColumn<string>(
                name: "EventNumber",
                table: "Award",
                nullable: true);
        }
    }
}
