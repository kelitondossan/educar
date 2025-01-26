using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using AcademicApp.Domain.Entities;
using AcademicApp.Domain.Interfaces;
using AcademicApp.Domain.Services;

namespace AcademicApp.Tests
{
    public class StudentServiceTests
    {
        [Fact]
        public async Task CreateStudentAsync_ShouldCreateNewStudent()
        {
            // Arrange
            var mockRepo = new Mock<IStudentRepository>();
            mockRepo.Setup(r => r.GetByRAAsync(It.IsAny<string>()))
                    .ReturnsAsync((Student?)null); // Allow null returns

            mockRepo.Setup(r => r.CreateAsync(It.IsAny<Student>()))
                    .ReturnsAsync((Student s) => s);

            var service = new StudentService(mockRepo.Object);

            var newStudent = new Student
            {
                RA = "12345",
                Name = "Test Student",
                Email = "test@test.com",
                CPF = "999.999.999-99"
            };

            // Act
            var result = await service.CreateStudentAsync(newStudent);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("12345", result.RA);
            mockRepo.Verify(r => r.CreateAsync(It.IsAny<Student>()), Times.Once);
        }

        [Fact]
        public async Task CreateStudentAsync_ShouldThrowException_IfRAExists()
        {
            // Arrange
            var existingStudent = new Student
            {
                RA = "12345",
                Name = "Existing Student",
                Email = "existing@student.com",
                CPF = "111.111.111-11"
            };

            var mockRepo = new Mock<IStudentRepository>();
            mockRepo.Setup(r => r.GetByRAAsync("12345"))
                    .ReturnsAsync(existingStudent);

            var service = new StudentService(mockRepo.Object);

            var newStudent = new Student
            {
                RA = "12345",
                Name = "Another Student",
                Email = "another@test.com",
                CPF = "888.888.888-88"
            };

            await Assert.ThrowsAsync<Exception>(() => service.CreateStudentAsync(newStudent));
        }

    }
}
