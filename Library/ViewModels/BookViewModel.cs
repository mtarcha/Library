﻿using System;
using System.Collections.Generic;

namespace Library.Presentation.ViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte[] Picture { get; set; }

        public DateTime Date { get; set; }

        public IEnumerable<AuthorViewModel> Authors { get; set; }

        public string Summary { get; set; }

        public int Rate { get; set; }
    }
}