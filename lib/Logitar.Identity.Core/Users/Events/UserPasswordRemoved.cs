﻿using Logitar.EventSourcing;
using MediatR;

namespace Logitar.Identity.Core.Users.Events;

/// <summary>
/// The event raised when the password of an user is removed.
/// </summary>
public record UserPasswordRemoved : DomainEvent, INotification;
