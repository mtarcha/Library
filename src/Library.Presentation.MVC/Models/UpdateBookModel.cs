using System;
using System.Collections.Generic;

namespace Library.Presentation.MVC.Models
{
    public class UpdateBookModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte[] Picture { get; set; }
        
        public DateTime Date { get; set; }

        public string Summary { get; set; }

        public List<UpdateAuthorModel> Authors { get; set; }
    }
}