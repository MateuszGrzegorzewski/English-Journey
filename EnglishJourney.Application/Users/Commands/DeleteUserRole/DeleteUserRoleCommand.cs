﻿using MediatR;

namespace EnglishJourney.Application.Users.Commands.DeleteUserRole
{
    public class DeleteUserRoleCommand : IRequest
    {
        public string UserEmail { get; set; } = default!;
        public string RoleName { get; set; } = default!;
    }
}