using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Indspire.Soaring.Engagement.Data.Migrations
{
    public partial class NewColumnsRedemptionPointsRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
#if !DISABLE_UP_MIGRATIONS
            migrationBuilder.AddColumn<int>(
                name: "PointsRequired",
                table: "Redemption",
                type: "int",
                nullable: false,
                defaultValue: 0);
#endif
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PointsRequired",
                table: "Redemption");
        }
    }
}
