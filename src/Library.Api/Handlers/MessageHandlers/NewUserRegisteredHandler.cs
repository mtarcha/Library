using AutoMapper;
using Library.Application.Commands.AddUser;
using Library.Infrastructure;
using Library.Messaging.Contracts;
using MediatR;

namespace Library.Api.Handlers.MessageHandlers
{
    public class NewUserRegisteredHandler : IMessageHandler<NewUserRegistered>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public NewUserRegisteredHandler(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public void Handle(NewUserRegistered message)
        {
            var command = _mapper.Map<NewUserRegistered, AddUserCommand>(message);
            _mediator.Send(command).Wait();
        }
    }
}