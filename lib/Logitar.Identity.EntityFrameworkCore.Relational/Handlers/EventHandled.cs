using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers;

public record EventHandled(DomainEvent Event) : INotification;
