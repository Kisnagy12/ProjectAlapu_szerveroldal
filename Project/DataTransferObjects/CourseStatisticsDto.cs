namespace Project.DataTransferObjects
{
    public class CourseStatisticsDto
    {
        public string TeacherName { get; set; } //Szűrés
        public double TeacherProportion { get; set; }
        public string CourseCode { get; set; }
        public string CourseSemester { get; set; } //Szűrés
        public string CourseType { get; set; } //Pl. Elmélet gyakorlat
        public string CourseProgram { get; set; } //Pl. Levelező nappali
        public string CourseLanguage { get; set; } 
        public string SubjectName { get; set; } //Szűrés
        public string SubjectCode { get; set; } //Szűrés

        public double SignaturePerEnrollment { get; set; }
        public double? CompletedPerSignature { get; set; }
        public double CompletedPerEnrollment { get; set; }

        public int NumberOfEnrollment { get; set; }
        public int NumberOfSignature { get; set; }
        public int NumberOfSignatureRefusal { get; set; }
        public int NumberOfCompleted { get; set; }
    }
}