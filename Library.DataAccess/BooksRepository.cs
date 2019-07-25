using System;
using System.Linq;
using System.Linq.Expressions;
using Library.Domain;

namespace Library.DataAccess
{
    public sealed class BooksRepository : IRepository<User, int>
    {
        BooksRepository()
        {
            
        }

        public void Create(User entity)
        {
            throw new NotImplementedException();
        }

        public User GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateProperties(User entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> Get(Expression<Func<User, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}