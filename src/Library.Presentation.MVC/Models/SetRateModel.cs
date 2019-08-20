using System;

namespace Library.Presentation.MVC.Models
{
    public class SetRateModel
    {
        public Guid BookId { get; set; }

        public Guid UserId { get; set; }

        public int Rate { get; set; }
    }
}