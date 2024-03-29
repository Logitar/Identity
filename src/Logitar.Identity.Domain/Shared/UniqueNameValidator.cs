﻿using FluentValidation;
using Logitar.Identity.Contracts.Settings;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The validator used to validate unique names.
/// See <see cref="UniqueNameUnit"/> for more information.
/// </summary>
public class UniqueNameValidator : AbstractValidator<string>
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UniqueNameValidator"/> class.
  /// </summary>
  /// <param name="uniqueNameSettings">The settings used to validate unique names.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public UniqueNameValidator(IUniqueNameSettings uniqueNameSettings, string? propertyName = null)
  {
    RuleFor(x => x).NotEmpty()
      .MaximumLength(UniqueNameUnit.MaximumLength)
      .SetValidator(new AllowedCharactersValidator(uniqueNameSettings.AllowedCharacters))
      .WithPropertyName(propertyName);
  }
}
