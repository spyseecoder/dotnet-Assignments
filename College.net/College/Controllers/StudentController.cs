using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using College.Common.Models;

namespace College.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Route("api")]
    public class StudentController : ControllerBase
    {
        private static List<Student> students = new List<Student>
        {
            new Student { Id = 1, Name = "John Doe", Email = "utk@hdu.com" },
            new Student { Id = 2, Name = "Jane Smith", Email = "jane@hdu.com" } 
        };

        [HttpGet]
        public IActionResult GetStudents()
        {
            return Ok(students);
        }
        //by id
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);

            if (student == null)
                return NotFound("Student not found");

            return Ok(student);
        }
        //add new student
        [HttpPost]
        public IActionResult AddStudent(Student newStudent)
        {
            newStudent.Id = students.Max(s => s.Id) + 1;
            students.Add(newStudent);

            return Ok(newStudent);
        }
        //update student
        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, Student updatedStudent)
        {
            var student = students.FirstOrDefault(s => s.Id == id);

            if (student == null)
                return NotFound("Student not found");

            student.Name = updatedStudent.Name;
            student.Email = updatedStudent.Email;

            return Ok(student);
        }
        //delete student
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);

            if (student == null)
                return NotFound("Student not found");

            students.Remove(student);

            return Ok("Student deleted successfully");
        }

    }
}
