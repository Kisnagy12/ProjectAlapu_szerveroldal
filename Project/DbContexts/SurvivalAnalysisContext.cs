using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Project.Entities;

namespace Project.DbContexts
{
    public class SurvivalAnalysisContext : DbContext
    {
        public DbSet<SurvivalAnalysisItem> SurvivalAnalysisItems { get; set; }
        public DbSet<SurvivalPrediction> SurvivalPrediction { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentOnCourse> StudentsOnCourses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TeacherOnCourse> TeachersOnCourses { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<IdentityRole> IdentityRole { get; set; }
        public DbSet<IdentityUserRole> IdentityUserRole { get; set; }

        public SurvivalAnalysisContext(DbContextOptions<SurvivalAnalysisContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Subject)
                .WithMany(s => s.Courses)
                .HasForeignKey(c => c.SubjectId);

            modelBuilder.Entity<StudentOnCourse>()
                .HasOne(s => s.Course)
                .WithMany(c => c.StudentOnCourses)
                .HasForeignKey(s => s.CourseId);

            modelBuilder.Entity<StudentOnCourse>()
                .HasOne(s => s.Student)
                .WithMany(s => s.StudentOnCourses)
                .HasForeignKey(s => s.StudentId);

            modelBuilder.Entity<TeacherOnCourse>()
                .HasOne(t => t.Course)
                .WithMany(c => c.TeacherOnCourses)
                .HasForeignKey(t => t.CourseId);

            modelBuilder.Entity<TeacherOnCourse>()
                .HasOne(t => t.Teacher)
                .WithMany(t => t.TeacherOnCourses)
                .HasForeignKey(t => t.TeacherId);

            modelBuilder.Entity<Course>()
                .HasIndex(c => new { c.SubjectId, c.Code })
                .IsUnique();

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.NeptunCode)
                .IsUnique();

            modelBuilder.Entity<StudentOnCourse>()
                .HasIndex(s => new { s.StudentId, s.CourseId })
                .IsUnique();

            modelBuilder.Entity<Subject>()
                .HasIndex(s => s.Code)
                .IsUnique();

            modelBuilder.Entity<Teacher>()
                .HasIndex(t => t.Name)
                .IsUnique();

            modelBuilder.Entity<TeacherOnCourse>()
                .HasIndex(t => new { t.CourseId, t.TeacherId })
                .IsUnique();

            modelBuilder.Entity<ApplicationUser>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<IdentityRole>()
                .HasData(new IdentityRole { Id = "7caaa9c2-068c-4a42-8f96-79dac866f7f4", ConcurrencyStamp = "73d1a299-4540-4296-8a9b-debc0f3b6ffe", Name = "user", NormalizedName = "USER" });
            modelBuilder.Entity<IdentityRole>()
                .HasData(new IdentityRole { Id = "8ff2a9f2-5738-4097-80a3-6e364161263d", ConcurrencyStamp = "415009c2-61df-4c07-830b-804af8ec5f2e", Name = "admin", NormalizedName = "ADMIN" });
            modelBuilder.Entity<IdentityRole>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<IdentityUserRole>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<SurvivalAnalysisItem>()
                .Property(s => s.LegalRelationshipStartDate)
                .HasColumnType("date");

            modelBuilder.Entity<SurvivalAnalysisItem>()
                .Property(s => s.LegalRelationshipEndDate)
                   .HasColumnType("date");

            modelBuilder.Entity<SurvivalAnalysisItem>()
                .Property(s => s.DiplomaObtainingDate)
                .HasColumnType("date");

            modelBuilder.Entity<SurvivalAnalysisItem>()
                .Property(s => s.LanguageExamFulfillmentDate)
                .HasColumnType("date");
        }
    }
}
