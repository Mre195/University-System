using System;

namespace UniversitySystem.Services.DTOs.Student.Notifications
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid SpecialRequestId { get; set; }
    }
}
