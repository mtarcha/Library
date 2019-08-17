using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Api.ViewModels
{
    public class SetRateViewModel
    {
        [Required]
        public Guid BookId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public int Rate { get; set; }
    }
}