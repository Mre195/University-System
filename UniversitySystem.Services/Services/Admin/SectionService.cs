using Microsoft.EntityFrameworkCore;
using UniversitySystem.Repository.DbConnection;
using UniversitySystem.Repository.Models;
using UniversitySystem.Services.DTOs.Admin.Sections;
using UniversitySystem.Services.Enums;
using UniversitySystem.Services.Services.Shared;

namespace UniversitySystem.Services.Services.Admin
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

        public async Task<Section> CreateSectionAsync(CreateSectionDto dto)
        {
            bool courseExists = await _context.Courses.AnyAsync(c => c.Id == dto.CourseId);
            if (!courseExists)
                throw new Exception("Course not found");

            var professor = await _context.Professors.FirstOrDefaultAsync(p => p.Id == dto.ProfessorId);
            if (professor == null)
                throw new Exception("Professor not found");
            if (!professor.IsActive)
                throw new Exception("Professor is not active");

            bool semesterExists = await _context.Semesters.AnyAsync(s => s.Id == dto.SemesterId);
            if (!semesterExists)
                throw new Exception("Semester not found");

            if (dto.RoomCapacity != 30 && dto.RoomCapacity != 60)
                throw new Exception("Room capacity must be 30 or 60");

            bool professorClash = await _context.Sections
                .AnyAsync(s => s.ProfessorId == dto.ProfessorId
                            && s.SemesterId == dto.SemesterId
                            && s.Schedule == dto.Schedule);
            if (professorClash)
                throw new Exception("This professor already has a section at that exact time slot");

            var section = new Section
            {
                CourseId = dto.CourseId,
                ProfessorId = dto.ProfessorId,
                SemesterId = dto.SemesterId,
                Schedule = dto.Schedule,
                RoomName = dto.RoomName,
                RoomCapacity = dto.RoomCapacity
            };

            _context.Sections.Add(section);
            await _context.SaveChangesAsync();

            return section;
        }

        public async Task<string> UpdateSectionAsync(int sectionId, UpdateSectionDto dto)
        {
            var section = await _context.Sections.FirstOrDefaultAsync(s => s.Id == sectionId)
                ?? throw new Exception("Section not found");

            int targetProfessorId = dto.ProfessorId ?? section.ProfessorId;
            string targetSchedule = !string.IsNullOrWhiteSpace(dto.Schedule) ? dto.Schedule : section.Schedule;

            if (dto.ProfessorId.HasValue)
            {
                var professor = await _context.Professors.FirstOrDefaultAsync(p => p.Id == dto.ProfessorId.Value);
                if (professor == null)
                    return "Professor not found";
                if (!professor.IsActive)
                    return "Professor is not active";
            }

            if (dto.RoomCapacity.HasValue)
            {
                if (dto.RoomCapacity.Value != 30 && dto.RoomCapacity.Value != 60)
                    return "Room capacity must be 30 or 60";
                section.RoomCapacity = dto.RoomCapacity.Value;
            }

            bool professorClash = await _context.Sections
                .AnyAsync(s => s.Id != sectionId
                            && s.ProfessorId == targetProfessorId
                            && s.SemesterId == section.SemesterId
                            && s.Schedule == targetSchedule);

            if (professorClash)
                return "This professor already has a section at that exact time slot";

            if (dto.ProfessorId.HasValue)
                section.ProfessorId = dto.ProfessorId.Value;

            if (!string.IsNullOrWhiteSpace(dto.Schedule))
                section.Schedule = dto.Schedule;

            if (!string.IsNullOrWhiteSpace(dto.RoomName))
                section.RoomName = dto.RoomName;

            await _context.SaveChangesAsync();
            return "Section updated";
        }

        public async Task<string> DeleteSectionAsync(int sectionId)
        {
            var section = await _context.Sections.FirstOrDefaultAsync(s => s.Id == sectionId);
            if (section == null)
                return "Section not found";

            bool hasEnrollments = await _context.Enrollments.AnyAsync(e => e.SectionId == sectionId);
            if (hasEnrollments)
                return "Cannot delete: students are enrolled in this section";

            _context.Sections.Remove(section);
            await _context.SaveChangesAsync();
            return "Section deleted";
        }

        public async Task<IEnumerable<SectionAdminDto>> GetSectionsAsync(int semesterId, int courseId)
        {
            await _autoUpdateSemesterActiveStatus.AutoUpdateSemesterActiveStatusAsync();
            return await 
                _context.Sections.AsNoTracking()
                .Select(s => new SectionAdminDto
                {
                    Id = s.Id,
                    CourseName = s.Course.Name,
                    ProfessorName = s.Professor.User.Name,
                    SemesterName = s.Semester.Name,
                    Schedule = s.Schedule,
                    RoomName = s.RoomName,
                    RoomCapacity = s.RoomCapacity,
                    EnrolledCount = s.Enrollments.Count(e => e.EnrollmentStatusId == (int)EnrollmentStatusEnum.Registered)
                })
                .OrderBy(s => s.CourseName)
                .ToListAsync();
        }
    }
}
