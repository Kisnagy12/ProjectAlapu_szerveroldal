namespace Project.Entities
{
    public class SurvivalAnalysisItem : AbstractEntity
    {
        public string? NeptunCode { get; set; }
        public string? ModuleCode { get; set; }
        public string? AdmissionSemester { get; set; }
        public string? LegalRelationshipEstablishmentReason { get; set; }
        public DateTime? LegalRelationshipStartDate { get; set; }
        public string? StatusId { get; set; }
        public DateTime? LegalRelationshipEndDate { get; set; }
        public string? LegalRelationshipTerminationReason { get; set; }
        public string? Semester { get; set; }
        public string? SubjectCode { get; set; }
        public string? SubjectName { get; set; }
        public bool? Completed { get; set; }
        public string? EnrollmentType { get; set; }
        public int? EnrollmentCredit { get; set; }
        public int? SubjectTakenCount { get; set; }
        public string? Prerequisites { get; set; }
        public bool? Recognized { get; set; }
        public string? EntryValue { get; set; }
        public string? EntryType { get; set; }
        public bool? Valid { get; set; }
        public string? Program { get; set; }
        public DateTime? DiplomaObtainingDate { get; set; }
        public int? AdmissionScoreTotal { get; set; }
        public string? AdmissionFinancialStatus { get; set; }
        public DateTime? LanguageExamFulfillmentDate { get; set; }
    }
}
