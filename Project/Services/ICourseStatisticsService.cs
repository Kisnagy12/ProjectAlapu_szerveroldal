using Project.DataTransferObjects;

namespace Project.Services
{
    public interface ICourseStatisticsService
    {
        public IEnumerable<CourseStatisticsDto> GetCourseStatistics(List<string> semesterNames, List<string> subjectCodes, List<string> subjectNames, List<string> teacherNames);
    }
}
