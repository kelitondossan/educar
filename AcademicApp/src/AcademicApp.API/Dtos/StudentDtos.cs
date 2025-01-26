namespace AcademicApp.API.Dtos
{
    public class StudentCreateDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? RA { get; set; }
        public string? CPF { get; set; }
    }

    public class StudentUpdateDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        // RA e CPF não serão editados no PUT
    }
}
