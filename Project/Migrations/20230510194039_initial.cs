using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Project.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUserRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NeptunCode = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SurvivalAnalysisItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NeptunCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModuleCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdmissionSemester = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LegalRelationshipEstablishmentReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LegalRelationshipStartDate = table.Column<DateTime>(type: "date", nullable: true),
                    StatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LegalRelationshipEndDate = table.Column<DateTime>(type: "date", nullable: true),
                    LegalRelationshipTerminationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Semester = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Completed = table.Column<bool>(type: "bit", nullable: true),
                    EnrollmentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnrollmentCredit = table.Column<int>(type: "int", nullable: true),
                    SubjectTakenCount = table.Column<int>(type: "int", nullable: true),
                    Prerequisites = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recognized = table.Column<bool>(type: "bit", nullable: true),
                    EntryValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Valid = table.Column<bool>(type: "bit", nullable: true),
                    Program = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiplomaObtainingDate = table.Column<DateTime>(type: "date", nullable: true),
                    AdmissionScoreTotal = table.Column<int>(type: "int", nullable: true),
                    AdmissionFinancialStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LanguageExamFulfillmentDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurvivalAnalysisItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SurvivalPrediction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NeptunCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Semester_1 = table.Column<float>(type: "real", nullable: true),
                    Semester_2 = table.Column<float>(type: "real", nullable: true),
                    Semester_3 = table.Column<float>(type: "real", nullable: true),
                    Semester_4 = table.Column<float>(type: "real", nullable: true),
                    Semester_5 = table.Column<float>(type: "real", nullable: true),
                    Semester_6 = table.Column<float>(type: "real", nullable: true),
                    Semester_7 = table.Column<float>(type: "real", nullable: true),
                    Semester_8 = table.Column<float>(type: "real", nullable: true),
                    Semester_9 = table.Column<float>(type: "real", nullable: true),
                    Semester_10 = table.Column<float>(type: "real", nullable: true),
                    Semester_11 = table.Column<float>(type: "real", nullable: true),
                    risk_score = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurvivalPrediction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Semester = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Program = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Enrollment = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentsOnCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    DateOfSignature = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateOfSignatureRefusal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Completed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentsOnCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentsOnCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentsOnCourses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeachersOnCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    Proportion = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeachersOnCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeachersOnCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeachersOnCourses_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7caaa9c2-068c-4a42-8f96-79dac866f7f4", "73d1a299-4540-4296-8a9b-debc0f3b6ffe", "user", "USER" },
                    { "8ff2a9f2-5738-4097-80a3-6e364161263d", "415009c2-61df-4c07-830b-804af8ec5f2e", "admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SubjectId_Code",
                table: "Courses",
                columns: new[] { "SubjectId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_NeptunCode",
                table: "Students",
                column: "NeptunCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentsOnCourses_CourseId",
                table: "StudentsOnCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsOnCourses_StudentId_CourseId",
                table: "StudentsOnCourses",
                columns: new[] { "StudentId", "CourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_Code",
                table: "Subjects",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_Name",
                table: "Teachers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeachersOnCourses_CourseId_TeacherId",
                table: "TeachersOnCourses",
                columns: new[] { "CourseId", "TeacherId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeachersOnCourses_TeacherId",
                table: "TeachersOnCourses",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropTable(
                name: "IdentityRole");

            migrationBuilder.DropTable(
                name: "IdentityUserRole");

            migrationBuilder.DropTable(
                name: "StudentsOnCourses");

            migrationBuilder.DropTable(
                name: "SurvivalAnalysisItems");

            migrationBuilder.DropTable(
                name: "SurvivalPrediction");

            migrationBuilder.DropTable(
                name: "TeachersOnCourses");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Subjects");
        }
    }
}
