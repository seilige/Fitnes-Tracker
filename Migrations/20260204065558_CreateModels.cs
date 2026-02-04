using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace main.Migrations
{
    /// <inheritdoc />
    public partial class CreateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StandardPrograms",
                columns: table => new
                {
                    ProgId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Descripltion = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    WorkoutType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardPrograms", x => x.ProgId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Lastname = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.IdUser);
                });

            migrationBuilder.CreateTable(
                name: "CustomPrograms",
                columns: table => new
                {
                    CustProgId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatorId = table.Column<int>(type: "integer", nullable: true),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    Descripltion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomPrograms", x => x.CustProgId);
                    table.ForeignKey(
                        name: "FK_CustomPrograms_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "IdUser");
                });

            migrationBuilder.CreateTable(
                name: "UserStandardPrograms",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "integer", nullable: false),
                    ProgId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStandardPrograms", x => new { x.IdUser, x.ProgId });
                    table.ForeignKey(
                        name: "FK_UserStandardPrograms_StandardPrograms_ProgId",
                        column: x => x.ProgId,
                        principalTable: "StandardPrograms",
                        principalColumn: "ProgId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStandardPrograms_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomProgramUsers",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "integer", nullable: false),
                    CustProgId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomProgramUsers", x => new { x.IdUser, x.CustProgId });
                    table.ForeignKey(
                        name: "FK_CustomProgramUsers_CustomPrograms_CustProgId",
                        column: x => x.CustProgId,
                        principalTable: "CustomPrograms",
                        principalColumn: "CustProgId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomProgramUsers_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    ExId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Sets = table.Column<int>(type: "integer", nullable: true),
                    Reps = table.Column<int>(type: "integer", nullable: true),
                    CustomProgramCustProgId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.ExId);
                    table.ForeignKey(
                        name: "FK_Exercises_CustomPrograms_CustomProgramCustProgId",
                        column: x => x.CustomProgramCustProgId,
                        principalTable: "CustomPrograms",
                        principalColumn: "CustProgId");
                });

            migrationBuilder.CreateTable(
                name: "CustomProgramExercises",
                columns: table => new
                {
                    CustProgId = table.Column<int>(type: "integer", nullable: false),
                    ExId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomProgramExercises", x => new { x.CustProgId, x.ExId });
                    table.ForeignKey(
                        name: "FK_CustomProgramExercises_CustomPrograms_CustProgId",
                        column: x => x.CustProgId,
                        principalTable: "CustomPrograms",
                        principalColumn: "CustProgId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomProgramExercises_Exercises_ExId",
                        column: x => x.ExId,
                        principalTable: "Exercises",
                        principalColumn: "ExId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StandardProgramExercises",
                columns: table => new
                {
                    ProgId = table.Column<int>(type: "integer", nullable: false),
                    ExId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardProgramExercises", x => new { x.ProgId, x.ExId });
                    table.ForeignKey(
                        name: "FK_StandardProgramExercises_Exercises_ExId",
                        column: x => x.ExId,
                        principalTable: "Exercises",
                        principalColumn: "ExId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StandardProgramExercises_StandardPrograms_ProgId",
                        column: x => x.ProgId,
                        principalTable: "StandardPrograms",
                        principalColumn: "ProgId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomProgramExercises_ExId",
                table: "CustomProgramExercises",
                column: "ExId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomPrograms_CreatorId",
                table: "CustomPrograms",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomProgramUsers_CustProgId",
                table: "CustomProgramUsers",
                column: "CustProgId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_CustomProgramCustProgId",
                table: "Exercises",
                column: "CustomProgramCustProgId");

            migrationBuilder.CreateIndex(
                name: "IX_StandardProgramExercises_ExId",
                table: "StandardProgramExercises",
                column: "ExId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStandardPrograms_ProgId",
                table: "UserStandardPrograms",
                column: "ProgId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomProgramExercises");

            migrationBuilder.DropTable(
                name: "CustomProgramUsers");

            migrationBuilder.DropTable(
                name: "StandardProgramExercises");

            migrationBuilder.DropTable(
                name: "UserStandardPrograms");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "StandardPrograms");

            migrationBuilder.DropTable(
                name: "CustomPrograms");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
