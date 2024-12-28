using Logitar.Identity.Core.Passwords.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers;

public sealed class OneTimePasswordEvents : INotificationHandler<OneTimePasswordCreated>,
  INotificationHandler<OneTimePasswordDeleted>,
  INotificationHandler<OneTimePasswordUpdated>,
  INotificationHandler<OneTimePasswordValidationFailed>,
  INotificationHandler<OneTimePasswordValidationSucceeded>
{
  private readonly IdentityContext _context;
  private readonly ICustomAttributeService _customAttributes;
  private readonly IMediator _mediator;

  public OneTimePasswordEvents(IdentityContext context, ICustomAttributeService customAttributes, IMediator mediator)
  {
    _context = context;
    _customAttributes = customAttributes;
    _mediator = mediator;
  }

  public async Task Handle(OneTimePasswordCreated @event, CancellationToken cancellationToken)
  {
    OneTimePasswordEntity? oneTimePassword = await _context.OneTimePasswords.AsNoTracking()
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);

    if (oneTimePassword == null)
    {
      oneTimePassword = new(@event);

      _context.OneTimePasswords.Add(oneTimePassword);

      await _context.SaveChangesAsync(cancellationToken);

      await _mediator.Publish(new EventHandled(@event), cancellationToken);
    }
    else
    {
      await _mediator.Publish(new EventNotHandled(@event, oneTimePassword), cancellationToken);
    }
  }

  public async Task Handle(OneTimePasswordDeleted @event, CancellationToken cancellationToken)
  {
    OneTimePasswordEntity? oneTimePassword = await _context.OneTimePasswords
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);

    if (oneTimePassword == null)
    {
      await _mediator.Publish(new EventNotHandled(@event, oneTimePassword), cancellationToken);
    }
    else
    {
      _context.OneTimePasswords.Remove(oneTimePassword);

      await _customAttributes.RemoveAsync(EntityType.OneTimePassword, oneTimePassword.OneTimePasswordId, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);

      await _mediator.Publish(new EventHandled(@event), cancellationToken);
    }
  }

  public async Task Handle(OneTimePasswordUpdated @event, CancellationToken cancellationToken)
  {
    OneTimePasswordEntity? oneTimePassword = await _context.OneTimePasswords
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);

    if (oneTimePassword == null || oneTimePassword.Version != (@event.Version - 1))
    {
      await _mediator.Publish(new EventNotHandled(@event, oneTimePassword), cancellationToken);
    }
    else
    {
      oneTimePassword.Update(@event);

      await _customAttributes.UpdateAsync(EntityType.OneTimePassword, oneTimePassword.OneTimePasswordId, @event.CustomAttributes, cancellationToken);
      await _context.SaveChangesAsync(cancellationToken);

      await _mediator.Publish(new EventHandled(@event), cancellationToken);
    }
  }

  public async Task Handle(OneTimePasswordValidationFailed @event, CancellationToken cancellationToken)
  {
    OneTimePasswordEntity? oneTimePassword = await _context.OneTimePasswords
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);

    if (oneTimePassword == null || oneTimePassword.Version != (@event.Version - 1))
    {
      await _mediator.Publish(new EventNotHandled(@event, oneTimePassword), cancellationToken);
    }
    else
    {
      oneTimePassword.Fail(@event);

      await _context.SaveChangesAsync(cancellationToken);

      await _mediator.Publish(new EventHandled(@event), cancellationToken);
    }
  }

  public async Task Handle(OneTimePasswordValidationSucceeded @event, CancellationToken cancellationToken)
  {
    OneTimePasswordEntity? oneTimePassword = await _context.OneTimePasswords
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);

    if (oneTimePassword == null || oneTimePassword.Version != (@event.Version - 1))
    {
      await _mediator.Publish(new EventNotHandled(@event, oneTimePassword), cancellationToken);
    }
    else
    {
      oneTimePassword.Succeed(@event);

      await _context.SaveChangesAsync(cancellationToken);

      await _mediator.Publish(new EventHandled(@event), cancellationToken);
    }
  }
}
