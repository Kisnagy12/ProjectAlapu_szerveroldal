﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Project.DbContexts;

#nullable disable

namespace Project.Migrations
{
    [DbContext(typeof(SurvivalAnalysisContext))]
    [Migration("20230513171908_DefaultAdminSeed")]
    partial class DefaultAdminSeed
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("IdentityRole");

                    b.HasData(
                        new
                        {
                            Id = "7caaa9c2-068c-4a42-8f96-79dac866f7f4",
                            ConcurrencyStamp = "73d1a299-4540-4296-8a9b-debc0f3b6ffe",
                            Name = "user",
                            NormalizedName = "USER"
                        },
                        new
                        {
                            Id = "8ff2a9f2-5738-4097-80a3-6e364161263d",
                            ConcurrencyStamp = "415009c2-61df-4c07-830b-804af8ec5f2e",
                            Name = "admin",
                            NormalizedName = "ADMIN"
                        });
                });

            modelBuilder.Entity("Project.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ApplicationUser");

                    b.HasData(
                        new
                        {
                            Id = "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "05032bc6-0a8a-4867-ae10-9a67eeb50d1b",
                            Email = "",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            NormalizedUserName = "ADMIN",
                            PasswordHash = "AQAAAAEAACcQAAAAEPsmRgUEow/LIap64iPMu5KLfe8h7XRyY9lJCqIxmCPKBW2GoyZMBfe68k2HTGyadA==",
                            PhoneNumberConfirmed = false,
                            RefreshTokenExpiryTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SecurityStamp = "465c2331-d5ae-4b0f-a131-e20f05349243",
                            TwoFactorEnabled = false,
                            UserName = "admin"
                        });
                });

            modelBuilder.Entity("Project.Entities.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Enrollment")
                        .HasColumnType("int");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Program")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Semester")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubjectId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SubjectId", "Code")
                        .IsUnique();

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("Project.Entities.IdentityUserRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("IdentityUserRole");

                    b.HasData(
                        new
                        {
                            Id = "b268a4bb-79b1-467a-957c-1fc2401e087e",
                            RoleId = "8ff2a9f2-5738-4097-80a3-6e364161263d",
                            UserId = "02174cf0–9412–4cfe-afbf-59f706d72cf6"
                        });
                });

            modelBuilder.Entity("Project.Entities.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("NeptunCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("NeptunCode")
                        .IsUnique();

                    b.ToTable("Students");
                });

            modelBuilder.Entity("Project.Entities.StudentOnCourse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Completed")
                        .HasColumnType("bit");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateOfSignature")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateOfSignatureRefusal")
                        .HasColumnType("datetime2");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentId", "CourseId")
                        .IsUnique();

                    b.ToTable("StudentsOnCourses");
                });

            modelBuilder.Entity("Project.Entities.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("Project.Entities.SurvivalAnalysisItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdmissionFinancialStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("AdmissionScoreTotal")
                        .HasColumnType("int");

                    b.Property<string>("AdmissionSemester")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Completed")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DiplomaObtainingDate")
                        .HasColumnType("date");

                    b.Property<int?>("EnrollmentCredit")
                        .HasColumnType("int");

                    b.Property<string>("EnrollmentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EntryType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EntryValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LanguageExamFulfillmentDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("LegalRelationshipEndDate")
                        .HasColumnType("date");

                    b.Property<string>("LegalRelationshipEstablishmentReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LegalRelationshipStartDate")
                        .HasColumnType("date");

                    b.Property<string>("LegalRelationshipTerminationReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModuleCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NeptunCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prerequisites")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Program")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Recognized")
                        .HasColumnType("bit");

                    b.Property<string>("Semester")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StatusId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubjectCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubjectName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SubjectTakenCount")
                        .HasColumnType("int");

                    b.Property<bool?>("Valid")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("SurvivalAnalysisItems");
                });

            modelBuilder.Entity("Project.Entities.SurvivalPrediction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("NeptunCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("Semester_1")
                        .HasColumnType("real");

                    b.Property<float?>("Semester_10")
                        .HasColumnType("real");

                    b.Property<float?>("Semester_11")
                        .HasColumnType("real");

                    b.Property<float?>("Semester_2")
                        .HasColumnType("real");

                    b.Property<float?>("Semester_3")
                        .HasColumnType("real");

                    b.Property<float?>("Semester_4")
                        .HasColumnType("real");

                    b.Property<float?>("Semester_5")
                        .HasColumnType("real");

                    b.Property<float?>("Semester_6")
                        .HasColumnType("real");

                    b.Property<float?>("Semester_7")
                        .HasColumnType("real");

                    b.Property<float?>("Semester_8")
                        .HasColumnType("real");

                    b.Property<float?>("Semester_9")
                        .HasColumnType("real");

                    b.Property<float?>("risk_score")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("SurvivalPrediction");
                });

            modelBuilder.Entity("Project.Entities.Teacher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("Project.Entities.TeacherOnCourse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<double>("Proportion")
                        .HasColumnType("float");

                    b.Property<int>("TeacherId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TeacherId");

                    b.HasIndex("CourseId", "TeacherId")
                        .IsUnique();

                    b.ToTable("TeachersOnCourses");
                });

            modelBuilder.Entity("Project.Entities.Course", b =>
                {
                    b.HasOne("Project.Entities.Subject", "Subject")
                        .WithMany("Courses")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("Project.Entities.StudentOnCourse", b =>
                {
                    b.HasOne("Project.Entities.Course", "Course")
                        .WithMany("StudentOnCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Project.Entities.Student", "Student")
                        .WithMany("StudentOnCourses")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Project.Entities.TeacherOnCourse", b =>
                {
                    b.HasOne("Project.Entities.Course", "Course")
                        .WithMany("TeacherOnCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Project.Entities.Teacher", "Teacher")
                        .WithMany("TeacherOnCourses")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Project.Entities.Course", b =>
                {
                    b.Navigation("StudentOnCourses");

                    b.Navigation("TeacherOnCourses");
                });

            modelBuilder.Entity("Project.Entities.Student", b =>
                {
                    b.Navigation("StudentOnCourses");
                });

            modelBuilder.Entity("Project.Entities.Subject", b =>
                {
                    b.Navigation("Courses");
                });

            modelBuilder.Entity("Project.Entities.Teacher", b =>
                {
                    b.Navigation("TeacherOnCourses");
                });
#pragma warning restore 612, 618
        }
    }
}
