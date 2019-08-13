using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Library.Application.Common;
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
                var subName = request.SubName;
                var query = $@"Select Id, Name as FirstName, SurName as LastName, DateOfBirth, DateOfDeath 
                                from dbo.Authors
                                where Name like '%{subName}%' or SurName like '%{subName}%'";
                var authors = await connection.QueryAsync<Author>(query, cancellationToken);

                return new GetAuthorsResult(authors);
            }
        }
    }
}