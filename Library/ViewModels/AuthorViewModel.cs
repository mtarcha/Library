using System;
using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels
{
    public class AuthorViewModel
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? DateOfDeath { get; set; }
    }
}