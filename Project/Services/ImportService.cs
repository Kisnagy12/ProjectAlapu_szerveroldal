using ExcelDataReader;
using IronPython.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Scripting.Hosting;
using Project.DbContexts;
using Project.Entities;
using System.Data;

namespace Project.Services
{
    public class ImportService : IImportService
    {
        private readonly SurvivalAnalysisContext _survivalAnalysisContext;
        private readonly ScriptEngine _scriptEngine;

        public ImportService(SurvivalAnalysisContext survivalAnalysisContext)
        {
            _survivalAnalysisContext = survivalAnalysisContext;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            _scriptEngine = Python.CreateEngine();
        }

        public async Task ProcessCourseStatisticsExcelFile(IFormFile file)
        {
            using (var fileStream = file.OpenReadStream())
            {
                var data = ReadExcelFile(fileStream);
                await UploadCourseStatisticsData(data);
            }
        }

        public async Task ProcessSurvivalAnalysisExcelFile(IFormFile file)
        {
            using (var fileStream = file.OpenReadStream())
            {
                var data = ReadExcelFile(fileStream);
                await UploadSurvivalAnalysisData(data);
            }
        }

        public Task RunPythonScriptAsync(string path)
        {
            ScriptScope scope = _scriptEngine.CreateScope();
            _scriptEngine.ExecuteFile(path, scope);
            return Task.CompletedTask;
        }

        public async Task TruncateSurvivalPrediction()
        {
            await _survivalAnalysisContext.SurvivalPrediction.ExecuteDeleteAsync();
        }

        private DataTable ReadExcelFile(Stream fileStream)
        {
            using (var reader = ExcelReaderFactory.CreateReader(fileStream))
            {
                var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });

                return result.Tables[0];
            }
        }

        private async Task UploadCourseStatisticsData(DataTable data)
        {
            var students = new List<Student>();
            var teachers = new List<Teacher>();
            var subjects = new List<Subject>();
            var courses = new List<Course>();
            var studentOnCourses = new List<StudentOnCourse>();
            var teacherOnCourses = new List<TeacherOnCourse>();

            foreach (DataRow row in data.Rows)
            {
                var studentNeptunCode = row["Neptunkód"].ToString();
                var teacherName = row["Oktató név"].ToString();
                var subjectCode = row["Tárgykód"].ToString();
                var subjectName = row["Tárgynév"].ToString();
                var courseSemester = row["Félév"].ToString();
                var courseCode = row["Kurzuskód"].ToString();
                var courseType = row["Kurzustípus"].ToString();
                var courseProgram = row["Tagozat"].ToString();
                var courseLanguage = row["Nyelv"].ToString();
                var courseEnrollment = Int32.Parse(row["Létszám"].ToString());
                DateTime? studentOnCourseDateOfSignature = row["Aláírás dátuma"].ToString() != String.Empty ? DateTime.Parse(row["Aláírás dátuma"].ToString()) : null;
                DateTime? studentOnCourseDateOfSignatureRefusal = row["Aláírás megtagadás dátuma"].ToString() != String.Empty ? DateTime.Parse(row["Aláírás megtagadás dátuma"].ToString()) : null;
                var studentOnCourseCompleted = row["Teljesített"].ToString() == "Igaz" ? true : false;
                var teacherOnCourseProportion = Int32.Parse(row["Százalék"].ToString()) / 100.0;

                if (!students.Any(s => s.NeptunCode == studentNeptunCode))
                {
                    var student = new Student()
                    {
                        NeptunCode = studentNeptunCode
                    };

                    students.Add(student);
                }

                if (!teachers.Any(t => t.Name == teacherName))
                {
                    var teacher = new Teacher()
                    {
                        Name = teacherName
                    };

                    teachers.Add(teacher);
                }

                if (!subjects.Any(s => s.Code == subjectCode))
                {
                    var subject = new Subject()
                    {
                        Code = subjectCode,
                        Name = subjectName
                    };

                    subjects.Add(subject);
                }

                if (!courses.Any(c => c.Subject.Code == subjectCode && c.Code == courseCode))
                {
                    var subject = subjects.First(s => s.Code == subjectCode);

                    var course = new Course()
                    {
                        Semester = courseSemester,
                        Code = courseCode,
                        Type = courseType,
                        Program = courseProgram,
                        Language = courseLanguage,
                        Enrollment = courseEnrollment,
                        Subject = subject
                    };

                    courses.Add(course);
                }

                if (!studentOnCourses.Any(s =>
                    s.Student.NeptunCode == studentNeptunCode &&
                    s.Course.Code == courseCode &&
                    s.Course.Subject.Code == subjectCode))
                {
                    var student = students.First(s => s.NeptunCode == studentNeptunCode);
                    var course = courses.First(c => c.Subject.Code == subjectCode && c.Code == courseCode);

                    var studentOnCourse = new StudentOnCourse()
                    {
                        Student = student,
                        Course = course,
                        DateOfSignature = studentOnCourseDateOfSignature,
                        DateOfSignatureRefusal = studentOnCourseDateOfSignatureRefusal,
                        Completed = studentOnCourseCompleted
                    };

                    studentOnCourses.Add(studentOnCourse);
                }

                if (!teacherOnCourses.Any(t =>
                    t.Teacher.Name == teacherName &&
                    t.Course.Code == courseCode &&
                    t.Course.Subject.Code == subjectCode))
                {
                    var teacher = teachers.First(t => t.Name == teacherName);
                    var course = courses.First(c => c.Subject.Code == subjectCode && c.Code == courseCode);

                    var teacherOnCourse = new TeacherOnCourse()
                    {
                        Teacher = teacher,
                        Course = course,
                        Proportion = teacherOnCourseProportion
                    };

                    teacherOnCourses.Add(teacherOnCourse);
                }
            }

            _survivalAnalysisContext.TeachersOnCourses.RemoveRange(_survivalAnalysisContext.TeachersOnCourses);
            _survivalAnalysisContext.StudentsOnCourses.RemoveRange(_survivalAnalysisContext.StudentsOnCourses);
            _survivalAnalysisContext.Courses.RemoveRange(_survivalAnalysisContext.Courses);
            _survivalAnalysisContext.Subjects.RemoveRange(_survivalAnalysisContext.Subjects);
            _survivalAnalysisContext.Teachers.RemoveRange(_survivalAnalysisContext.Teachers);
            _survivalAnalysisContext.Students.RemoveRange(_survivalAnalysisContext.Students);

            foreach (var student in students)
            {
                _survivalAnalysisContext.Add(student);
            }

            foreach (var teacher in teachers)
            {
                _survivalAnalysisContext.Add(teacher);
            }

            foreach (var subject in subjects)
            {
                _survivalAnalysisContext.Add(subject);
            }

            foreach (var course in courses)
            {
                _survivalAnalysisContext.Add(course);
            }

            foreach (var studentOnCourse in studentOnCourses)
            {
                _survivalAnalysisContext.Add(studentOnCourse);
            }

            foreach (var teacherOnCourse in teacherOnCourses)
            {
                _survivalAnalysisContext.Add(teacherOnCourse);
            }

            await _survivalAnalysisContext.SaveChangesAsync();
        }

        private async Task UploadSurvivalAnalysisData(DataTable data)
        {
            var survivalAnalysisItems = new List<SurvivalAnalysisItem>();

            foreach (DataRow row in data.Rows)
            {
                string? neptunCode = string.IsNullOrEmpty(row["Neptun kód"].ToString()) ? null : row["Neptun kód"].ToString();
                string? moduleCode = string.IsNullOrEmpty(row["Modulkód"].ToString()) ? null : row["Modulkód"].ToString();
                string? admissionSemester = string.IsNullOrEmpty(row["Felvétel féléve"].ToString()) ? null : row["Felvétel féléve"].ToString();
                string? legalRelationshipEstablishmentReason = string.IsNullOrEmpty(row["Jogviszony létrejöttének oka"].ToString()) ? null : row["Jogviszony létrejöttének oka"].ToString();
                DateTime? legalRelationshipStartDate = string.IsNullOrEmpty(row["Képzés jogviszony kezdete"].ToString()) ? null : DateTime.Parse(row["Képzés jogviszony kezdete"].ToString());
                string? statusId = string.IsNullOrEmpty(row["Státusz ID"].ToString()) ? null : row["Státusz ID"].ToString();
                DateTime? legalRelationshipEndDate = string.IsNullOrEmpty(row["Képzés jogviszony vége"].ToString()) ? null : DateTime.Parse(row["Képzés jogviszony vége"].ToString());
                string? legalRelationshipTerminationReason = string.IsNullOrEmpty(row["Jogviszony megszűnés oka"].ToString()) ? null : row["Jogviszony megszűnés oka"].ToString();
                string? semester = string.IsNullOrEmpty(row["Félév"].ToString()) ? null : row["Félév"].ToString();
                string? subjectCode = string.IsNullOrEmpty(row["Tárgykód"].ToString()) ? null : row["Tárgykód"].ToString();
                string? subjectName = string.IsNullOrEmpty(row["Tárgynév"].ToString()) ? null : row["Tárgynév"].ToString();
                bool? completed = string.IsNullOrEmpty(row["Teljesített"].ToString()) ? null : row["Teljesített"].ToString() == "Igaz";
                string? enrollmentType = string.IsNullOrEmpty(row["Felvétel típusa"].ToString()) ? null : row["Felvétel típusa"].ToString();
                int? enrollmentCredit = string.IsNullOrEmpty(row["Tárgyfelvételkori kredit"].ToString()) ? null : Int32.Parse(row["Tárgyfelvételkori kredit"].ToString());
                int? subjectTakenCount = string.IsNullOrEmpty(row["Tárgyfelvétel hányszor"].ToString()) ? null : Int32.Parse(row["Tárgyfelvétel hányszor"].ToString());
                string? prerequisites = string.IsNullOrEmpty(row["Tárgykövetelmény"].ToString()) ? null : row["Tárgykövetelmény"].ToString();
                bool? recognized = string.IsNullOrEmpty(row["Elismert"].ToString()) ? null : row["Elismert"].ToString() == "Igaz";
                string? entryValue = string.IsNullOrEmpty(row["Bejegyzés értéke"].ToString()) ? null : row["Bejegyzés értéke"].ToString();
                string? entryType = string.IsNullOrEmpty(row["Bejegyzés típusa"].ToString()) ? null : row["Bejegyzés típusa"].ToString();
                bool? valid = string.IsNullOrEmpty(row["Érvényes"].ToString()) ? null : row["Érvényes"].ToString() == "Igaz";
                string? program = string.IsNullOrEmpty(row["Tagozat"].ToString()) ? null : row["Tagozat"].ToString();
                DateTime? diplomaObtainingDate = string.IsNullOrEmpty(row["Diploma megszerzés dátuma"].ToString()) ? null : DateTime.Parse(row["Diploma megszerzés dátuma"].ToString());
                int? admissionScoreTotal = string.IsNullOrEmpty(row["Felvételi összes pontszám"].ToString()) ? null : Int32.Parse(row["Felvételi összes pontszám"].ToString());
                string? admissionFinancialStatus = string.IsNullOrEmpty(row["Felvételkori pénzügyi státusz"].ToString()) ? null : row["Felvételkori pénzügyi státusz"].ToString();
                DateTime? languageExamFulfillmentDate = string.IsNullOrEmpty(row["Nyelvvizsga követelmény teljesülésének dátuma"].ToString()) ? null : DateTime.Parse(row["Nyelvvizsga követelmény teljesülésének dátuma"].ToString());

                var survivalAnalysisItem = new SurvivalAnalysisItem()
                {
                    NeptunCode = neptunCode,
                    ModuleCode = moduleCode,
                    AdmissionSemester = admissionSemester,
                    LegalRelationshipEstablishmentReason = legalRelationshipEstablishmentReason,
                    LegalRelationshipStartDate = legalRelationshipStartDate,
                    StatusId = statusId,
                    LegalRelationshipEndDate = legalRelationshipEndDate,
                    LegalRelationshipTerminationReason = legalRelationshipTerminationReason,
                    Semester = semester,
                    SubjectCode = subjectCode,
                    SubjectName = subjectName,
                    Completed = completed,
                    EnrollmentType = enrollmentType,
                    EnrollmentCredit = enrollmentCredit,
                    SubjectTakenCount = subjectTakenCount,
                    Prerequisites = prerequisites,
                    Recognized = recognized,
                    EntryValue = entryValue,
                    EntryType = entryType,
                    Valid = valid,
                    Program = program,
                    DiplomaObtainingDate = diplomaObtainingDate,
                    AdmissionScoreTotal = admissionScoreTotal,
                    AdmissionFinancialStatus = admissionFinancialStatus,
                    LanguageExamFulfillmentDate = languageExamFulfillmentDate
                };

                survivalAnalysisItems.Add(survivalAnalysisItem);
            }

            _survivalAnalysisContext.SurvivalAnalysisItems.RemoveRange(_survivalAnalysisContext.SurvivalAnalysisItems);

            foreach (var item in survivalAnalysisItems)
            {
                _survivalAnalysisContext.Add(item);
            }

            await _survivalAnalysisContext.SaveChangesAsync();
        }
    }
}
