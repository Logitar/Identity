using FluentValidation.Results;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The exception raised when a time zone is not valid.
/// </summary>
public class InvalidTimeZoneEntryException : Exception, IValidationException
{
  private const string ErrorMessage = "The specified time zone identifier did not resolve to a tz entry.";

  /// <summary>
  /// Gets or sets the invalid time zone identifier.
  /// </summary>
  public string Id
  {
    get => (string)Data[nameof(Id)]!;
    private set => Data[nameof(Id)] = value;
  }
  /// <summary>
  /// Gets or sets the name of the validated property.
  /// </summary>
  public string? PropertyName
  {
    get => (string?)Data[nameof(PropertyName)];
    private set => Data[nameof(PropertyName)] = value;
  }

  /// <summary>
  /// Gets the validation failure of the exception.
  /// </summary>
  public ValidationFailure Failure => new(PropertyName, ErrorMessage, Id)
  {
    ErrorCode = this.GetErrorCode()
  };

  /// <summary>
  /// Initializes a new instance of the <see cref="InvalidTimeZoneEntryException"/> class.
  /// </summary>
  /// <param name="id">The invalid time zone identifier.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public InvalidTimeZoneEntryException(string id, string? propertyName = null) : base(BuildMessage(id, propertyName))
  {
    Id = id;
    PropertyName = propertyName;
  }

  private static string BuildMessage(string id, string? propertyName) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(Id), id)
    .AddData(nameof(PropertyName), propertyName)
    .Build();
}
