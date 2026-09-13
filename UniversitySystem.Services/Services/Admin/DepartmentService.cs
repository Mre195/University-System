using Microsoft.EntityFrameworkCore;
using UniversitySystem.Repository.DbConnection;
using UniversitySystem.Repository.Models;
using UniversitySystem.Services.DTOs.Admin.Departments;

namespace UniversitySystem.Services.Services.Admin
{
    public class DepartmentService
    {
        private readonly UniAppDbContext _context;
        public DepartmentService(UniAppDbContext context)
        {
            _context = context;
        }

        public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Department name is required.", nameof(dto.Name));

            var nameExists = await _context.Departments
                .SingleOrDefaultAsync(d => d.Name.ToLower() == dto.Name.Trim().ToLower());
            if (nameExists != null)
                throw new InvalidOperationException($"A department named '{dto.Name}' already exists.");

            if (dto.HeadId.HasValue)
            {
                bool professorExists = await _context.Professors.AnyAsync(p => p.Id == dto.HeadId.Value);
                if (!professorExists)
                    throw new KeyNotFoundException($"Professor with ID {dto.HeadId.Value} was not found.");
            }

            var department = new Department
            {
                Name = dto.Name.Trim(),
                HeadId = dto.HeadId
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                HeadId = department.HeadId
            };
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
        {
            return await _context.Departments
                .AsNoTracking()
                .Select(d => new DepartmentDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    HeadId = d.HeadId,
                    HeadName = d.HeadId != null ? d.Head!.User.Name : null
                })
                .OrderBy(d => d.Name)
                .ToListAsync();
        }

        // Update and Delete are commented out as per original
    }
}
