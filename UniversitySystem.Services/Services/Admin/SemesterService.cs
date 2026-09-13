using Microsoft.EntityFrameworkCore;
using UniversitySystem.Repository.DbConnection;
using UniversitySystem.Repository.Models;
using UniversitySystem.Services.DTOs.Admin.Semesters;

namespace UniversitySystem.Services.Services.Admin
{
    public class SemesterService
    {
        private readonly UniAppDbContext _context;
        public SemesterService(UniAppDbContext context)
        {
            _context = context;
        }

        public async Task<SemesterDto> CreateSemesterAsync(CreateSemesterDto dto)
        {
            if (dto.EndDate <= dto.StartDate)
                throw new Exception("End date must be after start date");

            if (dto.IsActive)
            {
                var currentlyActive = await _context.Semesters
                    .Where(s => s.IsActive)
                    .ToListAsync();
                foreach (var s in currentlyActive)
                    s.IsActive = false;
            }

            var semester = new Semester
            {
                Name = dto.Name,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsActive = dto.IsActive
            };
            _context.Semesters.Add(semester);
            await _context.SaveChangesAsync();

            return new SemesterDto
            {
                Id = semester.Id,
                Name = semester.Name,
                StartDate = semester.StartDate,
                EndDate = semester.EndDate,
                IsActive = semester.IsActive
            };
        }

        public async Task<string> SetActiveSemesterAsync(int semesterId)
        {
            var target = await _context.Semesters.FirstOrDefaultAsync(s => s.Id == semesterId);
            if (target == null)
                throw new Exception("Semester not found");

            var currentlyActive = await _context.Semesters
                .Where(s => s.IsActive && s.Id != semesterId)
                .ToListAsync();
            foreach (var s in currentlyActive)
                s.IsActive = false;

            target.IsActive = true;
            await _context.SaveChangesAsync();

            return "Semester " + target.Name + " set as " + target.IsActive;
        }

        public async Task<IEnumerable<SemesterDto>> GetAllSemestersAsync()
        {
            return await _context.Semesters
                .AsNoTracking()
                .OrderByDescending(s => s.StartDate)
                .Select(s => new SemesterDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    IsActive = s.IsActive
                })
                .ToListAsync();
        }
    }
}
