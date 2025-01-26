namespace AcademicApp.Domain.Entities
{
    public class Student
    {
          public Guid Id { get; set; }
        public required string RA { get; set; }     // Registro Acadêmico (único)
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string CPF { get; set; }
    }
}
