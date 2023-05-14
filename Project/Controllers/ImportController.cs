using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Project.DbContexts;
using Project.Services;
using System.Diagnostics;

namespace Project.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ImportController : Controller
    {
        private readonly IImportService _importService;
        private readonly SurvivalAnalysisContext _survivalAnalysisContext;

        public ImportController(IImportService importService, SurvivalAnalysisContext survivalAnalysisContext)
        {
            _importService = importService;
            _survivalAnalysisContext = survivalAnalysisContext;
        }

        [HttpPost]
        [EnableCors]
        public async Task<IActionResult> ImportCourseStatisticsExcelFile(IFormFile file)
        {
            if (!IsValidExcelFile(file))
            {
                return BadRequest("Not valid excel file.");
            }

            await _importService.ProcessCourseStatisticsExcelFile(file);

            return Ok();
        }

        [HttpPost]
        [EnableCors]
        public async Task<IActionResult> ImportSurvivalAnalysisExcelFile(IFormFile file)
        {
            using var transaction = _survivalAnalysisContext.Database.BeginTransaction();
            try
            {
                await _importService.TruncateSurvivalPrediction();
                if (!IsValidExcelFile(file))
                {
                    return BadRequest("Not valid excel file.");
                }

                await _importService.ProcessSurvivalAnalysisExcelFile(file);

                string scriptPath = "C:\\survival_analysis_ml_component\\main.py";
                await _importService.RunPythonScriptAsync(scriptPath);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new { message = "Hiba a SurvivalAnalysis futása során!", exceptionMessage = ex.Message });
                throw;
            }

            return new JsonResult(new { message = "A fájl feldolgozása és az analízis futása sikeresen megtörtént!" });

            string fileName = "C:\\survival_analysis_ml_component\\main.py";
            string arguments = ""; // opcionális argumentumok
            string workingDirectory = "C:\\survival_analysis_ml_component\\";

            Process process = new Process();
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.WorkingDirectory = workingDirectory;
            process.StartInfo.UseShellExecute = false;

            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                return BadRequest();
            }

            return Ok();
        }

        private bool IsValidExcelFile(IFormFile file)
        {
            if (file != null && file.Length > 0 && !file.ContentType.Equals("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"))
            {
                return false;
            }
            return true;
        }
    }
}
