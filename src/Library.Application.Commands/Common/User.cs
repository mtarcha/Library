﻿using System;

namespace Library.Application.Commands.Common
{
    public class User
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }
    }
}