using Logitar.Identity.ApiKeys.Events;
using Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Handlers.ApiKeys;

/// <summary>
/// The handler for <see cref="ApiKeyDeletedEvent"/> events.
/// </summary>
internal class ApiKeyDeletedEventHandler : INotificationHandler<ApiKeyDeletedEvent>
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
  private readonly ILogger<ApiKeyDeletedEventHandler> _logger;

  /// <summary>
  /// Initializes a new instance of the <see cref="ApiKeyDeletedEventHandler"/> class with the specified arguments.
  /// </summary>
  /// <param name="actorService">The actor service.</param>
  /// <param name="context">The identity database context.</param>
  /// <param name="logger">The logger instance.</param>
  public ApiKeyDeletedEventHandler(IActorService actorService,
    IdentityContext context,
    ILogger<ApiKeyDeletedEventHandler> logger)
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
  public async Task Handle(ApiKeyDeletedEvent notification, CancellationToken cancellationToken)
  {
    try
    {
      ApiKeyEntity? apiKey = await _context.ApiKeys
        .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
      if (apiKey == null)
      {
        _logger.LogError("The API key 'AggregateId={id}' could not be found.", notification.AggregateId);
        return;
      }

      _context.ApiKeys.Remove(apiKey);
      await _context.SaveChangesAsync(cancellationToken);

      await _actorService.DeleteAsync(apiKey, cancellationToken);
    }
    catch (Exception exception)
    {
      _logger.LogError(exception);
    }
  }
}
