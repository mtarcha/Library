using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Library.Application.Queries.Common;
using Library.Application.Queries.GetBook;

namespace Library.Application.Queries.Sql
{
    public class GetBookHandler : IGetBookHandler
    {
        private readonly IConnectionFactory _connectionFactory;

        public GetBookHandler(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Book> Handle(GetBookQuery request, CancellationToken cancellationToken)
        {
            using (var connection = _connectionFactory.Create())
            {

                var query = $@"Select b.Id, b.Name, b.Picture, b.Date, b.Summary, b.Rate, 
                                    a.Id, a.Name as FirstName, a.SurName as LastName, a.DateOfBirth, a.DateOfDeath
                               from dbo.BookAuthorEntity as ba 
		                            inner join dbo.Books as b on ba.BookId = b.Id 
		                            inner join dbo.Authors as a on ba.AuthorId = a.Id
                                where b.Id = @BookId";

                var authors = new List<Author>();
                var res = await connection.QueryAsync<Book, Author, Book>(
                    query,
                    (book, author) =>
                    {
                        authors.Add(author);
                        return book;
                    },
                    new { BookId = request.BookId }
                );

                var foundBook = res.First();
                foundBook.Authors = authors;

                return foundBook;
            }
        }
    }
}