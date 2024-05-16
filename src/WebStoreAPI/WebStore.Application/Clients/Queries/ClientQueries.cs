﻿using FluentResults;
using MediatR;
using WebStore.API.Domain;

namespace WebStore.API.Application.Clients.Queries;

public record GetClientHandlingRequest(Guid Id) : IRequest<Result<Client>>;
public record GetPaginatedClientsRequest(int Page) : IRequest<Result<IEnumerable<Client>>>;