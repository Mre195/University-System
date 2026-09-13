using Microsoft.EntityFrameworkCore;
using UniversitySystem.Repository.DbConnection;
using UniversitySystem.Services.DTOs.Student.Notifications;

namespace UniversitySystem.Services.Services.Student
{
    public class NotificationService
    {
        private readonly UniAppDbContext _context;
        public NotificationService(UniAppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetStudentUnreadCountAsync(Guid userId)
        {
            var email = await _context.Students
                .AsNoTracking()
                .Where(s => s.UserId == userId)
                .Select(s => s.User.Email)
                .FirstOrDefaultAsync();
            if (email == null)
                throw new InvalidOperationException("Student not found");

            return await _context.Notifications
                .AsNoTracking()
                .Where(n => n.TargetEmail == email && !n.IsRead)
                .CountAsync();
        }

        public async Task<IEnumerable<NotificationDto>> GetStudentNotificationsAsync(Guid userId)
        {
            var email = await _context.Students
                .AsNoTracking()
                .Where(s => s.UserId == userId)
                .Select(s => s.User.Email)
                .FirstOrDefaultAsync();

            if (email == null)
                throw new InvalidOperationException("Student not found");

            return await _context.Notifications
                .AsNoTracking()
                .Where(n => n.TargetEmail == email)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt,
                    SpecialRequestId = n.SpecialRequestId
                })
                .ToListAsync();
        }

        public async Task<string> MarkAsReadAsync(Guid notificationId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId)
                ?? throw new InvalidOperationException("Notification not found");

            notification.IsRead = true;
            await _context.SaveChangesAsync();

            return "Marked as read";
        }
    }
}
