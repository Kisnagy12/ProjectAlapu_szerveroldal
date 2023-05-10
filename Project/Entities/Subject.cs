namespace Project.Entities
{
    public class Subject : AbstractEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}
