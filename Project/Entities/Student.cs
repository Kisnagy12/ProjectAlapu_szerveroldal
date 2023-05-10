namespace Project.Entities
{
    public class Student : AbstractEntity
    {
        public string NeptunCode { get; set; }
        public ICollection<StudentOnCourse> StudentOnCourses { get; set; }
    }
}
