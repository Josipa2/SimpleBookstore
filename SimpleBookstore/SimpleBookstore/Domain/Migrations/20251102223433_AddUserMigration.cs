using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SimpleBookstore.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddUserMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

           // This is only for testing purposes. In a real application, you would not seed users with plain text passwords.
           migrationBuilder.InsertData(
               table: "Users",
               columns: new[] { "Username", "PasswordHash", "Role", "RefreshToken", "RefreshTokenExpiryTime" },
               values: new object[,]
               {
                    { "ReadUser", "AQAAAAIAAYagAAAAELL5hsXJFHzBee7UwGaYDhLzkRNG53MW3hQNkNc44EQy7T+nAOj9fkCRLXcaPNggEw==", "Read", "awT2PkKk/D3kS1wuk/5LRvMBMk1SYaXeEleoME0RweQ=", DateTime.MaxValue },
                    { "ReadWriteUser", "AQAAAAIAAYagAAAAEHpG5azH79NtB+Ad1+txqSxEOxRaHIfROf6ljzYl7D4w6RaBz4VGL/8OTE/4dIQ/yg==", "ReadWrite", "Wshnnf9LWNVFtkNIO90bDxPkz7cddQO1BA3u0F8HBzU=", DateTime.MaxValue }
               });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
