using Microsoft.EntityFrameworkCore;
using UniversitySystem.Repository.DbConnection;
using UniversitySystem.Repository.Models;
using UniversitySystem.Services.DTOs.Student.Enrollments;
using UniversitySystem.Services.DTOs.Student.Sections;
using UniversitySystem.Services.Enums;

namespace UniversitySystem.Services.Services.Student
{
    public class SectionRegistrationService
    {
        private readonly UniAppDbContext _context;

        public SectionRegistrationService(UniAppDbContext context)
        {
            _context = context;
        }

        public async Task<EnrollmentDto> RegisterStudentInSectionAsync(Guid userId, int sectionId)
        {
            using var transaction = await _context.Database
                .BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead);

            try
            {
                var student = await _context.Students
                    .FirstOrDefaultAsync(s => s.UserId == userId)
                    ?? throw new InvalidOperationException("Student not found");

                var section = await _context.Sections
                    .Include(s => s.Course)
                    .FirstOrDefaultAsync(s => s.Id == sectionId)
                    ?? throw new InvalidOperationException("Section not found");

                int currentCount = await _context.Enrollments
                    .CountAsync(e => e.SectionId == sectionId && e.EnrollmentStatusId == (int)EnrollmentStatusEnum.Registered);

                if (currentCount >= section.RoomCapacity)
                    throw new InvalidOperationException("The section is full");

                bool takenOrCompleted = await _context.Enrollments
                    .AnyAsync(e => e.StudentId == student.Id
                                && (e.EnrollmentStatusId == (int)EnrollmentStatusEnum.Registered || e.EnrollmentStatusId == (int)EnrollmentStatusEnum.Completed)
                                && e.Section.CourseId == section.CourseId);

                if (takenOrCompleted)
                    throw new InvalidOperationException("You have already taken or completed this course");

                bool timeSlotTaken = await _context.Enrollments
                    .AnyAsync(e => e.StudentId == student.Id
                                && e.EnrollmentStatusId == (int)EnrollmentStatusEnum.Registered
                                && e.Section.Schedule == section.Schedule);

                if (timeSlotTaken)
                    throw new InvalidOperationException("You are already registered for a section at the same time slot");

                int currentHours = await _context.Enrollments
                    .Where(e => e.StudentId == student.Id && e.EnrollmentStatusId == (int)EnrollmentStatusEnum.Registered)
                    .SumAsync(e => (int?)e.Section.Course.CreditHours) ?? 0;

                if (currentHours + section.Course.CreditHours > 18)
                    throw new InvalidOperationException("Adding this course exceeds your maximum limit of 18 credit hours");

                var enrollment = new Enrollment
                {
                    SectionId = sectionId,
                    StudentId = student.Id,
                    EnrollmentStatusId = (int)EnrollmentStatusEnum.Registered,
                    Grade = "F"
                };

                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new EnrollmentDto
                {
                    EnrollmentId = enrollment.Id,
                    SectionId = enrollment.SectionId,
                    StudentId = enrollment.StudentId,
                    ProfessorId = section.ProfessorId,
                    CreatedAt = enrollment.CreatedAt
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<AvailableSectionDto>> GetStudentAvailableSectionsAsync(Guid userId)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId)
                ?? throw new InvalidOperationException("Student not found");

            var registeredSectionIds = _context.Enrollments
                .Where(e => e.StudentId == student.Id && e.EnrollmentStatusId == (int)EnrollmentStatusEnum.Registered)
                .Select(e => e.SectionId);

            return await _context.Sections
                .Where(s => s.Semester.IsActive
                              && s.Course.MajorId == student.MajorId
                              && !registeredSectionIds.Contains(s.Id))
                .Select(s => new AvailableSectionDto
                {
                    SectionId = s.Id,
                    CourseName = s.Course.Name,
                    CreditHours = s.Course.CreditHours,
                    Schedule = s.Schedule,
                    RoomName = s.RoomName,
                    RoomCapacity = s.RoomCapacity,
                    SeatsTaken = s.Enrollments.Count(e => e.EnrollmentStatusId == (int)EnrollmentStatusEnum.Registered),
                    SeatsAvailable = s.RoomCapacity - s.Enrollments.Count(e => e.EnrollmentStatusId == (int)EnrollmentStatusEnum.Registered),
                    ProfessorName = s.Professor.User.Name
                })
                .OrderBy(s => s.CourseName)
                .ToListAsync();
        }
    }
}
