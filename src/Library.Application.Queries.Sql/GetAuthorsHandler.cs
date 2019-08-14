using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Library.Application.Queries.Common;
using Library.Application.Queries.GetAuthors;

namespace Library.Application.Queries.Sql
{
    public class GetAuthorsHandler : IGetAuthorsQueryHandler
    {
        private readonly IConnectionFactory _connectionFactory;

        public GetAuthorsHandler(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<GetAuthorsResult> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
        {
            using (var connection = _connectionFactory.Create())
            {
                try
                {
                    var query = @"Select Id, Name as FirstName, SurName as LastName, DateOfBirth, DateOfDeath 
                                from dbo.Authors
                                where Name like '%@SubName%' or SurName like '%@SubName%'";

                    var authors = await connection.QueryAsync<Author>(query, new { SubName = request.SubName });

                    return new GetAuthorsResult(authors);
                }
                catch (Exception e)
                {
                    return new GetAuthorsResult(e);
                }
            }
        }
    }
}