using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UniversitySystem.Repository.DbConnection;
using UniversitySystem.Services.Services.Shared;


// Alias to resolve the name collision between Admin and Professor SectionService
using ProfessorServices = UniversitySystem.Services.Services.Professor;
using StudentServices = UniversitySystem.Services.Services.Student;
using AdminServices = UniversitySystem.Services.Services.Admin;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Database
builder.Services.AddDbContext<UniAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ---------- JWT Authentication ----------
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new ArgumentNullException(null);
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

// ---------- Common & Auth Services ----------
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<GetProfileService>();

// ---------- Admin Services ----------
builder.Services.AddScoped<AdminServices.SemesterService>();
builder.Services.AddScoped<AdminServices.CourseService>();
builder.Services.AddScoped<AdminServices.SectionService>();
builder.Services.AddScoped<AdminServices.StudentAccountService>();
builder.Services.AddScoped<AdminServices.ProfessorAccountService>();
builder.Services.AddScoped<AdminServices.SpecialRequestAdminService>();
builder.Services.AddScoped<AdminServices.DepartmentService>();
builder.Services.AddScoped<AdminServices.MajorService>();

// ---------- Professor Services ----------
builder.Services.AddScoped<ProfessorServices.SectionService>();
builder.Services.AddScoped<ProfessorServices.UpdateMarksService>();
builder.Services.AddScoped<ProfessorServices.AttendanceService>();
builder.Services.AddScoped<ProfessorServices.DepartmentStudentsService>();

// ---------- Student Services ----------
builder.Services.AddScoped<StudentServices.ScheduleService>();
builder.Services.AddScoped<StudentServices.SectionRegistrationService>();
builder.Services.AddScoped<StudentServices.CompletedCoursesService>();
builder.Services.AddScoped<StudentServices.SemesterMarksService>();
builder.Services.AddScoped<StudentServices.NotificationService>();
builder.Services.AddScoped<StudentServices.SpecialRequestService>();

var app = builder.Build();

//app.UseExceptionHandling();   // catches exceptions from everything below it
//app.UseRequestLogging();      // logs every request/response
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();