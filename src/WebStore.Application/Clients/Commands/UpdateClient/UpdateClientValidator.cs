﻿using FluentValidation;

namespace WebStore.Application.Clients.Commands.UpdateClient
{
    public sealed class UpdateClientValidator : AbstractValidator<UpdateClientRequest>
    {
        public UpdateClientValidator()
        {
            RuleFor(x => x.Dto.Name).NotEmpty();
            RuleFor(x => x.Dto.PhoneNumber).NotEmpty();
            RuleFor(x => x.Dto.Email).NotEmpty();
        }
    }
}