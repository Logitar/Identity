using FluentValidation.Results;

namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The exception raised when an association between two entities in different tenants is made.
/// </summary>
public class TenantMismatchException : Exception, IFailureException
{
  private const string ErrorMessage = "The specified tenant identifier was not expected.";

  /// <summary>
  /// Gets or sets the expected tenant identifier.
  /// </summary>
  public TenantId? ExpectedTenantId
  {
    get
    {
      string? value = Data[nameof(ExpectedTenantId)] as string;
      return value == null ? null : new TenantId(value);
    }
    private set => Data[nameof(ExpectedTenantId)] = value?.Value;
  }
  /// <summary>
  /// Gets or sets the actual tenant identifier.
  /// </summary>
  public TenantId? ActualTenantId
  {
    get
    {
      string? value = Data[nameof(ActualTenantId)] as string;
      return value == null ? null : new TenantId(value);
    }
    private set => Data[nameof(ActualTenantId)] = value?.Value;
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
  public ValidationFailure Failure => new(PropertyName, ErrorMessage, ActualTenantId)
  {
    ErrorCode = this.GetErrorCode(),
    CustomState = new
    {
      ExpectedValue = ExpectedTenantId
    }
  };

  /// <summary>
  /// Initializes a new instance of the <see cref="TenantMismatchException"/> class.
  /// </summary>
  /// <param name="expected">The expected tenant identifier.</param>
  /// <param name="actual">The actual tenant identifier.</param>
  /// <param name="propertyName">The name of the property, used for validation.</param>
  public TenantMismatchException(TenantId? expected, TenantId? actual, string? propertyName = null)
    : base(BuildMessage(expected, actual, propertyName))
  {
    ExpectedTenantId = expected;
    ActualTenantId = actual;
    PropertyName = propertyName;
  }

  private static string BuildMessage(TenantId? expected, TenantId? actual, string? propertyName) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(ExpectedTenantId), expected?.Value)
    .AddData(nameof(ActualTenantId), actual?.Value)
    .AddData(nameof(PropertyName), propertyName)
    .Build();
}
