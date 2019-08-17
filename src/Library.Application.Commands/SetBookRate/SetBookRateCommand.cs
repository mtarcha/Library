using System;
using MediatR;

namespace Library.Application.Commands.SetBookRate
{
    public class SetBookRateCommand : IRequest<SetBookRateResult>
    {
        public string UserName { get; set; }

        public Guid BookId { get; set; }

        public int Rate { get; set; }
    }
}