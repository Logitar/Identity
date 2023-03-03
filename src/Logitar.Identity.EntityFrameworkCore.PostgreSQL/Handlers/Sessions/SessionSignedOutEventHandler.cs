using Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;
using Logitar.Identity.Sessions.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Handlers.Sessions;

/// <summary>
/// The handler for <see cref="SessionSignedOutEvent"/> events.
/// </summary>
internal class SessionSignedOutEventHandler : INotificationHandler<SessionSignedOutEvent>
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
  private readonly ILogger<SessionSignedOutEventHandler> _logger;

  /// <summary>
  /// Initializes a new instance of the <see cref="SessionSignedOutEventHandler"/> class with the specified arguments.
  /// </summary>
  /// <param name="actorService">The actor service.</param>
  /// <param name="context">The identity database context.</param>
  /// <param name="logger">The logger instance.</param>
  public SessionSignedOutEventHandler(IActorService actorService,
    IdentityContext context,
    ILogger<SessionSignedOutEventHandler> logger)
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
  public async Task Handle(SessionSignedOutEvent notification, CancellationToken cancellationToken)
  {
    try
    {
      SessionEntity? session = await _context.Sessions
        .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
      if (session == null)
      {
        _logger.LogError("The session 'AggregateId={id}' could not be found.", notification.AggregateId);
        return;
      }

      ActorEntity actor = await _actorService.GetActorAsync(notification.ActorId, cancellationToken);
      session.SignOut(notification, actor);
      await _context.SaveChangesAsync(cancellationToken);
    }
    catch (Exception exception)
    {
      _logger.LogError(exception);
    }
  }
}
