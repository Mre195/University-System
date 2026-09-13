using Microsoft.EntityFrameworkCore;
using UniversitySystem.Repository.DbConnection;
using UniversitySystem.Services.DTOs.Professor.Marks;

namespace UniversitySystem.Services.Services.Professor
{
    public class UpdateMarksService
    {
        private readonly UniAppDbContext _context;
        public UpdateMarksService(UniAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MarkRowDto>> GetSectionMarksForProfessorAsync(Guid userId, int sectionId)
        {
            return await _context.Enrollments
                .AsNoTracking()
                .Where(e => e.Section.Professor.UserId == userId
                            && e.SectionId == sectionId
                            && e.EnrollmentStatusId == 1)
                .Select(e => new MarkRowDto
                {
                    EnrollmentId = e.Id,
                    StudentId = e.StudentId,
                    StudentName = e.Student.User.Name,
                    AssignmentMark = e.AssignmentMark,
                    FinalExamMark = e.FinalExamMark,
                    MidExamMark = e.MidExamMark,
                    LetterGrade = e.Grade,
                })
                .ToListAsync();
        }

        public async Task<MarkRowDto> UpdateSectionMarksAsync(Guid userId, UpdateMarksDto dto)
        {
            var enrollment = await _context.Enrollments
                .AsNoTracking()
                .Include(e => e.Section)
                .Include(e => e.Student)
                .ThenInclude(s => s.User)
                .FirstOrDefaultAsync(e => e.Id == dto.EnrollmentId);

            if (enrollment == null)
                throw new InvalidOperationException("Enrollment not found");

            if (enrollment.Section.Professor.UserId != userId)
                throw new InvalidOperationException("Professor you are not registered in this course");

            int total = dto.AssignmentMark + dto.MidExamMark + dto.FinalExamMark;

            enrollment.AssignmentMark = dto.AssignmentMark;
            enrollment.MidExamMark = dto.MidExamMark;
            enrollment.FinalExamMark = dto.FinalExamMark;
            enrollment.Grade = GetLetter(total);

            await _context.SaveChangesAsync();
            return new MarkRowDto
            {
                EnrollmentId = enrollment.Id,
                StudentId = enrollment.StudentId,
                StudentName = enrollment.Student.User.Name,
                AssignmentMark = enrollment.AssignmentMark,
                MidExamMark = enrollment.MidExamMark,
                FinalExamMark = enrollment.FinalExamMark,
                LetterGrade = enrollment.Grade
            };
        }

        private string GetLetter(int total) => total switch
        {
            >= 90 => "A",
            >= 80 => "B",
            >= 70 => "C",
            >= 60 => "D",
            _ => "F"
        };
    }
}
