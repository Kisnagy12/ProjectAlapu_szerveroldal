using Microsoft.AspNetCore.Mvc;
using Project.DataTransferObjects;
using Project.Services;

namespace Project.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SurvivalAnalysisController : Controller
    {
        private readonly ISurvivalAnalysisService _survivalAnalysisService;

        public SurvivalAnalysisController(ISurvivalAnalysisService survivalAnalysisService)
        {
            _survivalAnalysisService = survivalAnalysisService;
        }

        [HttpPost]
        public async Task<IActionResult> GetSurvivalAnalysisStatistics(SurvivalAnalysisFilterDto filter)
        {
            var result = await _survivalAnalysisService.GetSurvivalAnalysisStatistics(filter.NeptunCodes);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetSurvivalAnalysisPrediction(SurvivalAnalysisFilterDto filter)
        {
            var result = await _survivalAnalysisService.GetSurvivalAnalysisPrediction(filter.NeptunCodes);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetCourseStatistics(CourseStatisticsFilterDto filter)
        {
            var result = await _survivalAnalysisService.GetCourseStatistics(filter.SemesterNames, filter.SubjectCodes, filter.SubjectNames, filter.TeacherNames);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCourseStatisticsFilters()
        {
            var result = await _survivalAnalysisService.GetCourseStatisticsFilters();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetSurvivalAnalysisFilters()
        {
            var result = await _survivalAnalysisService.GetSurvivalAnalysisFilters();
            return Ok(result);
        }

    }
}
