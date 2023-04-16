using System.Data;

namespace Project.Services
{
    public interface IImportService
    {
        public Task ProcessCourseStatisticsExcelFile(IFormFile file);
        public Task ProcessSurvivalAnalysisExcelFile(IFormFile file);
    }
}
