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

  public OneTimePasswordEvents(IdentityContext context)
  {
    _context = context;
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
    }
  }

  public async Task Handle(OneTimePasswordDeleted @event, CancellationToken cancellationToken)
  {
    OneTimePasswordEntity? oneTimePassword = await _context.OneTimePasswords
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken);
    if (oneTimePassword != null)
    {
      _context.OneTimePasswords.Remove(oneTimePassword);

      await _context.SaveChangesAsync(cancellationToken); // TODO(fpion): delete CustomAttributes
    }
  }

  public async Task Handle(OneTimePasswordUpdated @event, CancellationToken cancellationToken)
  {
    OneTimePasswordEntity oneTimePassword = await _context.OneTimePasswords
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The One-Time Password (OTP) entity 'StreamId={@event.StreamId}' could not be found.");

    oneTimePassword.Update(@event);

    await _context.SaveChangesAsync(cancellationToken); // TODO(fpion): save CustomAttributes
  }

  public async Task Handle(OneTimePasswordValidationFailed @event, CancellationToken cancellationToken)
  {
    OneTimePasswordEntity oneTimePassword = await _context.OneTimePasswords
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The One-Time Password (OTP) entity 'StreamId={@event.StreamId}' could not be found.");

    oneTimePassword.Fail(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }

  public async Task Handle(OneTimePasswordValidationSucceeded @event, CancellationToken cancellationToken)
  {
    OneTimePasswordEntity oneTimePassword = await _context.OneTimePasswords
      .SingleOrDefaultAsync(x => x.StreamId == @event.StreamId.Value, cancellationToken)
      ?? throw new InvalidOperationException($"The One-Time Password (OTP) entity 'StreamId={@event.StreamId}' could not be found.");

    oneTimePassword.Succeed(@event);

    await _context.SaveChangesAsync(cancellationToken);
  }
}
