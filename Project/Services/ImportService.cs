using ExcelDataReader;
using Project.DbContexts;
using Project.Entities.CourseStatistics;
using System.Data;

namespace Project.Services
{
    public class ImportService : IImportService
    {
        private readonly CourseStatisticsContext _courseStatisticsContext;

        public ImportService(CourseStatisticsContext courseStatisticsContext)
        {
            _courseStatisticsContext = courseStatisticsContext;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public async Task ProcessCourseStatisticsExcelFile(IFormFile file)
        {
            using (var fileStream = file.OpenReadStream())
            {
                var data = ReadExcelFile(fileStream);
                await UploadData(data);
            }
        }

        public async Task ProcessSurvivalAnalysisExcelFile(IFormFile file)
        {
            throw new NotImplementedException();
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

        private async Task UploadData(DataTable data)
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

            _courseStatisticsContext.TeachersOnCourses.RemoveRange(_courseStatisticsContext.TeachersOnCourses);
            _courseStatisticsContext.StudentsOnCourses.RemoveRange(_courseStatisticsContext.StudentsOnCourses);
            _courseStatisticsContext.Courses.RemoveRange(_courseStatisticsContext.Courses);
            _courseStatisticsContext.Subjects.RemoveRange(_courseStatisticsContext.Subjects);
            _courseStatisticsContext.Teachers.RemoveRange(_courseStatisticsContext.Teachers);
            _courseStatisticsContext.Students.RemoveRange(_courseStatisticsContext.Students);

            foreach (var student in students)
            {
                _courseStatisticsContext.Add(student);
            }

            foreach (var teacher in teachers)
            {
                _courseStatisticsContext.Add(teacher);
            }

            foreach (var subject in subjects)
            {
                _courseStatisticsContext.Add(subject);
            }

            foreach (var course in courses)
            {
                _courseStatisticsContext.Add(course);
            }

            foreach (var studentOnCourse in studentOnCourses)
            {
                _courseStatisticsContext.Add(studentOnCourse);
            }

            foreach (var teacherOnCourse in teacherOnCourses)
            {
                _courseStatisticsContext.Add(teacherOnCourse);
            }

            _courseStatisticsContext.SaveChanges();
        }
    }
}
