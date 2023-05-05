namespace Project.DataTransferObjects
{
    public class SurvivalAnalysisDto
    {
        public string? NeptunCode { get; set; }
        public string? AdmissionSemester { get; set; }
        public int? SemesterCount { get; set; }
        public int? Credit { get; set; }
        public double? WeightedGradePointAverage { get; set; }
    }
}
