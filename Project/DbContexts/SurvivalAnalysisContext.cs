using Microsoft.EntityFrameworkCore;
using Project.Entities.SurvivalAnalysis;

namespace Project.DbContexts
{
    public class SurvivalAnalysisContext : DbContext
    {
        public DbSet<SurvivalAnalysisItem> SurvivalAnalysisItems { get; set; }

        public SurvivalAnalysisContext(DbContextOptions<SurvivalAnalysisContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
