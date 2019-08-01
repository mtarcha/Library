namespace Library.Infrastucture.Data.Entities
{
    public class BookAuthorEntity
    {
        public int BookId { get; set; }
        public int AuthorId { get; set; }

        public BookEntity Book { get; set; }
        public AuthorEntity Author { get; set; }
    }
}