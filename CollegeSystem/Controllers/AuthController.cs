using CollegeSystem.Common.Models;
using CollegeSystem.Service.Abstractions;
using Microsoft.AspNetCore.Mvc;
using CollegeSystem.API.Helpers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly JwtHelper _jwtHelper;

    public AuthController(IStudentService studentService, JwtHelper jwtHelper)
    {
        _studentService = studentService;
        _jwtHelper = jwtHelper;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        var user = await _studentService.LoginAsync(login.FirstName, login.Password);

        if (user == null)
            return Unauthorized("Invalid credentials");

        var token = _jwtHelper.GenerateToken(user.FirstName, user.Role);

        return Ok(new { token });
    }
}