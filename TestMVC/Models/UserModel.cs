using System.ComponentModel.DataAnnotations;

namespace TestMVC.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }
        [MaxLength(50)]
        public string UserName { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
    }
}
