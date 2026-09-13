using Microsoft.EntityFrameworkCore;
using UniversitySystem.Repository.DbConnection;
using UniversitySystem.Repository.Models;
using UniversitySystem.Services.DTOs.Admin.Majors;

namespace UniversitySystem.Services.Services.Admin
{
    public class MajorService
    {
        private readonly UniAppDbContext _context;
        public MajorService(UniAppDbContext context)
        {
            _context = context;
        }

        public async Task<MajorDto> CreateMajorAsync(CreateMajorDto dto)
        {
            var departmentExists = await _context.Departments.FindAsync(dto.DepartmentId)
                ?? throw new Exception("Department not found");

            if (dto.Hours is < 120 or > 263)
                throw new Exception("Hours must be between 120 and 263");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new Exception("Major name is required");

            var nameExists = await _context.Majors
                .AnyAsync(m => m.Name.ToLower() == dto.Name.Trim().ToLower());
            if (nameExists)
                throw new InvalidOperationException($"A major named '{dto.Name}' already exists.");

            var major = new Major
            {
                DepartmentId = dto.DepartmentId,
                Name = dto.Name,
                Hours = dto.Hours
            };

            _context.Majors.Add(major);
            await _context.SaveChangesAsync();

            return new MajorDto
            {
                Id = major.Id,
                Name = major.Name,
                Hours = major.Hours,
                DepartmentId = major.DepartmentId,
                DepartmentName = departmentExists.Name
            };
        }

        public async Task<IEnumerable<MajorDto>> GetAllMajorsAsync(int departmentId)
        {
            return await _context.Majors
                .AsNoTracking()
                .Where(m => m.DepartmentId == departmentId)
                .Select(m => new MajorDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Hours = m.Hours,
                    DepartmentId = m.DepartmentId,
                    DepartmentName = m.Department.Name
                })
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        // Update and Delete are commented out as per original
    }
}
