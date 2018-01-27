using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Indspire.Soaring.Engagement.Data.Migrations
{
    public partial class Award : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
#if !DISABLE_UP_MIGRATIONS
            migrationBuilder.CreateTable(
                name: "Award",
                columns: table => new
                {
                    AwardID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    EventNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    VendorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Award", x => x.AwardID);
                });
#endif
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Award");
        }
    }
}
