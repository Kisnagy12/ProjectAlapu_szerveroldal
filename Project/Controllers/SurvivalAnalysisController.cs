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
    }
}
