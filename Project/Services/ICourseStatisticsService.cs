using Project.DataTransferObjects;

namespace Project.Services
{
    public interface ICourseStatisticsService
    {
        public Task<IEnumerable<CourseStatisticsDto>> GetCourseStatistics(List<string> semesterNames, List<string> subjectCodes, List<string> subjectNames, List<string> teacherNames);
        public Task<CourseStatisticsFilterDto> GetFilters();
    }
}
