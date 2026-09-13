using Microsoft.EntityFrameworkCore;
using UniversitySystem.Repository.DbConnection;
using UniversitySystem.Repository.Models;

namespace UniversitySystem.Services.Services.Shared
{
    public class AutoUpdateSemesterActiveStatus
    {
        private readonly UniAppDbContext _context;

        public AutoUpdateSemesterActiveStatus(UniAppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AutoUpdateSemesterActiveStatusAsync()
        {
            DateTime date = DateTime.UtcNow.Date;
            var semesterList = await _context.Semesters.ToListAsync();
            bool anyChanges = false;
            foreach (var semester in semesterList)
            {
                bool isActive = date >= semester.StartDate && date <= semester.EndDate;
                if (semester.IsActive != isActive)
                {
                    semester.IsActive = isActive;
                    anyChanges = true;
                }
            }

            if (anyChanges)
                await _context.SaveChangesAsync();

            return anyChanges;

        }
    }
}
