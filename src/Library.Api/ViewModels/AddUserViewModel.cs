using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Api.ViewModels
{
    public class AddUserViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
       public string PhoneNumber { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}