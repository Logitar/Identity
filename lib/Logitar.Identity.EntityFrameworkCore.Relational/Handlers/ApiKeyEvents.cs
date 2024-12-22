using Logitar.Identity.Core.ApiKeys.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers;

public sealed class ApiKeyEvents : INotificationHandler<ApiKeyAuthenticated>,
  INotificationHandler<ApiKeyCreated>,
  INotificationHandler<ApiKeyDeleted>,
  INotificationHandler<ApiKeyRoleAdded>,
  INotificationHandler<ApiKeyRoleRemoved>,
  INotificationHandler<ApiKeyUpdated>
{
  private readonly IdentityContext _context;

  public ApiKeyEvents(IdentityContext context)
  {
    _context = context;
  }

  public async Task Handle(ApiKeyAuthenticated @event, CancellationToken cancellationToken)
  {
    ApiKeyEntity apiKey = await _context.ApiKeys
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The API key entity 'StreamId={@event.StreamId}' could not be found.");

    apiKey.Authenticate(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(ApiKeyCreated @event, CancellationToken cancellationToken)
  {
    ApiKeyEntity? apiKey = await _context.ApiKeys.AsNoTracking()
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (apiKey == null)
    {
      apiKey = new(@event);

      _context.ApiKeys.Add(apiKey);

      await _context.SaveChangesAsync(cancellationToken); // TODO(fpion): save Actor
    }
  }

  public async Task Handle(ApiKeyDeleted @event, CancellationToken cancellationToken)
  {
    ApiKeyEntity? apiKey = await _context.ApiKeys
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (apiKey != null)
    {
      _context.ApiKeys.Remove(apiKey);

      await _context.SaveChangesAsync(cancellationToken); // TODO(fpion): delete Actor & CustomAttributes
    }
  }

  public async Task Handle(ApiKeyRoleAdded @event, CancellationToken cancellationToken)
  {
    ApiKeyEntity apiKey = await _context.ApiKeys
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The API key entity 'StreamId={@event.StreamId}' could not be found.");

    RoleEntity role = await _context.Roles
      .SingleOrDefaultAsync(x => x.StreamId == @event.RoleId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The role entity 'StreamId={@event.RoleId}' could not be found.");

    apiKey.AddRole(role, @event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(ApiKeyRoleRemoved @event, CancellationToken cancellationToken)
  {
    ApiKeyEntity apiKey = await _context.ApiKeys
      .Include(x => x.Roles)
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The API key entity 'StreamId={@event.StreamId}' could not be found.");

    apiKey.RemoveRole(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(ApiKeyUpdated @event, CancellationToken cancellationToken)
  {
    ApiKeyEntity apiKey = await _context.ApiKeys
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The API key entity 'StreamId={@event.StreamId}' could not be found.");

    apiKey.Update(@event);

    await _context.SaveChangesAsync(cancellationToken); // TODO(fpion): save Actor & CustomAttributes
  }
}
