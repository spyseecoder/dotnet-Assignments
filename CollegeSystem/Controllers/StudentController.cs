using CollegeSystem.Common.Models;
using CollegeSystem.Service.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace CollegeSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("active")]
        public IActionResult GetActiveStudents()
        {
            var result = _studentService.GetActiveStudents();
            return Ok(result);
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> GetStudentByGuid(string guid)
        {
            var result = await _studentService.GetStudentByGuidAsync(guid);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] StudentCreateDto student)
        {
            try
            {
                var result = await _studentService.InsertStudentAsync(student);

                if (!result)
                    return BadRequest("Insert failed");

                return Ok("Student created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Roles = "Admin,Faculty")]
        [HttpPut("{guid}")]
        public async Task<IActionResult> UpdateStudent(string guid, [FromBody] StudentCreateDto student)
        {
            try
            {
                var result = await _studentService.UpdateStudentAsync(guid, student);

                return Ok("Student updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{guid}")]
        public async Task<IActionResult> DeleteStudent(string guid)
        {
            try
            {
                await _studentService.DeleteStudentAsync(guid);

                return Ok("Student deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("enrollments")]
        public async Task<IActionResult> GetEnrollments(int page = 1, int pageSize = 10)
        {
            var data = await _studentService.GetEnrollmentsPaged(page, pageSize);
            return Ok(data);
        }
    }
}
