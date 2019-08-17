using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Library.Application.Queries.Common;
using Library.Application.Queries.GetBooks;

namespace Library.Application.Queries.Sql
{
    public class GetBooksHandler : IGetBooksHandler
    {
        private readonly IConnectionFactory _connectionFactory;

        public GetBooksHandler(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<SearchBooksResult> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            using (var connection = _connectionFactory.Create())
            {
                var query = @"Select b.Id, b.Name, b.Picture, b.Date, b.Summary, b.Rate, 
                                    a.Id, a.Name as FirstName, a.SurName as LastName, a.DateOfBirth, a.DateOfDeath
                            from dbo.BookAuthorEntity as ba 
                                inner join dbo.Books as b on ba.BookId = b.Id 
	                            inner join dbo.Authors as a on ba.AuthorId = a.Id
                            where b.Id in (Select Id from 
                                (Select distinct b.Id, b.Rate
                                from dbo.BookAuthorEntity as ba 
                                    inner join dbo.Books as b on ba.BookId = b.Id 	
                                where b.Name like @SearchPattern or b.Summary like @SearchPattern
                                ORDER BY b.Rate DESC 
                                OFFSET @SkipCount ROWS 
                                FETCH NEXT @TakeCount ROWS ONLY ) as books)";

                var books = new Dictionary<Guid, Book>();
                var res = await connection.QueryAsync<Book, Author, Book>(
                    query,
                    (book, author) =>
                    {
                        if (!books.TryGetValue(book.Id, out var theBook))
                        {
                            theBook = book;
                            books.Add(book.Id, theBook);
                        }

                        theBook.Authors.Add(author);
                        return theBook;
                    },
                    new { SearchPattern = $"%{request.SearchPattern}%", SkipCount = request.SkipCount, TakeCount = request.TakeCount });

                var totalBooksCount = await connection.ExecuteScalarAsync<int>(
                    @"Select Count(*) 
                        from dbo.Books 
                        where Name like @SearchPattern or Summary like @SearchPattern",
                    new { SearchPattern = $"%{request.SearchPattern}%" });

                return new SearchBooksResult(books.Values, totalBooksCount, books.Count);
            }
        }
    }
}