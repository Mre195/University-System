using Microsoft.EntityFrameworkCore;
using UniversitySystem.Repository.DbConnection;
using UniversitySystem.Repository.Models;
using UniversitySystem.Services.DTOs.Admin.Students;

namespace UniversitySystem.Services.Services.Admin
{
    public class StudentAccountService
    {
        private readonly UniAppDbContext _context;
        public StudentAccountService(UniAppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GenerateStudentId()
        {
            int year = DateTime.UtcNow.Year;
            int yearStart = year * 10000;
            int yearEnd = (year + 1) * 10000;

            int lastStudent = await _context.Students
                .Where(s => s.Id >= yearStart && s.Id < yearEnd)
                .Select(s => s.Id)
                .OrderByDescending(id => id)
                .FirstOrDefaultAsync();

            return lastStudent == 0 ? yearStart + 1 : lastStudent + 1;
        }

        public async Task<StudentAdminDto> CreateStudentAccountAsync(CreateStudentDto dto)
        {
            int newStudentId = await GenerateStudentId();

            bool emailTaken = await _context.Users.AnyAsync(u => u.Email == dto.Email.Trim().ToLower());
            if (emailTaken)
                throw new InvalidOperationException("That email is already registered");

            bool majorExists = await _context.Majors.AnyAsync(m => m.Id == dto.MajorId);
            if (!majorExists)
                throw new InvalidOperationException("Major not found");

            var user = new User
            {
                Name = $"{dto.FirstName.Trim()} {dto.LastName.Trim()}",
                Email = dto.Email.Trim().ToLower(),
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _context.Students.Add(new UniversitySystem.Repository.Models.Student
            {
                Id = newStudentId,
                UserId = user.Id,
                MajorId = dto.MajorId,
                IsActive = true
            });
            await _context.SaveChangesAsync();

            return new StudentAdminDto
            {
                StudentId = newStudentId,
                Name = user.Name,
                Email = user.Email,
                UserId = user.Id,
                MajorId = dto.MajorId,
                MajorName = await _context.Majors.Where(m => m.Id == dto.MajorId).Select(m => m.Name).FirstOrDefaultAsync() ?? string.Empty,
                IsActive = true
            };
        }

        public async Task<IEnumerable<StudentAdminDto>> GetAllStudentsAsync()
        {
            return await _context.Students
                .AsNoTracking()
                .Select(s => new StudentAdminDto
                {
                    StudentId = s.Id,
                    UserId = s.UserId,
                    Name = s.User.Name,
                    Email = s.User.Email,
                    MajorId = s.MajorId,
                    MajorName = s.Major.Name,
                    IsActive = s.IsActive
                })
                .OrderBy(s => s.Name)
                .ToListAsync();
        }
    }
}
