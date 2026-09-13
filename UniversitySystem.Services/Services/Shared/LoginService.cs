using Microsoft.EntityFrameworkCore;
using UniversitySystem.Repository.DbConnection;
using UniversitySystem.Services.DTOs.Shared.Auth;
namespace UniversitySystem.Services.Services.Shared;

public class LoginService
{
    private readonly UniAppDbContext _context;
    private readonly JwtService _jwtService;
    public LoginService(UniAppDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<string> LoginAsync(LoginDto request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u=> u.Email == request.Email);
        string passwordHash =  user?.Password ?? "$2a$11$DJV28hhg27bwFsoYkDBCEOpC1iALIPqqXDN.O20/63QpH2X3x4uum";
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, passwordHash);
        if (user == null || !isPasswordValid)
            throw new InvalidOperationException("Invalid Email or Password");

        var status = await _context.Students
                .Where(s => s.User.Id == user.Id)
                .Select(s => "Student")
            .Concat(_context.Professors
                .Where(p => p.User.Id == user.Id)
                .Select(p => "Professor"))
            .FirstOrDefaultAsync();

        string role = status ?? "Admin";
        return _jwtService.tokenGenerator(user.Id, user.Email, role);
    }

}
