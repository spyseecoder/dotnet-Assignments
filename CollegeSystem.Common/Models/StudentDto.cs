using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CollegeSystem.Common.Models
{
    public class StudentDto
    {
        [Required(ErrorMessage ="FirstName Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage ="last name required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "department name required")]
        public string DepartmentName { get; set; }
        public string Role { get; set; }
    }
}
