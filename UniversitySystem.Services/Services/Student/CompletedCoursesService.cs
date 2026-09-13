using Microsoft.EntityFrameworkCore;
using UniversitySystem.Repository.DbConnection;
using UniversitySystem.Services.DTOs.Student.Courses;

namespace UniversitySystem.Services.Services.Student
{
    public class CompletedCoursesService
    {
        private readonly UniAppDbContext _context;
        public CompletedCoursesService(UniAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CompletedCourseDto>> StudentCompletedCoursesAsync(Guid userId)
        {
            return await _context.Enrollments
                .AsNoTracking()
                .Where(e => e.Student.UserId == userId && e.EnrollmentStatusId == 4) // Completed
                .Select(e => new CompletedCourseDto
                {
                    StudentId = e.StudentId,
                    CourseName = e.Section.Course.Name,
                    CreditHours = e.Section.Course.CreditHours,
                    TotalMarks = e.AssignmentMark + e.MidExamMark + e.FinalExamMark,
                })
                .OrderBy(x => x.TotalMarks)
                .ToListAsync();
        }
    }
}
