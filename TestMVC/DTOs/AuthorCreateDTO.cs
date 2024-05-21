using System.ComponentModel.DataAnnotations;

namespace TestMVC.DTOs
{
    public class AuthorCreateDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string BirthYear { get; set; }
    }
}
