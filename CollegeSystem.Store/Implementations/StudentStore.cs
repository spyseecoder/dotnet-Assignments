
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using CollegeSystem.Common.Models;
using CollegeSystem.Store.Abstractions;

namespace CollegeSystem.Store.Implementations
{
    public class StudentStore : IStudentStore
    {
        private readonly string _connectionString;

        public StudentStore(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        private string HashPassword(string password)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public List<StudentDto> GetActiveStudents()
        {
            List<StudentDto> students = new List<StudentDto>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetAllStudentsActive", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new StudentDto
                            {
                                FirstName = reader["first_name"].ToString(),
                                LastName = reader["last_name"].ToString(),
                                DepartmentName = reader["department_name"].ToString()
                            });
                        }
                    }
                }
            }

            return students;
        }

        public async Task<StudentDto> GetStudentByGuidAsync(string guid)
        {
            StudentDto student = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetStudentByGuid", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StudentGuid", guid);

                    await conn.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            student = new StudentDto
                            {
                                FirstName = reader["first_name"].ToString(),
                                LastName = reader["last_name"].ToString(),
                                DepartmentName = reader["department_name"].ToString()
                            };
                        }
                    }
                }
            }

            return student;
        }

        public async Task<bool> InsertStudentAsync(StudentCreateDto student)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_InsertStudent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var hashedPassword = HashPassword(student.Password);
                    cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", student.LastName);
                    cmd.Parameters.AddWithValue("@EnrollmentYear", student.EnrollmentYear);
                    cmd.Parameters.AddWithValue("@DepartmentName", student.DepartmentName);
                    cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                    await conn.OpenAsync();

                    await cmd.ExecuteNonQueryAsync();

                    return true;
                }
            }
        }

        public async Task<bool> UpdateStudentAsync(string guid, StudentCreateDto student)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_UpdateStudentByGuid", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StudentGuid", guid);
                    cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", student.LastName);
                    cmd.Parameters.AddWithValue("@EnrollmentYear", student.EnrollmentYear);
                    cmd.Parameters.AddWithValue("@DepartmentName", student.DepartmentName);

                    await conn.OpenAsync();

                    await cmd.ExecuteNonQueryAsync();

                    return true;
                }
            }
        }

        public async Task<bool> DeleteStudentAsync(string guid)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_DeleteStudentByGuid", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StudentGuid", guid);

                    await conn.OpenAsync();

                    await cmd.ExecuteNonQueryAsync();

                    return true;
                }
            }
        }

        public async Task<StudentDto> LoginAsync(string firstName, string password)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_LoginStudent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var hashedPassword = HashPassword(password);

                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                    await conn.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new StudentDto
                            {
                                FirstName = reader["first_name"].ToString(),
                                Role = reader["role_name"].ToString()
                            };
                        }
                    }   
                }
            }

            return null;
        }


        public async Task<List<EnrollmentDto>> GetEnrollmentsPaged(int page, int pageSize)
        {
            var result = new List<EnrollmentDto>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"
            SELECT student_id, course_id, enrollment_date, grade
            FROM tbl_enrollment
            ORDER BY enrollment_date DESC
            OFFSET (@Page - 1) * @PageSize ROWS
            FETCH NEXT @PageSize ROWS ONLY", conn))
                {
                    cmd.Parameters.AddWithValue("@Page", page);
                    cmd.Parameters.AddWithValue("@PageSize", pageSize);

                    await conn.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result.Add(new EnrollmentDto
                            {
                                StudentId = Convert.ToInt32(reader["student_id"]),
                                CourseId = Convert.ToInt32(reader["course_id"]),
                                EnrollmentDate = Convert.ToDateTime(reader["enrollment_date"]),
                                Grade = reader["grade"].ToString()
                            });
                        }
                    }
                }
            }

            return result;
        }

    }
}