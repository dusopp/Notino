using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Notino.Persistence.MSSQL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    InternalId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RawJson = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => new { x.Id, x.InternalId });
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });


            
            migrationBuilder.CreateTable(
                name: "DocumentTags",
                columns: table => new
                {
                    DocumentId = table.Column<string>(nullable: false),
                    DocumentInternalId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTags", x => new { x.DocumentId, x.DocumentInternalId, x.TagId });
                    table.ForeignKey(
                        name: "FK_DocumentTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction); //to by bolo lepsie nastavit v konfiguracii cez fluentApi. Malo casu
                    table.ForeignKey(
                        name: "FK_DocumentTags_Documents_DocumentId_DocumentInternalId",
                        columns: x => new { x.DocumentId, x.DocumentInternalId },
                        principalTable: "Documents",
                        principalColumns: new[] { "Id", "InternalId" },
                        onDelete: ReferentialAction.NoAction);//to by bolo lepsie nastavit v konfiguracii cez fluentApi
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTags_TagId",
                table: "DocumentTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentTags");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Documents");
        }
    }
}
