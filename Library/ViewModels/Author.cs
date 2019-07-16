namespace Library.ViewModels
{
    public class Author
    {
        public Author(string firstName, string sureName)
        {
            FirstName = firstName;
            SureName = sureName;
        }

        public string FirstName { get; }

        public string SureName { get; }
    }
}