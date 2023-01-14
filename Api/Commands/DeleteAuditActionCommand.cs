using MediatR;
using System;

namespace Api.Commands;

public sealed record DeleteAuditActionCommand(Guid ActionId) : IRequest;
