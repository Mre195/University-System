using Microsoft.EntityFrameworkCore;
using UniversitySystem.Repository.DbConnection;
using UniversitySystem.Repository.Models;
using UniversitySystem.Services.DTOs.Admin.SpecialRequests;
using UniversitySystem.Services.DTOs.Professor.SpecialRequests;
using UniversitySystem.Services.DTOs.Student.Requests;
using UniversitySystem.Services.Enums;

namespace UniversitySystem.Services.Services.Student
{
    public class SpecialRequestService
    {
        private readonly UniAppDbContext _context;
        public SpecialRequestService(UniAppDbContext context)
        {
            _context = context;
        }

        public async Task<SpecialRequestDto> SubmitStudentSpecialRequestAsync(Guid userId, CreateSpecialRequestDto dto)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId)
                ?? throw new InvalidOperationException("Student not found");

            if (dto.EnrollmentId.HasValue)
            {
                bool ownsEnrollment = await _context.Enrollments
                    .AnyAsync(e => e.Id == dto.EnrollmentId.Value && e.Student.UserId == userId);
                if (!ownsEnrollment)
                    throw new InvalidOperationException("Enrollment not found");
            }

            var request = new SpecialRequest
            {
                StudentId = student.Id,
                SpecialRequestTypeId = dto.SpecialRequestTypeId,
                EnrollmentId = dto.EnrollmentId,
                Description = dto.Description,
                SpecialRequestStatusId = (int)SpecialRequestStatusEnum.Pending
            };

            _context.SpecialRequests.Add(request);
            await _context.SaveChangesAsync();

            return new SpecialRequestDto
            {
                Id = request.Id,
                StudentId = request.StudentId,
                Description = request.Description,
                Status = "Pending",
                SubmittedAt = request.SubmittedAt
            };
        }

        public async Task<IEnumerable<SpecialRequestDto>> GetStudentSpecialRequestsAsync(Guid userId)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId)
                ?? throw new InvalidOperationException("Student not found");

            return await _context.SpecialRequests
                .AsNoTracking()
                .Where(r => r.StudentId == student.Id)
                .OrderByDescending(r => r.SubmittedAt)
                .Select(r => new SpecialRequestDto
                {
                    Id = r.Id,
                    StudentId = r.StudentId,
                    Type = r.SpecialRequestType.Type,
                    Description = r.Description,
                    Status = r.SpecialRequestStatus.Status,
                    SubmittedAt = r.SubmittedAt
                })
                .ToListAsync();
        }

        public async Task<Notification> ReviewSpecialRequestStatusAsync(ReviewSpecialRequestDto dto)
        {
            var request = await _context.SpecialRequests
                .Include(r => r.Student).ThenInclude(s => s.User)
                .FirstOrDefaultAsync(r => r.Id == dto.RequestId);

            if (request == null)
                throw new InvalidOperationException("Special request not found");

            request.SpecialRequestStatusId = dto.Approve ? (int)SpecialRequestStatusEnum.Approved : (int)SpecialRequestStatusEnum.Rejected;

            var notification = new Notification
            {
                SpecialRequestId = request.Id,
                TargetEmail = request.Student.User.Email,
                Message = dto.Approve ? "Your request is approved" : "Your request is rejected"
            };
            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync();
            return notification;
        }
    }
}
