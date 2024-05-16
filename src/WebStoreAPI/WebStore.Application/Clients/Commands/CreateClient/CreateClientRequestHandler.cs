﻿using FluentResults;
using MediatR;
using WebStore.API.Infrastructure.Clients;
using WebStore.API.Domain;
using Microsoft.Extensions.Logging;

namespace WebStore.API.Application.Clients.Commands.CreateClient
{
    public class CreateClientRequestHandler(ILogger<IRequestHandler<RegisterClientRequest, Result<Guid>>> logger,
        IClientRepository clientRepository) : IRequestHandler<RegisterClientRequest, Result<Guid>>
    {
        private readonly IClientRepository _clientRepository = clientRepository;
        private readonly ILogger _logger = logger;

        public async Task<Result<Guid>> Handle(RegisterClientRequest request, CancellationToken cancellationToken)
        {
            var businessValidationResult = await ClientBusinessValidator.BusinessValidationAsync(_clientRepository, request);

            if (businessValidationResult.IsFailed)
            {
                _logger.LogDebug($"{request.Id} did not pass business validation.");

                return businessValidationResult;
            }

            var client = new Client { Id = request.Id, Email = request.Email, Name = request.Name, PhoneNumber = request.PhoneNumber, Orders = [] };

            await _clientRepository.AddClientAsync(client);

            return Result.Ok(client.Id);

        }
    }
}
