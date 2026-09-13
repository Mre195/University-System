using Microsoft.EntityFrameworkCore;
using UniversitySystem.Repository.DbConnection;
using UniversitySystem.Services.DTOs.Shared.Sections;
using UniversitySystem.Services.Enums;
using UniversitySystem.Services.Services.Shared;

namespace UniversitySystem.Services.Services.Student
{
    public class ScheduleService
    {
        private readonly UniAppDbContext _context;
        private readonly AutoUpdateSemesterActiveStatus _autoUpdateSemesterActiveStatus;
        public ScheduleService(UniAppDbContext context, AutoUpdateSemesterActiveStatus semesterChecker)
        {
            _context = context;
            _autoUpdateSemesterActiveStatus = semesterChecker;
        }

        public async Task<IEnumerable<SectionSummaryDto>> GetStudentScheduleAsync(Guid userId)
        {
                await _autoUpdateSemesterActiveStatus.AutoUpdateSemesterActiveStatusAsync();
                return
                    await _context.Enrollments
                        .AsNoTracking()
                        .Where(e => e.Student.UserId == userId
                                    && e.EnrollmentStatusId == (int)EnrollmentStatusEnum.Registered
                                    && e.Section.Semester.IsActive)
                        .Select(e => new SectionSummaryDto
                        {
                            SectionId =     e.SectionId,
                            CourseName =    e.Section.Course.Name,
                            CreditHours =   e.Section.Course.CreditHours,
                            Schedule =      e.Section.Schedule,
                            RoomName =      e.Section.RoomName,
                            ProfessorName = e.Section.Professor.User.Name,
                            SemesterName =  e.Section.Semester.Name,
                            ScheduleDate =  e.CreatedAt
                        })
                        .OrderBy(x => x.ScheduleDate)
                        .ToListAsync();
        }
    }
}
