using Microsoft.EntityFrameworkCore;
using UniversitySystem.Repository.DbConnection;
using UniversitySystem.Repository.Models;
using UniversitySystem.Services.DTOs.Admin.Professors;

namespace UniversitySystem.Services.Services.Admin
{
    public class ProfessorAccountService
    {
        private readonly UniAppDbContext _context;
        public ProfessorAccountService(UniAppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GenerateProfessorId()
        {
            int year = DateTime.Now.Year;
            int yearStart = year * 100000;
            int yearEnd = (year + 1) * 100000;

            int lastProfessor = await _context.Professors
                .Where(p => p.Id >= yearStart && p.Id < yearEnd)
                .Select(p => p.Id)
                .OrderByDescending(id => id)
                .FirstOrDefaultAsync();

            return lastProfessor == 0 ? yearStart + 1 : lastProfessor + 1;
        }

        public async Task<ProfessorAdminDto> CreateProfessorAccountAsync(CreateProfessorDto dto)
        {
            int professorId = await GenerateProfessorId();
            var emailTaken = await _context.Users.SingleOrDefaultAsync(u => u.Email == dto.Email);
            if (emailTaken != null)
                throw new InvalidOperationException("That email is already registered");

            bool departmentExists = await _context.Departments.AnyAsync(d => d.Id == dto.DepartmentId);
            if (!departmentExists)
                throw new InvalidOperationException("Department not found");

            var user = new User
            {
                Name = $"{dto.FirstName.Trim()} {dto.LastName.Trim()}",
                Email = dto.Email.Trim().ToLower(),
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _context.Professors.Add(new Repository.Models.Professor
            {
                Id = professorId,
                DepartmentId = dto.DepartmentId,
                UserId = user.Id,
                IsActive = true
            });
            await _context.SaveChangesAsync();

            return new ProfessorAdminDto
            {
                ProfessorId = professorId,
                Name = user.Name,
                Email = user.Email,
                DepartmentName = await _context.Departments.Where(d => d.Id == dto.DepartmentId).Select(d => d.Name).FirstOrDefaultAsync() ?? string.Empty,
                IsActive = true
            };
        }

        public async Task<IEnumerable<ProfessorAdminDto>> GetAllProfessorsAsync()
        {
            return await _context.Professors
                .AsNoTracking()
                .Select(p => new ProfessorAdminDto
                {
                    ProfessorId = p.Id,
                    Name = p.User.Name,
                    Email = p.User.Email,
                    DepartmentName = p.Department.Name,
                    IsActive = p.IsActive
                })
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<string> UpdateProfessorAsync(int professorId, UpdateProfessorDto dto)
        {
            var professor = await _context.Professors.FirstOrDefaultAsync(p => p.Id == professorId);
            if (professor == null)
                throw new Exception("Professor not found");

            if (dto.DepartmentId.HasValue)
            {
                bool departmentExists = await _context.Departments.AnyAsync(d => d.Id == dto.DepartmentId.Value);
                if (!departmentExists)
                    throw new Exception("Department not found");
                professor.DepartmentId = dto.DepartmentId.Value;
            }

            if (dto.IsActive.HasValue)
                professor.IsActive = dto.IsActive.Value;

            await _context.SaveChangesAsync();
            return "Professor updated";
        }
    }
}
