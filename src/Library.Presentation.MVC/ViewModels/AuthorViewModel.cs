using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Presentation.MVC.ViewModels
{
    public class AuthorViewModel
    {
        public Guid Id { get; set; }

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