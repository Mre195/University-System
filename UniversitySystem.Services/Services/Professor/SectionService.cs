using Microsoft.EntityFrameworkCore;
using UniversitySystem.Repository.DbConnection;
using UniversitySystem.Services.DTOs.Shared.Sections;
using UniversitySystem.Services.Enums;
using UniversitySystem.Services.Services.Shared;

namespace UniversitySystem.Services.Services.Professor
{
    public class SectionService
    {
        private readonly UniAppDbContext _context;
        private readonly AutoUpdateSemesterActiveStatus _autoUpdateSemesterActiveStatus;
        public SectionService(UniAppDbContext context, AutoUpdateSemesterActiveStatus autoUpdateSemesterActiveStatus)
        {
            _context = context;
            _autoUpdateSemesterActiveStatus = autoUpdateSemesterActiveStatus;
        }

        public async Task<List<SectionSummaryDto>> GetProfessorSectionsAsync(Guid userId)
        {
            await _autoUpdateSemesterActiveStatus.AutoUpdateSemesterActiveStatusAsync();
            return await _context.Sections
                .AsNoTracking()
                .Where(s => s.Professor.UserId == userId && s.Semester.IsActive)
                .Select(s => new SectionSummaryDto
                {
                    SectionId =     s.Id,
                    CourseName =    s.Course.Name,
                    CreditHours =   s.Course.CreditHours,
                    Schedule =      s.Schedule,
                    RoomName =      s.RoomName,
                    SemesterName =  s.Semester.Name,
                    EnrolledCount = s.Enrollments.Count(x => x.EnrollmentStatusId == (int)EnrollmentStatusEnum.Registered),
                })
                .OrderBy(x => x.SectionId)
                .ToListAsync();
        }
    }
}
