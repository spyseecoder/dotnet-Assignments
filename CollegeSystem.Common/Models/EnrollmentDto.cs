using System;
using System.Collections.Generic;
using System.Text;

namespace CollegeSystem.Common.Models
{
    public class EnrollmentDto
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Grade { get; set; }
    }
}
