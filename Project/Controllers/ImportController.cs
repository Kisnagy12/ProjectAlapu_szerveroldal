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
            if (!IsValidExcelFile(file))
            {
                return BadRequest("Not valid excel file.");
            }

            await _importService.TruncateSurvivalPrediction();
            await _importService.ProcessSurvivalAnalysisExcelFile(file);

            //string scriptPath = "C:\\survival_analysis_ml_component\\main.py";
            //await _importService.RunPythonScriptAsync(scriptPath);

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            //startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "C:\\ProgramData\\anaconda3\\Library\\bin\\conda.bat";
            startInfo.Arguments = "run -n base python C:\\survival_analysis_ml_component\\main.py";
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();

            string result = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            Console.WriteLine("ITTVANARESULT" + result);
            Console.WriteLine("ITTVANAZERROR" + error);


            process.WaitForExit();

            return new JsonResult(new { message = result, errorr = error});
        }

        private bool IsValidExcelFile(IFormFile file)
        {
            if (file != null && file.Length > 0 && !file.ContentType.Equals("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"))
            {
                return false;
            }
            return true;
        }

        private static void ConsoleLog(object process, DataReceivedEventArgs args)
        {
            System.IO.File.AppendAllText("C:\\consolelog.txt", args.Data);
        }
    }
}
