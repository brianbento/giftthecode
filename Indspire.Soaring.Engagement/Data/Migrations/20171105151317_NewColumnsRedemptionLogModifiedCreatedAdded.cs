using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Indspire.Soaring.Engagement.Data.Migrations
{
    public partial class NewColumnsRedemptionLogModifiedCreatedAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
#if !DISABLE_UP_MIGRATIONS
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "RedemptionLog",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "RedemptionLog",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
#endif
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "RedemptionLog");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "RedemptionLog");
        }
    }
}
