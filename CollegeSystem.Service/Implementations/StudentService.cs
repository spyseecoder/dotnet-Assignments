using System;
using System.Collections.Generic;
using System.Text;
using CollegeSystem.Common.Models;
using CollegeSystem.Service.Abstractions;
using CollegeSystem.Store.Abstractions;

namespace CollegeSystem.Service.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStudentStore _Store;
        public StudentService(IStudentStore store)
        {
            _Store = store;
        }
        public List<StudentDto> GetActiveStudents()
        {
            return _Store.GetActiveStudents();
        }
        public async Task<StudentDto> GetStudentByGuidAsync(string guid)
        {
            return await _Store.GetStudentByGuidAsync(guid);
        }
        public async Task<bool> InsertStudentAsync(StudentCreateDto student)
        {
            return await _Store.InsertStudentAsync(student);
        }

        public async Task<bool> UpdateStudentAsync(string guid, StudentCreateDto student)
        {
            return await _Store.UpdateStudentAsync(guid, student);
        }

        public async Task<bool> DeleteStudentAsync(string guid)
        {
            return await _Store.DeleteStudentAsync(guid);
        }

        public async Task<StudentDto> LoginAsync(string firstName, string password)
        {
            return await _Store.LoginAsync(firstName, password);
        }

        public async Task<List<EnrollmentDto>> GetEnrollmentsPaged(int page, int pageSize)
        {
            return await _Store.GetEnrollmentsPaged(page, pageSize);
        }
    }
}
