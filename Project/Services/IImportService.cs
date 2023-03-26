using System.Data;

namespace Project.Services
{
    public interface IImportService
    {
        public void ProcessCourseStatisticsExcelFile(IFormFile file);
        public void ProcessSurvivalAnalysisExcelFile(IFormFile file);
    }
}
