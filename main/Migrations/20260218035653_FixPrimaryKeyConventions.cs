using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace main.Migrations
{
    /// <inheritdoc />
    public partial class FixPrimaryKeyConventions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StandardPrograms",
                columns: table => new
                {
                    ProgramId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    WorkoutType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardPrograms", x => x.ProgramId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Lastname = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Author = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "CustomPrograms",
                columns: table => new
                {
                    CustomProgramId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatorId = table.Column<int>(type: "integer", nullable: true),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatorUserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomPrograms", x => x.CustomProgramId);
                    table.ForeignKey(
                        name: "FK_CustomPrograms_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_CustomPrograms_Users_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
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
                        principalColumn: "ProgramId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStandardPrograms_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomProgramUsers",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "integer", nullable: false),
                    CustomProgramId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomProgramUsers", x => new { x.IdUser, x.CustomProgramId });
                    table.ForeignKey(
                        name: "FK_CustomProgramUsers_CustomPrograms_CustomProgramId",
                        column: x => x.CustomProgramId,
                        principalTable: "CustomPrograms",
                        principalColumn: "CustomProgramId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomProgramUsers_Users_IdUser",
                        column: x => x.IdUser,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Sets = table.Column<int>(type: "integer", nullable: true),
                    Reps = table.Column<int>(type: "integer", nullable: true),
                    MuscleGroup = table.Column<int>(type: "integer", nullable: false),
                    CustomProgramId = table.Column<int>(type: "integer", nullable: true),
                    StandardProgramProgramId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.ExerciseId);
                    table.ForeignKey(
                        name: "FK_Exercises_CustomPrograms_CustomProgramId",
                        column: x => x.CustomProgramId,
                        principalTable: "CustomPrograms",
                        principalColumn: "CustomProgramId");
                    table.ForeignKey(
                        name: "FK_Exercises_StandardPrograms_StandardProgramProgramId",
                        column: x => x.StandardProgramProgramId,
                        principalTable: "StandardPrograms",
                        principalColumn: "ProgramId");
                });

            migrationBuilder.CreateTable(
                name: "WorkoutSessions",
                columns: table => new
                {
                    WorkoutSessionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    StandardProgramId = table.Column<int>(type: "integer", nullable: true),
                    CustomProgramId = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutSessions", x => x.WorkoutSessionId);
                    table.ForeignKey(
                        name: "FK_WorkoutSessions_CustomPrograms_CustomProgramId",
                        column: x => x.CustomProgramId,
                        principalTable: "CustomPrograms",
                        principalColumn: "CustomProgramId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_WorkoutSessions_StandardPrograms_StandardProgramId",
                        column: x => x.StandardProgramId,
                        principalTable: "StandardPrograms",
                        principalColumn: "ProgramId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_WorkoutSessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomProgramExercises",
                columns: table => new
                {
                    CustomProgramId = table.Column<int>(type: "integer", nullable: false),
                    ExerciseId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomProgramExercises", x => new { x.CustomProgramId, x.ExerciseId });
                    table.ForeignKey(
                        name: "FK_CustomProgramExercises_CustomPrograms_CustomProgramId",
                        column: x => x.CustomProgramId,
                        principalTable: "CustomPrograms",
                        principalColumn: "CustomProgramId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomProgramExercises_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "ExerciseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StandardProgramExercises",
                columns: table => new
                {
                    ProgramId = table.Column<int>(type: "integer", nullable: false),
                    ExerciseId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardProgramExercises", x => new { x.ProgramId, x.ExerciseId });
                    table.ForeignKey(
                        name: "FK_StandardProgramExercises_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "ExerciseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StandardProgramExercises_StandardPrograms_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "StandardPrograms",
                        principalColumn: "ProgramId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutExerciseSets",
                columns: table => new
                {
                    WorkoutExerciseSetId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExerciseId = table.Column<int>(type: "integer", nullable: false),
                    Sets = table.Column<int>(type: "integer", nullable: false),
                    Reps = table.Column<int>(type: "integer", nullable: false),
                    Weight = table.Column<double>(type: "double precision", nullable: false),
                    WorkoutSessionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutExerciseSets", x => x.WorkoutExerciseSetId);
                    table.ForeignKey(
                        name: "FK_WorkoutExerciseSets_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "ExerciseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkoutExerciseSets_WorkoutSessions_WorkoutSessionId",
                        column: x => x.WorkoutSessionId,
                        principalTable: "WorkoutSessions",
                        principalColumn: "WorkoutSessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomProgramExercises_ExerciseId",
                table: "CustomProgramExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomPrograms_CreatorId",
                table: "CustomPrograms",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomPrograms_CreatorUserId",
                table: "CustomPrograms",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomProgramUsers_CustomProgramId",
                table: "CustomProgramUsers",
                column: "CustomProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_CustomProgramId",
                table: "Exercises",
                column: "CustomProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_StandardProgramProgramId",
                table: "Exercises",
                column: "StandardProgramProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_StandardProgramExercises_ExerciseId",
                table: "StandardProgramExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStandardPrograms_ProgId",
                table: "UserStandardPrograms",
                column: "ProgId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExerciseSets_ExerciseId",
                table: "WorkoutExerciseSets",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExerciseSets_WorkoutSessionId",
                table: "WorkoutExerciseSets",
                column: "WorkoutSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSessions_CustomProgramId",
                table: "WorkoutSessions",
                column: "CustomProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSessions_StandardProgramId",
                table: "WorkoutSessions",
                column: "StandardProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSessions_UserId",
                table: "WorkoutSessions",
                column: "UserId");
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
                name: "WorkoutExerciseSets");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "WorkoutSessions");

            migrationBuilder.DropTable(
                name: "CustomPrograms");

            migrationBuilder.DropTable(
                name: "StandardPrograms");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
