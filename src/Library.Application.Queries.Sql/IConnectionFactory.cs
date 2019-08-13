using System.Data.SqlClient;

namespace Library.Application.Queries.Sql
{
    public interface IConnectionFactory
    {
        SqlConnection Create();
    }
}