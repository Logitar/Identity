using Logitar.EventSourcing;
using Logitar.Identity.Domain.ApiKeys.Events;
using Logitar.Identity.EntityFrameworkCore.Relational.CustomAttributes;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;
using Logitar.Identity.Infrastructure.Handlers;
using Microsoft.EntityFrameworkCore;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Handlers;

public class ApiKeyEventHandler : IApiKeyEventHandler
{
  protected const string EntityType = nameof(IdentityContext.ApiKeys);

  protected virtual IdentityContext Context { get; }
  protected virtual ICustomAttributeService CustomAttributes { get; }

  public ApiKeyEventHandler(IdentityContext context, ICustomAttributeService customAttributes)
  {
    Context = context;
    CustomAttributes = customAttributes;
  }

  public virtual async Task HandleAsync(ApiKeyAuthenticatedEvent @event, CancellationToken cancellationToken)
  {
    var apiKey = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (apiKey != null)
    {
      apiKey.Authenticate(@event);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(ApiKeyCreatedEvent @event, CancellationToken cancellationToken)
  {
    var apiKey = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (apiKey == null)
    {
      apiKey = new(@event);

      Context.ApiKeys.Add(apiKey);

      await SaveActorAsync(apiKey, cancellationToken);
      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(ApiKeyDeletedEvent @event, CancellationToken cancellationToken)
  {
    var apiKey = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (apiKey != null)
    {
      Context.ApiKeys.Remove(apiKey);

      await DeleteActorAsync(apiKey, cancellationToken);
      await CustomAttributes.RemoveAsync(EntityType, apiKey.ApiKeyId, cancellationToken);
      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(ApiKeyRoleAddedEvent @event, CancellationToken cancellationToken)
  {
    var apiKey = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (apiKey != null)
    {
      var role = await Context.Roles
        .SingleOrDefaultAsync(x => x.AggregateId == @event.RoleId.AggregateId.Value, cancellationToken)
        ?? throw new InvalidOperationException($"The role entity 'AggregateId={@event.AggregateId}' could not be found.");

      apiKey.AddRole(role, @event);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(ApiKeyRoleRemovedEvent @event, CancellationToken cancellationToken)
  {
    var apiKey = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (apiKey != null)
    {
      apiKey.RemoveRole(@event);

      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  public virtual async Task HandleAsync(ApiKeyUpdatedEvent @event, CancellationToken cancellationToken)
  {
    var apiKey = await TryLoadAsync(@event.AggregateId, cancellationToken);
    if (apiKey != null)
    {
      apiKey.Update(@event);

      await CustomAttributes.SynchronizeAsync(EntityType, apiKey.ApiKeyId, @event.CustomAttributes, cancellationToken);
      await SaveActorAsync(apiKey, cancellationToken);
      await Context.SaveChangesAsync(cancellationToken);
    }
  }

  protected virtual async Task<ApiKeyEntity> LoadAsync(AggregateId aggregateId, CancellationToken cancellationToken)
  {
    return await TryLoadAsync(aggregateId, cancellationToken)
      ?? throw new InvalidOperationException($"The API key entity 'AggregateId={aggregateId}' could not be found.");
  }
  protected virtual async Task<ApiKeyEntity?> TryLoadAsync(AggregateId aggregateId, CancellationToken cancellationToken)
  {
    return await Context.ApiKeys
      .Include(x => x.Roles)
      .SingleOrDefaultAsync(x => x.AggregateId == aggregateId.Value, cancellationToken);
  }

  protected virtual async Task DeleteActorAsync(ApiKeyEntity apiKey, CancellationToken cancellationToken)
    => await SaveActorAsync(apiKey, isDeleted: true, cancellationToken);
  protected virtual async Task SaveActorAsync(ApiKeyEntity apiKey, CancellationToken cancellationToken)
    => await SaveActorAsync(apiKey, isDeleted: false, cancellationToken);
  protected virtual async Task SaveActorAsync(ApiKeyEntity apiKey, bool isDeleted, CancellationToken cancellationToken)
  {
    var actor = await Context.Actors
      .SingleOrDefaultAsync(x => x.Id == apiKey.AggregateId, cancellationToken);

    if (actor == null)
    {
      actor = new()
      {
        Id = apiKey.AggregateId,
        Type = ActorType.ApiKey
      };
      Context.Actors.Add(actor);
    }

    actor.IsDeleted = isDeleted;

    actor.DisplayName = apiKey.DisplayName;
  }
}
