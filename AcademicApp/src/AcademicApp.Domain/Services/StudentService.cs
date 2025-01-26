using AcademicApp.Domain.Entities;
using AcademicApp.Domain.Interfaces;

namespace AcademicApp.Domain.Services
{
    public class StudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<Student> CreateStudentAsync(Student newStudent)
        {
            var existing = await _studentRepository.GetByRAAsync(newStudent.RA);
            if (existing != null)
                throw new Exception("RA already in use.");

            return await _studentRepository.CreateAsync(newStudent);
        }

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            var existing = await _studentRepository.GetByIdAsync(student.Id);
            if (existing == null)
                throw new Exception("Student not found.");

            student.RA = existing.RA;
            student.CPF = existing.CPF;

            return await _studentRepository.UpdateAsync(student);
        }

        public async Task<bool> DeleteStudentAsync(Guid id)
        {
            return await _studentRepository.DeleteAsync(id);
        }

        public async Task<Student> GetByIdAsync(Guid id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
                throw new Exception("Student not found.");

            return student;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _studentRepository.GetAllAsync();
        }
    }
}
