namespace Project.Entities
{
    public class TeacherOnCourse : AbstractEntity
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public double Proportion { get; set; }
    }
}