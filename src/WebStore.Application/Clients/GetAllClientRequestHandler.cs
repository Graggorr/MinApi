﻿using FluentResults;
using MediatR;
using WebStore.Domain;

namespace WebStore.Application.Clients
{
    public class GetAllClientsRequestHandler(IClientRepository repository) : IRequestHandler<GetAllClientsHandlingRequest, Result<IEnumerable<Client>>>
    {
        private readonly IClientRepository _repository = repository;

        public async Task<Result<IEnumerable<Client>>> Handle(GetAllClientsHandlingRequest request, CancellationToken cancellationToken)
            => await _repository.GetAllClientsAsync();
    }
}