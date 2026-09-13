using Microsoft.EntityFrameworkCore;
using UniversitySystem.Repository.DbConnection;
using UniversitySystem.Repository.Models;
using UniversitySystem.Services.DTOs.Admin.SpecialRequests;
using UniversitySystem.Services.DTOs.Professor.SpecialRequests;
using UniversitySystem.Services.Services.Student;

namespace UniversitySystem.Services.Services.Admin
{
    public class SpecialRequestAdminService
    {
        private readonly UniAppDbContext _context;
        private readonly SpecialRequestService _specialRequestService;

        public SpecialRequestAdminService(UniAppDbContext context, SpecialRequestService specialRequestService)
        {
            _context = context;
            _specialRequestService = specialRequestService;
        }

        public async Task<IEnumerable<SpecialRequestDto>> GetAllSpecialRequestsAsync(int statusId)
        {
            return await _context.SpecialRequests
                .AsNoTracking()
                .OrderByDescending(r => r.SubmittedAt)
                .Where(r => r.SpecialRequestStatusId == statusId)
                .Select(r => new SpecialRequestDto
                {
                    Id = r.Id,
                    StudentId = r.StudentId,
                    StudentName = r.Student.User.Name,
                    Type = r.SpecialRequestType.Type,
                    Description = r.Description,
                    Status = r.SpecialRequestStatus.Status,
                    SubmittedAt = r.SubmittedAt
                })
                .ToListAsync();
        }

        public async Task<Notification> ReviewSpecialRequestAsync(ReviewSpecialRequestDto dto)
        {
            return await _specialRequestService.ReviewSpecialRequestStatusAsync(dto);
        }
    }
}
