namespace Project.Entities.CourseStatistics
{
    public class StudentOnCourse : AbstractEntity
    {
        public int CourseId { get; set; } 
        public Course Course { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public DateTime? DateOfSignature { get; set; }
        public DateTime? DateOfSignatureRefusal { get; set; }
        public bool Completed { get; set; }
    }
}
