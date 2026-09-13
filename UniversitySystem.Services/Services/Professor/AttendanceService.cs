using Microsoft.EntityFrameworkCore;
using UniversitySystem.Repository.DbConnection;
using UniversitySystem.Repository.Models;
using UniversitySystem.Services.DTOs.Professor.Attendances;
using UniversitySystem.Services.Enums;

namespace UniversitySystem.Services.Services.Professor
{
    public class AttendanceService
    {
        private readonly UniAppDbContext _context;
        public AttendanceService(UniAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AttendanceRecordDto>> GetStudentsAttendanceForProfessorAsync(Guid userId, int sectionId)
        {
            return await _context.Enrollments
                .AsNoTracking()
                .Where(e => e.Section.Professor.UserId == userId && e.SectionId == sectionId && e.EnrollmentStatusId == (int)EnrollmentStatusEnum.Registered)
                .Select(e => new AttendanceRecordDto
                {
                    EnrollmentId = e.Id,
                    StudentId = e.StudentId,
                    StudentName = e.Student.User.Name,
                    Record = e.Attendances
                          .OrderBy(a => a.Date)
                          .Select(a => new AttendanceEntryDto { Date = a.Date, IsPresent = a.IsPresent })
                          .ToList()
                })
                .ToListAsync();
        }

        public async Task<string> UpdateStudentsAttendanceForProfessorAsync(Guid userId, int sectionId, List<UpdateAttendanceDto> dto)
        {
            var section = await _context.Sections
                .FirstOrDefaultAsync(s => s.Id == sectionId && s.Professor.UserId == userId)
                ?? throw new InvalidOperationException("Section Id or Professor Id is not valid.");

            var enrollmentIds = dto.Select(d => d.EnrollmentId).ToList();
            DateTime today = DateTime.UtcNow.Date;
            DateTime tomorrow = today.AddDays(1);

            var existingRecords = await _context.Attendances
                .AsNoTracking()
                .Where(a => enrollmentIds.Contains(a.EnrollmentId) && a.Date >= today && a.Date < tomorrow)
                .ToListAsync();

            foreach (var x in dto)
            {
                var record = existingRecords.FirstOrDefault(a => a.EnrollmentId == x.EnrollmentId);

                if (record != null)
                    record.IsPresent = x.IsPresent;
                else
                {
                    _context.Attendances.Add(new Attendance
                    {
                        EnrollmentId = x.EnrollmentId,
                        IsPresent = x.IsPresent,
                        Date = x.Date ?? today
                    });
                }
            }
            await _context.SaveChangesAsync();
            return "Attendance updated successfully.";
        }
    }
}
