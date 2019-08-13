using System;
using Library.Application.Common;
using MediatR;

namespace Library.Application.Commands.SetBookRate
{
    public class SetBookRateCommand : IRequest<RequestResult>
    {
        public string UserName { get; set; }

        public Guid BookId { get; set; }

        public int Rate { get; set; }
    }
}