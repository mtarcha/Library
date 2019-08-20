using System;
using MediatR;

namespace Library.Application.Commands.SetBookRate
{
    public class SetBookRateCommand : IRequest<SetBookRateResult>
    {
        public Guid UserId { get; set; }

        public Guid BookId { get; set; }

        public int Rate { get; set; }
    }
}