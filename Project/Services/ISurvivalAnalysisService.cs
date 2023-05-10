using Project.DataTransferObjects;
using Project.Entities;

namespace Project.Services
{
    public interface ISurvivalAnalysisService
    {
        public Task<IEnumerable<SurvivalPrediction>> GetSurvivalAnalysisPrediction(List<string?> neptunCodes);
        public Task<IEnumerable<SurvivalAnalysisDto>> GetSurvivalAnalysisStatistics(List<string?> neptunCodes);
        public Task<IEnumerable<CourseStatisticsDto>> GetCourseStatistics(List<string> semesterNames, List<string> subjectCodes, List<string> subjectNames, List<string> teacherNames);
        public Task<CourseStatisticsFilterDto> GetCourseStatisticsFilters();
        public Task<SurvivalAnalysisFilterDto> GetSurvivalAnalysisFilters();
    }
}
