using Project.DataTransferObjects;
using Project.DbContexts;
using Project.Entities.CourseStatistics;

namespace Project.Services
{
    public class CourseStatisticsService : ICourseStatisticsService
    {
        private readonly CourseStatisticsContext _courseStatisticsContext;

        public CourseStatisticsService(CourseStatisticsContext courseStatisticsContext)
        {
            _courseStatisticsContext = courseStatisticsContext;
        }

        public IEnumerable<CourseStatisticsDto> GetCourseStatistics(
            List<string> semesterNames,
            List<string> subjectCodes,
            List<string> subjectNames, 
            List<string> teacherNames)
        {
            var result = (from Course in _courseStatisticsContext.Courses
                         join Subject in _courseStatisticsContext.Subjects on Course.SubjectId equals Subject.Id
                         join TeacherOnCourse in _courseStatisticsContext.TeachersOnCourses on Course.Id equals TeacherOnCourse.CourseId
                         join StudentOnCourse in _courseStatisticsContext.StudentsOnCourses on Course.Id equals StudentOnCourse.CourseId
                         join Teacher in _courseStatisticsContext.Teachers on TeacherOnCourse.TeacherId equals Teacher.Id
                         where semesterNames.Contains(Course.Semester) && subjectCodes.Contains(Subject.Code) && subjectNames.Contains(Subject.Name) && teacherNames.Contains(Teacher.Name)
                         group new { Teacher, TeacherOnCourse, Subject, Course, StudentOnCourse } by new { TeacherId = Teacher.Id, SubjectId = Subject.Id, CourseId = Course.Id } into grp
                         select new CourseStatisticsDto
                         {
                             TeacherName = grp.First().Teacher.Name,
                             TeacherProportion = grp.First().TeacherOnCourse.Proportion,
                             CourseCode = grp.First().Course.Code,
                             CourseSemester = grp.First().Course.Semester,
                             CourseType = grp.First().Course.Type,
                             CourseProgram = grp.First().Course.Program,
                             CourseLanguage = grp.First().Course.Language,
                             SubjectName = grp.First().Subject.Name,
                             SubjectCode = grp.First().Subject.Code,
                             SignaturePerEnrollment = 1.0 * grp.Count(s => s.StudentOnCourse.DateOfSignature != null) / grp.Count(),
                             CompletedPerSignature = grp.Count(s => s.StudentOnCourse.DateOfSignature != null) == 0 ? null : 1.0 * grp.Count(s => s.StudentOnCourse.Completed == true) / grp.Count(s => s.StudentOnCourse.DateOfSignature != null),
                             CompletedPerEnrollment = 1.0 * grp.Count(s => s.StudentOnCourse.Completed == true) / grp.Count(),
                             NumberOfEnrollment = grp.Count(),
                             NumberOfSignature = grp.Count(s => s.StudentOnCourse.DateOfSignature != null),
                             NumberOfSignatureRefusal = grp.Count(s => s.StudentOnCourse.DateOfSignatureRefusal != null),
                             NumberOfCompleted = grp.Count(s => s.StudentOnCourse.Completed == true)
                         }).ToList();

            return result;
        }
    }
}
