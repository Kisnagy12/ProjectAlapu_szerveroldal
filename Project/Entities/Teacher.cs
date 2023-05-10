namespace Project.Entities
{
    public class Teacher : AbstractEntity
    {
        public string Name { get; set; }
        public ICollection<TeacherOnCourse> TeacherOnCourses { get; set; }
    }
}
