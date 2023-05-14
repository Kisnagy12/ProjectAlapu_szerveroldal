namespace Project.Services
{
    public interface IImportService
    {
        public Task ProcessCourseStatisticsExcelFile(IFormFile file);
        public Task ProcessSurvivalAnalysisExcelFile(IFormFile file);
        public Task RunPythonScriptAsync(string path);
        public Task TruncateSurvivalPrediction();
    }
}
