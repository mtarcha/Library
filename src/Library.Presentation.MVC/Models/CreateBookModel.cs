using System;

namespace Library.Presentation.MVC.Models
{
    public class CreateBookModel
    {
        public string Name { get; set; }

        public byte[] Picture { get; set; }

        public DateTime Date { get; set; }

        public string Summary { get; set; }
    }
}