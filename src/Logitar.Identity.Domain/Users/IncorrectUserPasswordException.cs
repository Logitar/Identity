﻿using Logitar.Identity.Domain.Shared;

namespace Logitar.Identity.Domain.Users;

public class IncorrectUserPasswordException : InvalidCredentialsException
{
  public const string BaseMessage = "The specified password does not match the specified user.";

  public string User
  {
    get => (string)Data[nameof(User)]!;
    private set => Data[nameof(User)] = value;
  }
  public string Password
  {
    get => (string)Data[nameof(Password)]!;
    private set => Data[nameof(Password)] = value;
  }

  public IncorrectUserPasswordException(UserAggregate user, string password) : base(BuildMessage(user, password))
  {
    User = user.ToString();
    Password = password;
  }

  private static string BuildMessage(UserAggregate user, string password) => new ErrorMessageBuilder(BaseMessage)
    .AddData(nameof(User), user.ToString())
    .AddData(nameof(Password), password)
    .Build();
}
