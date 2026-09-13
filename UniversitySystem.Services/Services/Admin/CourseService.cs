using Microsoft.EntityFrameworkCore;
using UniversitySystem.Repository.DbConnection;
using UniversitySystem.Repository.Models;
using UniversitySystem.Services.DTOs.Admin.Courses;

namespace UniversitySystem.Services.Services.Admin
{
    public class CourseService
    {
        private readonly UniAppDbContext _context;
        public CourseService(UniAppDbContext context)
        {
            _context = context;
        }

        public async Task<CourseAdminDto> CreateCourseAsync(CreateCourseDto dto)
        {
            bool majorExists = await _context.Majors.AnyAsync(m => m.Id == dto.MajorId);
            if (!majorExists)
                throw new Exception("Major not found");

            var nameExists = await _context.Courses
                .AnyAsync(c => c.Name.ToLower() == dto.Name.Trim().ToLower());
            if (nameExists)
                throw new InvalidOperationException($"A course named '{dto.Name}' already exists.");

            if (dto.CreditHours is < 1 or > 6)
                throw new Exception("Credit hours must be between 1 and 6");

            var course = new Course
            {
                Name = dto.Name,
                CreditHours = dto.CreditHours,
                MajorId = dto.MajorId
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return new CourseAdminDto
            {
                Id = course.Id,
                Name = course.Name,
                CreditHours = course.CreditHours,
                MajorId = course.MajorId
            };
        }

        public async Task<string> UpdateCourseAsync(int courseId, UpdateCourseDto dto)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId)
                ?? throw new Exception("Course not found");

            if (dto.CreditHours is < 1 or > 6)
                throw new Exception("Credit hours must be between 1 and 6");

            course.Name = dto.Name;
            course.CreditHours = dto.CreditHours;
            await _context.SaveChangesAsync();

            return "Course updated successfully.";
        }

        public async Task<string> DeleteCourseAsync(int courseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId)
                ?? throw new Exception("Course not found");

            bool hasSections = await _context.Sections.AnyAsync(s => s.CourseId == courseId);
            if (hasSections)
                throw new Exception("Cannot delete: sections exist for this course");

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return "Course deleted";
        }

        public async Task<IEnumerable<CourseAdminDto>> GetAllCoursesAsync(int majorId)
        {
            return await _context.Courses
                .AsNoTracking()
                .Where(c => c.MajorId == majorId)
                .Select(c => new CourseAdminDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    CreditHours = c.CreditHours,
                    MajorId = c.MajorId,
                    MajorName = c.Major.Name
                })
                .OrderBy(c => c.Name)
                .ToListAsync();
        }
    }
}
