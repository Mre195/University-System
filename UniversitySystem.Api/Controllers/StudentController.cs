using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Services.DTOs.Professor.SpecialRequests;
using UniversitySystem.Services.DTOs.Student.Requests;
using UniversitySystem.Services.Services.Student;

namespace UniversitySystem.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Student")]
    public class StudentController : BaseApiController
    {
        private readonly ScheduleService scheduleService;
        private readonly SectionRegistrationService sectionRegistrationService;
        private readonly CompletedCoursesService completedCoursesService;
        private readonly SemesterMarksService semesterMarksService;
        private readonly NotificationService notificationService;
        private readonly SpecialRequestService specialRequestService;

        public StudentController(
            ScheduleService scheduleService,
            SectionRegistrationService sectionRegistrationService,
            CompletedCoursesService completedCoursesService,
            SemesterMarksService semesterMarksService,
            NotificationService notificationService,
            SpecialRequestService specialRequestService)
        {
            this.scheduleService = scheduleService;
            this.sectionRegistrationService = sectionRegistrationService;
            this.completedCoursesService = completedCoursesService;
            this.semesterMarksService = semesterMarksService;
            this.notificationService = notificationService;
            this.specialRequestService = specialRequestService;
        }

        [HttpGet("my-schedule")]
        public async Task<IActionResult> GetSchedule() =>
             Ok(await scheduleService.GetStudentScheduleAsync(GetId()));

        [HttpGet("available-sections")]
        public async Task<IActionResult> GetAvailableSections() =>
            Ok(await sectionRegistrationService.GetStudentAvailableSectionsAsync(GetId()));

        [HttpPost("section-registration")]
        public async Task<IActionResult> RegisterForSections([FromBody] int sectionId) =>
            Ok(await sectionRegistrationService.RegisterStudentInSectionAsync(GetId(), sectionId));

        [HttpGet("completed-courses")]
        public async Task<IActionResult> GetCompletedCourses() =>
            Ok(await completedCoursesService.StudentCompletedCoursesAsync(GetId()));

        [HttpGet("semester-marks")]
        public async Task<IActionResult> GetSemesterMarks() =>
            Ok(await semesterMarksService.GetStudentSemesterMarksAsync(GetId()));

        [HttpGet("notifications-unread-count")]
        public async Task<IActionResult> GetUnreadNotificationCount() =>
            Ok(await notificationService.GetStudentUnreadCountAsync(GetId()));

        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotifications() =>
            Ok(await notificationService.GetStudentNotificationsAsync(GetId()));

        [HttpPut("notifications/{notificationId}/read")]
        public async Task<IActionResult> MarkNotificationAsRead(Guid notificationId) =>
            Ok(await notificationService.MarkAsReadAsync(notificationId));

        [HttpPost("special-requests")]
        public async Task<IActionResult> SubmitSpecialRequestApi([FromBody] CreateSpecialRequestDto dto) =>
            Ok(await specialRequestService.SubmitStudentSpecialRequestAsync(GetId(), dto));

        [HttpGet("my-special-requests")]
        public async Task<IActionResult> GetSpecialRequests() =>
            Ok(await specialRequestService.GetStudentSpecialRequestsAsync(GetId()));

        [HttpGet("review-my-special-requests")]
        public async Task<IActionResult> GetSpecialRequestsStatus([FromQuery] ReviewSpecialRequestDto dto) =>
             Ok(await specialRequestService.ReviewSpecialRequestStatusAsync(dto));
    }
}