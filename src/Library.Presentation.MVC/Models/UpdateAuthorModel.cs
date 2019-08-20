using System;

namespace Library.Presentation.MVC.Models
{
    public class UpdateAuthorModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }
        
        public DateTime? DateOfDeath { get; set; }
    }
}