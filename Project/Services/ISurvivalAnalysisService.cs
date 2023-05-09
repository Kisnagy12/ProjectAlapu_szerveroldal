using Project.DataTransferObjects;
using Project.Entities.SurvivalAnalysis;

namespace Project.Services
{
    public interface ISurvivalAnalysisService
    {
        public Task<IEnumerable<SurvivalPrediction>> GetSurvivalAnalysisPrediction(List<string?> neptunCodes);
        public Task<IEnumerable<SurvivalAnalysisDto>> GetSurvivalAnalysisStatistics(List<string?> neptunCodes);
    }
}
