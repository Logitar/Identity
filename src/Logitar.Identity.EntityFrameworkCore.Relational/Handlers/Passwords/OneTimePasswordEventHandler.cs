using Logitar.EventSourcing;
using Logitar.Identity.Domain.Passwords.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.CustomAttributes;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers.Passwords;

public class OneTimePasswordEventHandler : IOneTimePasswordEventHandler
{
  protected const string EntityType = nameof(IdentityContext.OneTimePasswords);

  protected virtual IdentityContext Context { get; }
  protected virtual ICustomAttributeService CustomAttributes { get; }

  public OneTimePasswordEventHandler(IdentityContext context, ICustomAttributeService customAttributes)
  {
    Context = context;
    CustomAttributes = customAttributes;
  }

  public virtual async Task HandleAsync(OneTimePasswordCreatedEvent @event, CancellationToken cancellationToken)
  {
    OneTimePasswordEntity? oneTimePassword = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (oneTimePassword == null)
    {
      oneTimePassword = new(@event);

      Context.OneTimePasswords.Add(oneTimePassword);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(OneTimePasswordDeletedEvent @event, CancellationToken cancellationToken)
  {
    OneTimePasswordEntity? oneTimePassword = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (oneTimePassword != null)
    {
      Context.OneTimePasswords.Remove(oneTimePassword);

      await CustomAttributes.RemoveAsync(EntityType, oneTimePassword.OneTimePasswordId, cancellationToken);
      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(OneTimePasswordUpdatedEvent @event, CancellationToken cancellationToken)
  {
    OneTimePasswordEntity? oneTimePassword = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (oneTimePassword != null)
    {
      oneTimePassword.Update(@event);

      await CustomAttributes.SynchronizeAsync(EntityType, oneTimePassword.OneTimePasswordId, @event.CustomAttributes, cancellationToken);
      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(OneTimePasswordValidationFailedEvent @event, CancellationToken cancellationToken)
  {
    OneTimePasswordEntity? oneTimePassword = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (oneTimePassword != null)
    {
      oneTimePassword.Fail(@event);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(OneTimePasswordValidationSucceededEvent @event, CancellationToken cancellationToken)
  {
    OneTimePasswordEntity? oneTimePassword = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (oneTimePassword != null)
    {
      oneTimePassword.Succeed(@event);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  protected virtual async Task<OneTimePasswordEntity> LoadAsync(AggregateId aggregateId, CancellationToken cancellationToken)
  {
    return await TryLoadAsync(aggregateId, cancellationToken)
      ?? throw new InvalidOperationException($"The One-Time Password (OTP) entity 'AggregateId={aggregateId}' could not be found.");
  }
  protected virtual async Task<OneTimePasswordEntity?> TryLoadAsync(AggregateId aggregateId, CancellationToken cancellationToken)
  {
    return await Context.OneTimePasswords.SingleOrDefaultAsync(x => x.AggregateId == aggregateId.Value, cancellationToken);
  }
}
