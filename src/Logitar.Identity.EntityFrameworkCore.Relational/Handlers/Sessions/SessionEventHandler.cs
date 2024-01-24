using Logitar.EventSourcing;
using Logitar.Identity.Domain.Sessions.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.CustomAttributes;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Sessions;

public class SessionEventHandler : ISessionEventHandler
{
  protected const string EntityType = nameof(IdentityContext.Sessions);

  protected virtual IdentityContext Context { get; }
  protected virtual ICustomAttributeService CustomAttributes { get; }

  public SessionEventHandler(IdentityContext context, ICustomAttributeService customAttributes)
  {
    Context = context;
    CustomAttributes = customAttributes;
  }

  public virtual async Task HandleAsync(SessionCreatedEvent @event, CancellationToken cancellationToken)
  {
    SessionEntity? session = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (session == null)
    {
      UserEntity user = await Context.Users
        .SingleOrDefaultAsync(x => x.AggregateId == @event.UserId.Value, cancellationToken)
        ?? throw new InvalidOperationException($"The user entity 'AggregateId={@event.AggregateId}' could not be found.");

      session = new(user, @event);
      user.Sessions.Add(session);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(SessionDeletedEvent @event, CancellationToken cancellationToken)
  {
    SessionEntity? session = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (session != null)
    {
      Context.Sessions.Remove(session);
      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(SessionRenewedEvent @event, CancellationToken cancellationToken)
  {
    SessionEntity? session = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (session != null)
    {
      session.Renew(@event);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(SessionSignedOutEvent @event, CancellationToken cancellationToken)
  {
    SessionEntity? session = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (session != null)
    {
      session.SignOut(@event);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(SessionUpdatedEvent @event, CancellationToken cancellationToken)
  {
    SessionEntity? session = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (session != null)
    {
      session.Update(@event);

      await CustomAttributes.SynchronizeAsync(EntityType, session.SessionId, @event.CustomAttributes, cancellationToken);
      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  protected virtual async Task<SessionEntity> LoadAsync(AggregateId aggregateId, CancellationToken cancellationToken)
  {
    return await TryLoadAsync(aggregateId, cancellationToken)
      ?? throw new InvalidOperationException($"The session entity 'AggregateId={aggregateId}' could not be found.");
  }
  protected virtual async Task<SessionEntity?> TryLoadAsync(AggregateId aggregateId, CancellationToken cancellationToken)
  {
    return await Context.Sessions
      .SingleOrDefaultAsync(x => x.AggregateId == aggregateId.Value, cancellationToken);
  }
}
