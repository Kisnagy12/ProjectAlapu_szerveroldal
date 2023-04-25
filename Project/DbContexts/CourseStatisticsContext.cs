using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Entities;
using Project.Entities.CourseStatistics;

namespace Project.DbContexts
{
    public class CourseStatisticsContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentOnCourse> StudentsOnCourses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TeacherOnCourse> TeachersOnCourses { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<IdentityRole> IdentityRole { get; set; }
        public DbSet<IdentityUserRole> IdentityUserRole { get; set; }

        public CourseStatisticsContext(DbContextOptions<CourseStatisticsContext> options) : base(options)
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
                .HasKey(x => x.Id);

            modelBuilder.Entity<IdentityUserRole>()
                .HasKey(x => x.Id);
        }
    }
}