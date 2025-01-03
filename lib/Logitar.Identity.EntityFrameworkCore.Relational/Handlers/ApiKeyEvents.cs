﻿using Logitar.Identity.Core.ApiKeys.Events;
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
  private readonly ICustomAttributeService _customAttributes;
  private readonly IMediator _mediator;

  public ApiKeyEvents(IdentityContext context, ICustomAttributeService customAttributes, IMediator mediator)
  {
    _context = context;
    _customAttributes = customAttributes;
    _mediator = mediator;
  }

  public async Task Handle(ApiKeyAuthenticated @event, CancellationToken cancellationToken)
  {
    ApiKeyEntity? apiKey = await _context.ApiKeys
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);

    if (apiKey == null || apiKey.Version != (@event.Version - 1))
    {
      await _mediator.Publish(new EventNotHandled(@event, apiKey), cancellationToken);
    }
    else
    {
      apiKey.Authenticate(@event);

      await _context.SaveChangesAsync(cancellationToken);

      await _mediator.Publish(new EventHandled(@event), cancellationToken);
    }
  }

  public async Task Handle(ApiKeyCreated @event, CancellationToken cancellationToken)
  {
    ApiKeyEntity? apiKey = await _context.ApiKeys.AsNoTracking()
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);

    if (apiKey == null)
    {
      apiKey = new(@event);

      _context.ApiKeys.Add(apiKey);

      await SaveActorAsync(apiKey, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);

      await _mediator.Publish(new EventHandled(@event), cancellationToken);
    }
    else
    {
      await _mediator.Publish(new EventNotHandled(@event, apiKey), cancellationToken);
    }
  }

  public async Task Handle(ApiKeyDeleted @event, CancellationToken cancellationToken)
  {
    ApiKeyEntity? apiKey = await _context.ApiKeys
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);

    if (apiKey == null)
    {
      await _mediator.Publish(new EventNotHandled(@event, apiKey), cancellationToken);
    }
    else
    {
      _context.ApiKeys.Remove(apiKey);

      await DeleteActorAsync(apiKey, cancellationToken);
      await _customAttributes.RemoveAsync(EntityType.ApiKey, apiKey.ApiKeyId, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);

      await _mediator.Publish(new EventHandled(@event), cancellationToken);
    }
  }

  public async Task Handle(ApiKeyRoleAdded @event, CancellationToken cancellationToken)
  {
    ApiKeyEntity? apiKey = await _context.ApiKeys
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);

    if (apiKey == null || apiKey.Version != (@event.Version - 1))
    {
      await _mediator.Publish(new EventNotHandled(@event, apiKey), cancellationToken);
    }
    else
    {
      RoleEntity role = await _context.Roles
        .SingleOrDefaultAsync(x => x.StreamId == @event.RoleId.Value, cancellationToken)
        ?? throw new InvalidOperationException($"The role entity 'StreamId={@event.RoleId}' could not be found.");

      apiKey.AddRole(role, @event);

      await _context.SaveChangesAsync(cancellationToken);

      await _mediator.Publish(new EventHandled(@event), cancellationToken);
    }
  }

  public async Task Handle(ApiKeyRoleRemoved @event, CancellationToken cancellationToken)
  {
    ApiKeyEntity? apiKey = await _context.ApiKeys
      .Include(x => x.Roles)
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);

    if (apiKey == null || apiKey.Version != (@event.Version - 1))
    {
      await _mediator.Publish(new EventNotHandled(@event, apiKey), cancellationToken);
    }
    else
    {
      apiKey.RemoveRole(@event);

      await _context.SaveChangesAsync(cancellationToken);

      await _mediator.Publish(new EventHandled(@event), cancellationToken);
    }
  }

  public async Task Handle(ApiKeyUpdated @event, CancellationToken cancellationToken)
  {
    ApiKeyEntity? apiKey = await _context.ApiKeys
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);

    if (apiKey == null || apiKey.Version != (@event.Version - 1))
    {
      await _mediator.Publish(new EventNotHandled(@event, apiKey), cancellationToken);
    }
    else
    {
      apiKey.Update(@event);

      await SaveActorAsync(apiKey, cancellationToken);
      await _customAttributes.UpdateAsync(EntityType.ApiKey, apiKey.ApiKeyId, @event.CustomAttributes, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);

      await _mediator.Publish(new EventHandled(@event), cancellationToken);
    }
  }

  private async Task DeleteActorAsync(ApiKeyEntity apiKey, CancellationToken cancellationToken)
  {
    await SaveActorAsync(apiKey, isDeleted: true, cancellationToken);
  }
  private async Task SaveActorAsync(ApiKeyEntity apiKey, CancellationToken cancellationToken)
  {
    await SaveActorAsync(apiKey, isDeleted: false, cancellationToken);
  }
  private async Task SaveActorAsync(ApiKeyEntity apiKey, bool isDeleted, CancellationToken cancellationToken)
  {
    ActorEntity? actor = await _context.Actors.SingleOrDefaultAsync(x => x.Id == apiKey.StreamId, cancellationToken);
    if (actor == null)
    {
      actor = new()
      {
        Id = apiKey.StreamId,
        Type = ActorType.ApiKey
      };
      _context.Actors.Add(actor);
    }

    actor.IsDeleted = isDeleted;

    actor.DisplayName = apiKey.DisplayName;
  }
}
