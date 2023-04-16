using Microsoft.AspNetCore.Mvc;
using Project.DataTransferObjects;
using Project.Services;

namespace Project.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CourseStatisticsController : Controller
    {
        private readonly ICourseStatisticsService _courseStatisticsService;

        public CourseStatisticsController(ICourseStatisticsService courseStatisticsService)
        {
            _courseStatisticsService = courseStatisticsService;
        }

        [HttpPost]
        public async Task<IActionResult> GetCourseStatistics(CourseStatisticsFilterDto filter)
        {
            var result = await _courseStatisticsService.GetCourseStatistics(filter.SemesterNames, filter.SubjectCodes, filter.SubjectNames, filter.TeacherNames);
            return Ok(result);
        }
    }
}
