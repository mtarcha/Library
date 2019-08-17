using System.Collections.Generic;
using Library.Application.Queries.Common;
using MediatR;

namespace Library.Application.Queries.GetAuthors
{
    public class GetAuthorsQuery : IRequest<IEnumerable<Author>>
    {
        public string SubName { get; set; }
    }
}