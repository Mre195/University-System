using Microsoft.EntityFrameworkCore;
using UniversitySystem.Repository.DbConnection;
using UniversitySystem.Services.DTOs.Shared.Auth;

namespace UniversitySystem.Services.Services.Shared
{
    public class GetProfileService
    {
        private readonly UniAppDbContext _context;
        public GetProfileService(UniAppDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfileDto> GetProfileAsync(Guid userId, string role)
        {
            var query = role switch
            {
                "Student" => _context.Students
                    .Where(s => s.UserId == userId)
                    .Select(s => new UserProfileDto { Id = s.Id, Uid = s.UserId, Name = s.User.Name, Email = s.User.Email, Role = role }),

                "Professor" => _context.Professors
                    .Where(p => p.UserId == userId)
                    .Select(p => new UserProfileDto { Id = p.Id, Uid = p.UserId, Name = p.User.Name, Email = p.User.Email, Role = role }),

                "Admin" => _context.Admins
                    .Where(a => a.UserId == userId)
                    .Select(a => new UserProfileDto { Id = a.Id, Uid = a.UserId, Name = a.User.Name, Email = a.User.Email, Role = role }),

                _ => throw new ArgumentException("Invalid role")
            };

            return await query.AsNoTracking().FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException("User not found");

        }
    }
}

