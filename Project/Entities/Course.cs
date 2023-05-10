namespace Project.Entities
{
    public class Course : AbstractEntity
    {
        public string Semester { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string Program { get; set; }
        public string Language { get; set; }
        public int Enrollment { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public ICollection<StudentOnCourse> StudentOnCourses { get; set; }
        public ICollection<TeacherOnCourse> TeacherOnCourses { get; set; }
    }
}
