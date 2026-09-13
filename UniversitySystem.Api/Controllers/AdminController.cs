using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Services.DTOs.Admin.Courses;
using UniversitySystem.Services.DTOs.Admin.Departments;
using UniversitySystem.Services.DTOs.Admin.Majors;
using UniversitySystem.Services.DTOs.Admin.Professors;
using UniversitySystem.Services.DTOs.Admin.Sections;
using UniversitySystem.Services.DTOs.Admin.Semesters;
using UniversitySystem.Services.DTOs.Admin.Students;
using UniversitySystem.Services.DTOs.Professor.SpecialRequests;
using UniversitySystem.Services.Services.Admin;

namespace UniversitySystem.Api.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly SemesterService semesterService;
        private readonly CourseService courseService;
        private readonly SectionService sectionService;
        private readonly StudentAccountService studentAccountService;
        private readonly ProfessorAccountService professorAccountService;
        private readonly SpecialRequestAdminService specialRequestAdminService;
        private readonly DepartmentService departmentService;
        private readonly MajorService majorService;

        public AdminController(
            SemesterService semesterService,
            CourseService courseService,
            SectionService sectionService,
            StudentAccountService studentAccountService,
            ProfessorAccountService professorAccountService,
            SpecialRequestAdminService specialRequestAdminService,
            DepartmentService departmentService,
            MajorService majorService)
        {
            this.semesterService = semesterService;
            this.courseService = courseService;
            this.sectionService = sectionService;
            this.studentAccountService = studentAccountService;
            this.professorAccountService = professorAccountService;
            this.specialRequestAdminService = specialRequestAdminService;
            this.departmentService = departmentService;
            this.majorService = majorService;
        }

        // ================= Departments =================

        [HttpPost("create-department")]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentDto dto) =>
            await departmentService.CreateDepartmentAsync(dto) is var res && res != null ? Ok(res) : BadRequest();

        [HttpGet("get-all-departments")]
        public async Task<IActionResult> GetAllDepartments() =>
            await departmentService.GetAllDepartmentsAsync() is var res && res != null ? Ok(res) : BadRequest();


        // ================= Majors =================

        [HttpPost("create-major")]
        public async Task<IActionResult> CreateMajor([FromBody] CreateMajorDto dto) =>
            await majorService.CreateMajorAsync(dto) is var res && res != null ? Ok(res) : BadRequest();

        [HttpGet("get-all-majors")]
        public async Task<IActionResult> GetAllMajors([FromQuery] int departmentId) =>
            await majorService.GetAllMajorsAsync(departmentId) is var res && res != null ? Ok(res) : BadRequest();


        // ================= Semesters =================

        [HttpPost("create-semester")]
        public async Task<IActionResult> CreateSemester([FromBody] CreateSemesterDto dto) =>
            await semesterService.CreateSemesterAsync(dto) is var res && res != null ? Ok(res) : BadRequest();


        [HttpGet("get-all-semesters")]
        public async Task<IActionResult> GetAllSemesters() =>
            await semesterService.GetAllSemestersAsync() is var res && res != null ? Ok(res) : BadRequest();


        // ================= Courses =================

        [HttpPost("create-course")]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto dto) =>
            await courseService.CreateCourseAsync(dto) is var res && res != null ? Ok(res) : BadRequest();

        [HttpGet("get-all-courses")]
        public async Task<IActionResult> GetAllCourses([FromQuery] int majorId) =>
            await courseService.GetAllCoursesAsync(majorId) is var res && res != null ? Ok(res) : BadRequest();


        // ================= Sections =================

        [HttpPost("create-section")]
        public async Task<IActionResult> CreateSection([FromBody] CreateSectionDto dto) =>
            await sectionService.CreateSectionAsync(dto) is var res && res != null ? Ok(res) : BadRequest();

        [HttpGet("get-all-sections")]
        public async Task<IActionResult> GetSections([FromQuery] int semesterId, [FromQuery] int courseId) =>
            await sectionService.GetSectionsAsync(semesterId, courseId) is var res && res != null ? Ok(res) : BadRequest();


        // ================= Students =================

        [HttpPost("create-student")]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto dto) =>
            await studentAccountService.CreateStudentAccountAsync(dto) is var res && res != null ? Ok(res) : BadRequest();

        [HttpGet("get-all-students")]
        public async Task<IActionResult> GetAllStudents() =>
            await studentAccountService.GetAllStudentsAsync() is var res && res != null ? Ok(res) : BadRequest();


        // ================= Professors =================

        [HttpPost("create-professor")]
        public async Task<IActionResult> CreateProfessor([FromBody] CreateProfessorDto dto) =>
            await professorAccountService.CreateProfessorAccountAsync(dto) is var res && res != null ? Ok(res) : BadRequest();

        [HttpGet("get-all-professors")]
        public async Task<IActionResult> GetAllProfessors() =>
            await professorAccountService.GetAllProfessorsAsync() is var res && res != null ? Ok(res) : BadRequest();

        // ================= Special Requests =================

        [HttpGet("get-all-special-requests")]
        public async Task<IActionResult> GetAllSpecialRequests([FromQuery] int statusId) =>
            await specialRequestAdminService.GetAllSpecialRequestsAsync(statusId) is var res && res != null ? Ok(res) : BadRequest();

        [HttpGet("special-requests/review")]
        public async Task<IActionResult> ReviewSpecialRequest([FromQuery] ReviewSpecialRequestDto dto) =>
            await specialRequestAdminService.ReviewSpecialRequestAsync(dto) is var res && res != null ? Ok(res) : BadRequest();

    }
}