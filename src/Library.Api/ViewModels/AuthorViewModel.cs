using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Api.ViewModels
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
        public DateTime DateOfBirth { get; set; }
        
        public DateTime? DateOfDeath { get; set; }
    }
}