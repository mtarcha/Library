using System;
using AutoMapper;
using Library.Application.Commands.AddUser;
using Library.Infrastructure;
using MediatR;

namespace Library.Api.MessageHandlers
{
    public class NewUserRegistered
    {
        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }
    }

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