﻿using Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;
using Logitar.Identity.Users.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Handlers.Users;

/// <summary>
/// The handler for <see cref="UserUpdatedEvent"/> events.
/// </summary>
internal class UserUpdatedEventHandler : INotificationHandler<UserUpdatedEvent>
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
  private readonly ILogger<UserUpdatedEventHandler> _logger;

  /// <summary>
  /// Initializes a new instance of the <see cref="UserUpdatedEventHandler"/> class with the specified arguments.
  /// </summary>
  /// <param name="actorService">The actor service.</param>
  /// <param name="context">The identity database context.</param>
  /// <param name="logger">The logger instance.</param>
  public UserUpdatedEventHandler(IActorService actorService,
    IdentityContext context,
    ILogger<UserUpdatedEventHandler> logger)
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
  public async Task Handle(UserUpdatedEvent notification, CancellationToken cancellationToken)
  {
    try
    {
      UserEntity? user = await _context.Users
        .Include(x => x.Roles)
        .SingleOrDefaultAsync(x => x.AggregateId == notification.AggregateId.Value, cancellationToken);
      if (user == null)
      {
        _logger.LogError("The user 'AggregateId={id}' could not be found.", notification.AggregateId);
        return;
      }

      bool updateActors = user.FullName != notification.FullName
        || user.EmailAddress != notification.Email?.Address
        || user.Picture != notification.Picture;

      ActorEntity actor = await _actorService.GetActorAsync(notification.ActorId, cancellationToken);
      user.Update(notification, actor);

      user.Roles.Clear();
      if (notification.Roles.Any())
      {
        IEnumerable<string> roleIds = notification.Roles.Select(id => id.Value);
        RoleEntity[] roles = await _context.Roles.Where(x => roleIds.Contains(x.AggregateId))
          .ToArrayAsync(cancellationToken);
        user.Roles.AddRange(roles);
      }

      await _context.SaveChangesAsync(cancellationToken);

      if (updateActors)
      {
        await _actorService.UpdateAsync(user, cancellationToken);
      }
    }
    catch (Exception exception)
    {
      _logger.LogError(exception);
    }
  }
}
