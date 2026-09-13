using Microsoft.EntityFrameworkCore;
using UniversitySystem.Repository.DbConnection;
using UniversitySystem.Services.DTOs.Professor.Students;

namespace UniversitySystem.Services.Services.Professor
{
    public class DepartmentStudentsService
    {
        private readonly UniAppDbContext _context;
        public DepartmentStudentsService(UniAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DepartmentStudentDto>> GetDepartmentStudentsByProfessorAsync(Guid userId)
        {
            var departmentId = await _context.Professors
               .AsNoTracking()
               .Where(p => p.UserId == userId)
               .Select(p => p.DepartmentId)
               .FirstOrDefaultAsync();
            if (departmentId <= 0)
                throw new Exception("Professor not found or does not belong to any department.");

            return await _context.Students
                .AsNoTracking()
                .Where(s => s.Major.DepartmentId == departmentId)
                .Select(s => new DepartmentStudentDto
                {
                    StudentId =   s.Id,
                    StudentName = s.User.Name,
                    Email =       s.User.Email,
                    MajorName =   s.Major.Name,
                })
                .ToListAsync();
        }
    }
}
