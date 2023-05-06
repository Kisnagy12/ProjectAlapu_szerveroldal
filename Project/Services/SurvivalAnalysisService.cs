using Microsoft.EntityFrameworkCore;
using Project.DataTransferObjects;
using Project.DbContexts;

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
    }
}
