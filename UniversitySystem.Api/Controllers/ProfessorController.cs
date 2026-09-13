using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Services.DTOs.Admin.Sections;
using UniversitySystem.Services.DTOs.Professor.Attendances;
using UniversitySystem.Services.DTOs.Professor.Marks;
using UniversitySystem.Services.Services.Professor;

namespace UniversitySystem.Api.Controllers
{
    [Route("api/professors")]
    [Authorize(Roles = "Professor")]
    public class ProfessorController : BaseApiController
    {
        private readonly SectionService sectionsService;
        private readonly UpdateMarksService updateMarksService;
        private readonly AttendanceService attendanceService;
        private readonly DepartmentStudentsService departmentStudentsService;

        public ProfessorController(
            SectionService sectionsService,
            UpdateMarksService updateMarksService,
            AttendanceService attendanceService,
            DepartmentStudentsService departmentStudentsService)
        {
            this.sectionsService = sectionsService;
            this.updateMarksService = updateMarksService;
            this.attendanceService = attendanceService;
            this.departmentStudentsService = departmentStudentsService;
        }

        [HttpGet("courses")]
        public async Task<IActionResult> GetMyCourses() =>
            Ok(await sectionsService.GetProfessorSectionsAsync(GetId()));

        [HttpGet("sections/{sectionId}/marks")]
        public async Task<IActionResult> GetSectionMarks([FromRoute] int sectionId) =>
            Ok(await updateMarksService.GetSectionMarksForProfessorAsync(GetId(), sectionId));

        [HttpPost("marks")]
        public async Task<IActionResult> UpdateMarks([FromBody] UpdateMarksDto dto) =>
            Ok(await updateMarksService.UpdateSectionMarksAsync(GetId(), dto));

        [HttpGet("sections/{sectionId}/attendance")]
        public async Task<IActionResult> GetSectionAttendance([FromRoute] int sectionId) =>
            Ok(await attendanceService.GetStudentsAttendanceForProfessorAsync(GetId(), sectionId));

        [HttpPost("sections/attendance")]
        public async Task<IActionResult> UpdateSectionAttendance([FromBody] int sectionId, [FromBody] List<UpdateAttendanceDto> studentsAttendance) =>
            Ok(await attendanceService.UpdateStudentsAttendanceForProfessorAsync(GetId(), sectionId, studentsAttendance));

        [HttpGet("department-students")]
        public async Task<IActionResult> GetDepartmentStudents() =>
            Ok(await departmentStudentsService.GetDepartmentStudentsByProfessorAsync(GetId()));
    }
}