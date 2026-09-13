using Microsoft.EntityFrameworkCore;
using UniversitySystem.Repository.DbConnection;
using UniversitySystem.Services.DTOs.Student.Semesters;
using UniversitySystem.Services.Enums;

namespace UniversitySystem.Services.Services.Student
{
    public class SemesterMarksService
    {
        private readonly UniAppDbContext _context;
        public SemesterMarksService(UniAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SemesterMarkDto>> GetStudentSemesterMarksAsync(Guid userId)
        {
            return await _context.Enrollments
                .AsNoTracking()
                .Where(e => e.Student.UserId == userId
                            && e.EnrollmentStatusId == (int)EnrollmentStatusEnum.Registered
                            && e.Section.Semester.IsActive)
                .Select(e => new SemesterMarkDto
                {
                    SectionId = e.SectionId,
                    CourseName = e.Section.Course.Name,
                    AssignmentMark = e.AssignmentMark,
                    MidExamMark = e.MidExamMark,
                    FinalExamMark = e.FinalExamMark,
                    Grade = e.Grade,
                    Absences = e.Attendances.Count(a => !a.IsPresent)
                })
                .ToListAsync();
        }
    }
}
