using Logitar.Identity.ApiKeys.Events;
using Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Handlers.ApiKeys;

/// <summary>
/// The handler for <see cref="ApiKeyUpdatedEvent"/> events.
/// </summary>
internal class ApiKeyUpdatedEventHandler : INotificationHandler<ApiKeyUpdatedEvent>
{
  /// <summary>
  /// The actor service.
  /// </summary>
  private readonly IActorService _actorService;
  /// <summary>
  /// The identity database context.
  /// </summary>
  private readonly IdentityContext _context;
  /// <summary>
  /// The logger instance.
  /// </summary>
  private readonly ILogger<ApiKeyUpdatedEventHandler> _logger;

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyUpdatedEventHandler"/> class with the specified arguments.
  /// </summary>
  /// <param name="actorService">The actor service.</param>
  /// <param name="context">The identity database context.</param>
  /// <param name="logger">The logger instance.</param>
  public ApiKeyUpdatedEventHandler(IActorService actorService,
    IdentityContext context,
    ILogger<ApiKeyUpdatedEventHandler> logger)
  {
    _actorService = actorService;
    _context = context;
    _logger = logger;
  }

  /// <summary>
  /// Handles the specified event.
  /// </summary>
  /// <param name="notification">The event to handle.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The asynchronous operation.</returns>
  public async Task Handle(ApiKeyUpdatedEvent notification, CancellationToken cancellationToken)
  {
    try
    {
      ApiKeyEntity? apiKey = await _context.ApiKeys
        .Include(x => x.Roles)
        .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
      if (apiKey == null)
      {
        _logger.LogError("The API key 'AggregateId={id}' could not be found.", notification.AggregateId);
        return;
      }

      bool updateActors = apiKey.Title != notification.Title;

      ActorEntity actor = await _actorService.GetActorAsync(notification.ActorId, cancellationToken);
      apiKey.Update(notification, actor);

      apiKey.Roles.Clear();
      if (notification.Roles.Any())
      {
        IEnumerable<string> roleIds = notification.Roles.Select(id => id.Value).Distinct();

        RoleEntity[] roles = await _context.Roles
          .Where(x => roleIds.Contains(x.AggregateId))
          .ToArrayAsync(cancellationToken);

        apiKey.Roles.AddRange(roles);
      }

      await _context.SaveChangesAsync(cancellationToken);

      if (updateActors)
      {
        await _actorService.UpdateAsync(apiKey, cancellationToken);
      }
    }
    catch (Exception exception)
    {
      _logger.LogError(exception);
    }
  }
}
