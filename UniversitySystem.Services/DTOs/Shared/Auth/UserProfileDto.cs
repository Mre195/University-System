using System;

namespace UniversitySystem.Services.DTOs.Shared.Auth
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public Guid Uid { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
