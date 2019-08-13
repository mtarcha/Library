using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Library.Application.Common;
using Library.Application.Queries.GetBook;
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
                var query = $@"Select b.Id, b.Name, b.Picture, b.Date, b.Summary, b.Rate, 
                                    a.Id, a.Name as FirstName, a.SurName as LastName, a.DateOfBirth, a.DateOfDeath
                            from dbo.BookAuthorEntity as ba 
                                inner join dbo.Books as b on ba.BookId = b.Id 
	                            inner join dbo.Authors as a on ba.AuthorId = a.Id
                            where b.Id in (Select Id from 
                                (Select distinct b.Id, b.Rate
                                from dbo.BookAuthorEntity as ba 
                                    inner join dbo.Books as b on ba.BookId = b.Id 	
                                where b.Name like '%{request.SearchPattern}%' or b.Summary like '%{request.SearchPattern}%'
                                ORDER BY b.Rate DESC 
                                OFFSET {request.SkipCount} ROWS 
                                FETCH NEXT {request.TakeCount} ROWS ONLY ) as books) ";

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
                    cancellationToken);
                
                var allBooksCount = await connection.ExecuteScalarAsync<int>(
                    "Select Count(*) from dbo.Books",
                    cancellationToken);

                return new SearchBooksResult(books.Values, allBooksCount, books.Count);
            }
        }
    }
}