using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AcademicApp.Domain.Entities;

namespace AcademicApp.Domain.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student?> GetByIdAsync(Guid id);
        Task<Student?> GetByRAAsync(string ra);
        Task<List<Student>> GetAllAsync();
        Task<Student> CreateAsync(Student student);
        Task<Student> UpdateAsync(Student student);
        Task<bool> DeleteAsync(Guid id);
    }
}
