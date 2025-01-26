using Microsoft.EntityFrameworkCore;
using AcademicApp.Domain.Entities;
using AcademicApp.Domain.Interfaces;
using AcademicApp.Infra.Data;

namespace AcademicApp.Infra.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Student?> GetByIdAsync(Guid id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<Student?> GetByRAAsync(string ra)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.RA == ra);
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> CreateAsync(Student student)
        {
            student.Id = Guid.NewGuid();
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student> UpdateAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _context.Students.FindAsync(id);
            if (existing == null) return false;

            _context.Students.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
