using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project.DataTransferObjects;
using Project.DbContexts;
using Project.Entities;

namespace Project.Services
{
    public class SurvivalAnalysisService : ISurvivalAnalysisService
    {
        private readonly SurvivalAnalysisContext _survivalAnalysisContext;

        public SurvivalAnalysisService(SurvivalAnalysisContext survivalAnalysisContext)
        {
            _survivalAnalysisContext = survivalAnalysisContext;
        }

        public async Task<IEnumerable<SurvivalAnalysisDto>> GetSurvivalAnalysisStatistics(List<string?> neptunCodes)
        {
            IQueryable<SurvivalAnalysisDto> query;

            if (neptunCodes.IsNullOrEmpty())
            {
                query = from s in _survivalAnalysisContext.SurvivalAnalysisItems
                        where (s.EntryValue == "Elégséges" ||
                               s.EntryValue == "Közepes" ||
                               s.EntryValue == "Jó" ||
                               s.EntryValue == "Jól megfelelt" ||
                               s.EntryValue == "Jeles" ||
                               s.EntryValue == "Kiválóan megfelelt (5)")
                              && s.EnrollmentCredit != 0
                        group s by new { s.NeptunCode, s.AdmissionSemester, s.Semester, s.SubjectCode } into g
                        select new
                        {
                            g.Key.NeptunCode,
                            g.Key.AdmissionSemester,
                            g.Key.Semester,
                            Credit = g.Max(x => x.EnrollmentCredit),
                            Grade = g.Max(x =>
                            x.EntryValue == "Elégséges" ? 2 :
                            x.EntryValue == "Közepes" ? 3 :
                            x.EntryValue == "Jó" ? 4 :
                            x.EntryValue == "Jól megfelelt" ? 4 : 5)
                        } into tbl
                        group tbl by new { tbl.NeptunCode, tbl.AdmissionSemester } into g
                        orderby g.Key.NeptunCode
                        select new SurvivalAnalysisDto
                        {
                            NeptunCode = g.Key.NeptunCode,
                            AdmissionSemester = g.Key.AdmissionSemester,
                            SemesterCount = g.Select(x => x.Semester).Distinct().Count(),
                            Credit = g.Sum(x => x.Credit),
                            WeightedGradePointAverage = g.Sum(x => x.Credit * 1.0 * x.Grade) / g.Sum(x => x.Credit)
                        };
            }
            else
            {
                query = from s in _survivalAnalysisContext.SurvivalAnalysisItems
                        where (s.EntryValue == "Elégséges" ||
                               s.EntryValue == "Közepes" ||
                               s.EntryValue == "Jó" ||
                               s.EntryValue == "Jól megfelelt" ||
                               s.EntryValue == "Jeles" ||
                               s.EntryValue == "Kiválóan megfelelt (5)")
                              && s.EnrollmentCredit != 0
                              && neptunCodes.Contains(s.NeptunCode)
                        group s by new { s.NeptunCode, s.AdmissionSemester, s.Semester, s.SubjectCode } into g
                        select new
                        {
                            g.Key.NeptunCode,
                            g.Key.AdmissionSemester,
                            g.Key.Semester,
                            Credit = g.Max(x => x.EnrollmentCredit),
                            Grade = g.Max(x =>
                            x.EntryValue == "Elégséges" ? 2 :
                            x.EntryValue == "Közepes" ? 3 :
                            x.EntryValue == "Jó" ? 4 :
                            x.EntryValue == "Jól megfelelt" ? 4 : 5)
                        } into tbl
                        group tbl by new { tbl.NeptunCode, tbl.AdmissionSemester } into g
                        orderby g.Key.NeptunCode
                        select new SurvivalAnalysisDto
                        {
                            NeptunCode = g.Key.NeptunCode,
                            AdmissionSemester = g.Key.AdmissionSemester,
                            SemesterCount = g.Select(x => x.Semester).Distinct().Count(),
                            Credit = g.Sum(x => x.Credit),
                            WeightedGradePointAverage = g.Sum(x => x.Credit * 1.0 * x.Grade) / g.Sum(x => x.Credit)
                        };
            }

            var result = await query.ToListAsync();

            return result;
        }

        public async Task<IEnumerable<SurvivalPrediction>> GetSurvivalAnalysisPrediction(List<string?> neptunCodes)
        {
            IQueryable<SurvivalPrediction> query;


            if (neptunCodes.IsNullOrEmpty())
            {
                query = from s in _survivalAnalysisContext.SurvivalPrediction
                        select s;
            } else
            {
                query = from s in _survivalAnalysisContext.SurvivalPrediction
                        where neptunCodes.Contains(s.NeptunCode)
                        select s;
            }

            var result = await query.ToListAsync();

            return result;
        }

        public async Task<IEnumerable<SurvivalPrediction>> GetSurvivalAnalysisPredictionAsc(List<string?> neptunCodes)
        {
            IQueryable<SurvivalPrediction> query;


            if (neptunCodes.IsNullOrEmpty())
            {
                query = from s in _survivalAnalysisContext.SurvivalPrediction
                        orderby s.risk_score ascending
                        select s;
            }
            else
            {
                query = from s in _survivalAnalysisContext.SurvivalPrediction
                        where neptunCodes.Contains(s.NeptunCode)
                        orderby s.risk_score ascending
                        select s;
            }

            var result = await query.ToListAsync();

            return result;
        }

        public async Task<IEnumerable<SurvivalPrediction>> GetSurvivalAnalysisPredictionDesc(List<string?> neptunCodes)
        {
            IQueryable<SurvivalPrediction> query;


            if (neptunCodes.IsNullOrEmpty())
            {
                query = from s in _survivalAnalysisContext.SurvivalPrediction
                        orderby s.risk_score descending
                        select s;
            }
            else
            {
                query = from s in _survivalAnalysisContext.SurvivalPrediction
                        where neptunCodes.Contains(s.NeptunCode)
                        orderby s.risk_score descending
                        select s;
            }

            var result = await query.ToListAsync();

            return result;
        }

        public async Task<IEnumerable<CourseStatisticsDto>> GetCourseStatistics(
            List<string> semesterNames,
            List<string> subjectCodes,
            List<string> subjectNames,
            List<string> teacherNames)
        {
            IQueryable<CourseStatisticsDto> query;

            if (semesterNames.IsNullOrEmpty()
                && subjectCodes.IsNullOrEmpty()
                && subjectNames.IsNullOrEmpty()
                && teacherNames.IsNullOrEmpty())
            {
                query = from Course in _survivalAnalysisContext.Courses
                        join Subject in _survivalAnalysisContext.Subjects on Course.SubjectId equals Subject.Id
                        join TeacherOnCourse in _survivalAnalysisContext.TeachersOnCourses on Course.Id equals TeacherOnCourse.CourseId
                        join StudentOnCourse in _survivalAnalysisContext.StudentsOnCourses on Course.Id equals StudentOnCourse.CourseId
                        join Teacher in _survivalAnalysisContext.Teachers on TeacherOnCourse.TeacherId equals Teacher.Id
                        group new { Teacher, TeacherOnCourse, Subject, Course, StudentOnCourse } by new { TeacherId = Teacher.Id, SubjectId = Subject.Id, CourseId = Course.Id } into grp
                        select new CourseStatisticsDto
                        {
                            TeacherName = grp.First().Teacher.Name,
                            TeacherProportion = grp.First().TeacherOnCourse.Proportion,
                            CourseCode = grp.First().Course.Code,
                            CourseSemester = grp.First().Course.Semester,
                            CourseType = grp.First().Course.Type,
                            CourseProgram = grp.First().Course.Program,
                            CourseLanguage = grp.First().Course.Language,
                            SubjectName = grp.First().Subject.Name,
                            SubjectCode = grp.First().Subject.Code,
                            SignaturePerEnrollment = 1.0 * grp.Count(s => s.StudentOnCourse.DateOfSignature != null) / grp.Count(),
                            CompletedPerSignature = grp.Count(s => s.StudentOnCourse.DateOfSignature != null) == 0 ? null : 1.0 * grp.Count(s => s.StudentOnCourse.Completed == true) / grp.Count(s => s.StudentOnCourse.DateOfSignature != null),
                            CompletedPerEnrollment = 1.0 * grp.Count(s => s.StudentOnCourse.Completed == true) / grp.Count(),
                            NumberOfEnrollment = grp.Count(),
                            NumberOfSignature = grp.Count(s => s.StudentOnCourse.DateOfSignature != null),
                            NumberOfSignatureRefusal = grp.Count(s => s.StudentOnCourse.DateOfSignatureRefusal != null),
                            NumberOfCompleted = grp.Count(s => s.StudentOnCourse.Completed == true)
                        };
            }
            else
            {
                query = from Course in _survivalAnalysisContext.Courses
                        join Subject in _survivalAnalysisContext.Subjects on Course.SubjectId equals Subject.Id
                        join TeacherOnCourse in _survivalAnalysisContext.TeachersOnCourses on Course.Id equals TeacherOnCourse.CourseId
                        join StudentOnCourse in _survivalAnalysisContext.StudentsOnCourses on Course.Id equals StudentOnCourse.CourseId
                        join Teacher in _survivalAnalysisContext.Teachers on TeacherOnCourse.TeacherId equals Teacher.Id
                        where semesterNames.Contains(Course.Semester) && subjectCodes.Contains(Subject.Code) && subjectNames.Contains(Subject.Name) && teacherNames.Contains(Teacher.Name)
                        group new { Teacher, TeacherOnCourse, Subject, Course, StudentOnCourse } by new { TeacherId = Teacher.Id, SubjectId = Subject.Id, CourseId = Course.Id } into grp
                        select new CourseStatisticsDto
                        {
                            TeacherName = grp.First().Teacher.Name,
                            TeacherProportion = grp.First().TeacherOnCourse.Proportion,
                            CourseCode = grp.First().Course.Code,
                            CourseSemester = grp.First().Course.Semester,
                            CourseType = grp.First().Course.Type,
                            CourseProgram = grp.First().Course.Program,
                            CourseLanguage = grp.First().Course.Language,
                            SubjectName = grp.First().Subject.Name,
                            SubjectCode = grp.First().Subject.Code,
                            SignaturePerEnrollment = 1.0 * grp.Count(s => s.StudentOnCourse.DateOfSignature != null) / grp.Count(),
                            CompletedPerSignature = grp.Count(s => s.StudentOnCourse.DateOfSignature != null) == 0 ? null : 1.0 * grp.Count(s => s.StudentOnCourse.Completed == true) / grp.Count(s => s.StudentOnCourse.DateOfSignature != null),
                            CompletedPerEnrollment = 1.0 * grp.Count(s => s.StudentOnCourse.Completed == true) / grp.Count(),
                            NumberOfEnrollment = grp.Count(),
                            NumberOfSignature = grp.Count(s => s.StudentOnCourse.DateOfSignature != null),
                            NumberOfSignatureRefusal = grp.Count(s => s.StudentOnCourse.DateOfSignatureRefusal != null),
                            NumberOfCompleted = grp.Count(s => s.StudentOnCourse.Completed == true)
                        };
            }

            var result = await query.ToListAsync();

            return result;
        }

        public async Task<CourseStatisticsFilterDto> GetCourseStatisticsFilters()
        {
            var semesterNames = await _survivalAnalysisContext.Courses.Select(c => c.Semester).Distinct().ToListAsync();
            var subjectCodes = await _survivalAnalysisContext.Subjects.Select(s => s.Code).Distinct().ToListAsync();
            var subjectNames = await _survivalAnalysisContext.Subjects.Select(s => s.Name).Distinct().ToListAsync();
            var teacherNames = await _survivalAnalysisContext.Teachers.Select(t => t.Name).Distinct().ToListAsync();

            var courseStatisticsFilterDto = new CourseStatisticsFilterDto()
            {
                SemesterNames = semesterNames,
                SubjectCodes = subjectCodes,
                SubjectNames = subjectNames,
                TeacherNames = teacherNames
            };

            return courseStatisticsFilterDto;
        }

        public async Task<SurvivalAnalysisFilterDto> GetSurvivalAnalysisFilters()
        {
            var neptunCodes = await _survivalAnalysisContext.SurvivalAnalysisItems.Select(i => i.NeptunCode).Distinct().ToListAsync();

            var survivalAnalysisFilterDto = new SurvivalAnalysisFilterDto()
            {
                NeptunCodes = neptunCodes
            };

            return survivalAnalysisFilterDto;
        }
    }
}
