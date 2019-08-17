using System.Collections.Generic;
using Library.Application.Queries.Common;
using MediatR;

namespace Library.Application.Queries.GetAuthors
{
    public interface IGetAuthorsQueryHandler : IRequestHandler<GetAuthorsQuery, IEnumerable<Author>>
    { 
    }
}