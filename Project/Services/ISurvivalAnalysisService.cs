using Project.DataTransferObjects;

namespace Project.Services
{
    public interface ISurvivalAnalysisService
    {
        public Task<IEnumerable<SurvivalAnalysisDto>> GetSurvivalAnalysisStatistics(List<string?> neptunCodes);
    }
}
