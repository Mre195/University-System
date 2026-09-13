using System;
using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Services.DTOs.Professor.SpecialRequests
{
    public class ReviewSpecialRequestDto
    {
        [Required]
        public Guid RequestId { get; set; }

        [Required]
        public bool Approve { get; set; }
    }
}
