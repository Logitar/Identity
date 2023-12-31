﻿using MediatR;

namespace Logitar.Identity.Sessions.Commands;

/// <summary>
/// The command raised to sign-in an user account.
/// </summary>
/// <param name="Input">The sign-in input data.</param>
internal record SignInCommand(SignInInput Input) : IRequest<Session>;
