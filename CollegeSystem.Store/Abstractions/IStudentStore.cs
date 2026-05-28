using System;
using System.Collections.Generic;
using System.Text;
using CollegeSystem.Common.Models;

namespace CollegeSystem.Store.Abstractions
{
    public interface IStudentStore
    {
        List<StudentDto> GetActiveStudents();
        Task<StudentDto> GetStudentByGuidAsync(string guid);
        Task<bool> InsertStudentAsync(StudentCreateDto student);
        Task<bool> UpdateStudentAsync(string guid, StudentCreateDto student);
        Task<bool> DeleteStudentAsync(string guid);
        Task<StudentDto> LoginAsync(string firstName, string password);
        Task<List<EnrollmentDto>> GetEnrollmentsPaged(int page, int pageSize);
    }
}
