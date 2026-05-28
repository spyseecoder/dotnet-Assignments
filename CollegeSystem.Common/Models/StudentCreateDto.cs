using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CollegeSystem.Common.Models
{
    public class StudentCreateDto
    {
        [Required(ErrorMessage = "First Name required")]
        [RegularExpression(@"^\w+$", ErrorMessage = "First Name must be a single word without spaces or punctuation")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name required")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Last Name must contain only letters and spaces")]
        public string LastName { get; set; }
        [RegularExpression(@"^[0-9]{4}$", ErrorMessage = "Enrollment Year must be a 4-digit number")]
        [Required(ErrorMessage = "Enrollment Year required")]
        public int EnrollmentYear { get; set; }
        [Required(ErrorMessage = "Department Name required")]
        public string DepartmentName { get; set; }
        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }
    }
}
