﻿using FluentValidation;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The validator used to validate custom attribute keys.
/// See <see cref="CustomAttributeValidator"/> for more information.
/// </summary>
public class CustomAttributeKeyValidator : AbstractValidator<string>
{
  /// <summary>
  /// The maximum length of custom attribute keys.
  /// </summary>
  public const int MaximumLength = byte.MaxValue;

  /// <summary>
  /// Initializes a new instance of the <see cref="CustomAttributeKeyValidator"/> class.
  /// </summary>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public CustomAttributeKeyValidator(string? propertyName = null)
  {
    RuleFor(x => x).NotEmpty()
      .MaximumLength(MaximumLength)
      .Identifier()
      .WithPropertyName(propertyName);
  }
}
