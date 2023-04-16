namespace Project.DataTransferObjects
{
    public class CourseStatisticsFilterDto
    {
        public List<string> SemesterNames { get; set; }
        public List<string> SubjectCodes { get; set; }
        public List<string> SubjectNames { get; set; }
        public List<string> TeacherNames { get; set; }
    }
}
