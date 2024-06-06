using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auis.StackOverflow.DatabaseMigrations.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "StackOverflow");

            migrationBuilder.CreateTable(
                name: "WebDataFiles",
                schema: "StackOverflow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "Name of the web data file at StackOverflow archive."),
                    Link = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "Partial link to the web data file at StackOverflow archive."),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    ExternalLastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, comment: "Datetime of last modified of the web data file at StackOverflow archive."),
                    ProcessingStatus = table.Column<int>(type: "int", nullable: false, comment: "Processing status of file based on enumeration in source code."),
                    IsSynchronizationEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebDataFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                schema: "StackOverflow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    WebDataFileId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", maxLength: -1, nullable: false),
                    ExternalLastActivityDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => new { x.WebDataFileId, x.Id });
                    table.ForeignKey(
                        name: "FK_Posts_WebDataFiles_WebDataFileId",
                        column: x => x.WebDataFileId,
                        principalSchema: "StackOverflow",
                        principalTable: "WebDataFiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AcceptedAnswers",
                schema: "StackOverflow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PostWebDataFileId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", maxLength: -1, nullable: false),
                    ExternalLastActivityDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcceptedAnswers", x => new { x.PostWebDataFileId, x.Id });
                    table.ForeignKey(
                        name: "FK_AcceptedAnswers_Posts_PostWebDataFileId_PostId",
                        columns: x => new { x.PostWebDataFileId, x.PostId },
                        principalSchema: "StackOverflow",
                        principalTable: "Posts",
                        principalColumns: new[] { "WebDataFileId", "Id" });
                });

            migrationBuilder.CreateTable(
                name: "PostComments",
                schema: "StackOverflow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    WebDataFileId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExternalCreationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    PostWebDataFileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostComments", x => new { x.WebDataFileId, x.Id });
                    table.ForeignKey(
                        name: "FK_PostComments_Posts_PostWebDataFileId_PostId",
                        columns: x => new { x.PostWebDataFileId, x.PostId },
                        principalSchema: "StackOverflow",
                        principalTable: "Posts",
                        principalColumns: new[] { "WebDataFileId", "Id" });
                });

            migrationBuilder.CreateTable(
                name: "AcceptedAnswerComments",
                schema: "StackOverflow",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    WebDataFileId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExternalCreationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AcceptedAnswerId = table.Column<int>(type: "int", nullable: false),
                    AcceptedAnswerPostWebDataFileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcceptedAnswerComments", x => new { x.WebDataFileId, x.Id });
                    table.ForeignKey(
                        name: "FK_AcceptedAnswerComments_AcceptedAnswers_AcceptedAnswerPostWebDataFileId_AcceptedAnswerId",
                        columns: x => new { x.AcceptedAnswerPostWebDataFileId, x.AcceptedAnswerId },
                        principalSchema: "StackOverflow",
                        principalTable: "AcceptedAnswers",
                        principalColumns: new[] { "PostWebDataFileId", "Id" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcceptedAnswerComments_AcceptedAnswerPostWebDataFileId_AcceptedAnswerId",
                schema: "StackOverflow",
                table: "AcceptedAnswerComments",
                columns: new[] { "AcceptedAnswerPostWebDataFileId", "AcceptedAnswerId" });

            migrationBuilder.CreateIndex(
                name: "IX_AcceptedAnswers_PostWebDataFileId_PostId",
                schema: "StackOverflow",
                table: "AcceptedAnswers",
                columns: new[] { "PostWebDataFileId", "PostId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_PostWebDataFileId_PostId",
                schema: "StackOverflow",
                table: "PostComments",
                columns: new[] { "PostWebDataFileId", "PostId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcceptedAnswerComments",
                schema: "StackOverflow");

            migrationBuilder.DropTable(
                name: "PostComments",
                schema: "StackOverflow");

            migrationBuilder.DropTable(
                name: "AcceptedAnswers",
                schema: "StackOverflow");

            migrationBuilder.DropTable(
                name: "Posts",
                schema: "StackOverflow");

            migrationBuilder.DropTable(
                name: "WebDataFiles",
                schema: "StackOverflow");
        }
    }
}
