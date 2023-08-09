using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banking.API.Migrations
{
    /// <inheritdoc />
    public partial class SupportSystemUpgrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "SupportTickets",
                newName: "SupportTicketRequests");

            migrationBuilder.DropColumn(
                name: "ResolvedDate",
                table: "SupportTicketRequests");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "SupportTicketRequests",
                newName: "DateUpdated");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SupportTicketRequests",
                newName: "TicketId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "SupportTicketRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SupportTicketRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsFromSupportTeam",
                table: "SupportTicketRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "SupportTicketRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SupportTicketResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RespondedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsFromSupportTeam = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTicketResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportTicketResponses_SupportTicketRequests_TicketId",
                        column: x => x.TicketId,
                        principalTable: "SupportTicketRequests",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupportTicketResponses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicketResponses_TicketId",
                table: "SupportTicketResponses",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicketResponses_UserId",
                table: "SupportTicketResponses",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {            
            migrationBuilder.DropTable(
                name: "SupportTicketResponses");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "SupportTicketRequests");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "SupportTicketRequests");

            migrationBuilder.DropColumn(
                name: "IsFromSupportTeam",
                table: "SupportTicketRequests");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "SupportTicketRequests");

            migrationBuilder.RenameColumn(
                name: "DateUpdated",
                table: "SupportTicketRequests",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "TicketId",
                table: "SupportTicketRequests",
                newName: "Id");

            migrationBuilder.AddColumn<DateTime>(
                name: "ResolvedDate",
                table: "SupportTicketRequests",
                type: "datetime2",
                nullable: true);
        }
    }
}
