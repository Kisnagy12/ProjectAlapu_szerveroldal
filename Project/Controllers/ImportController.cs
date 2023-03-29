using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Project.Services;

namespace Project.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ImportController : Controller
    {
        private readonly IImportService _importService;

        public ImportController(IImportService importService) 
        {
            _importService = importService;
        }

        [HttpPost]
        [EnableCors]
        public IActionResult ImportCourseStatisticsExcelFile(IFormFile file)
        {
            if(!IsValidExcelFile(file))
            {
                return BadRequest("Not valid excel file.");
            }

            _importService.ProcessCourseStatisticsExcelFile(file);

            return Ok();
        }
        
        [HttpPost]
        [EnableCors]
        public IActionResult ImportSurvivalAnalysisExcelFile(IFormFile file)
        {
            if (!IsValidExcelFile(file))
            {
                return BadRequest("Not valid excel file.");
            }

            _importService.ProcessSurvivalAnalysisExcelFile(file);

            return Ok();
        }

        private bool IsValidExcelFile(IFormFile file)
        {
            if (file != null && file.Length > 0 && !file.ContentType.Equals("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") )
            {
                return false;
            }
            return true;
        }
    }
}
