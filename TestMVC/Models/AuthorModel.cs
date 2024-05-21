﻿using System.ComponentModel.DataAnnotations;

namespace TestMVC.Models
{
    public class AuthorModel
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Surname { get; set; }
        [MaxLength(50)]
        public int BirthYear { get; set; }
    }
}
