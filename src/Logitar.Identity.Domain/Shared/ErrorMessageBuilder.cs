namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// Represents a builder of error messages.
/// </summary>
public class ErrorMessageBuilder : IErrorMessageBuilder
{
  private readonly StringBuilder _message;

  /// <summary>
  /// Initializes a new instance of the <see cref="ErrorMessageBuilder"/> class.
  /// </summary>
  public ErrorMessageBuilder()
  {
    _message = new();
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ErrorMessageBuilder"/> class.
  /// </summary>
  /// <param name="message">An initial error message.</param>
  public ErrorMessageBuilder(string message)
  {
    _message = new(message);
  }

  /// <summary>
  /// Adds the specified data to the error message.
  /// </summary>
  /// <param name="key">The key of the data.</param>
  /// <param name="value">The value of the data.</param>
  /// <returns>The error message builder.</returns>
  public IErrorMessageBuilder AddData(object key, object? value)
  {
    if (_message.Length > 0)
    {
      _message.AppendLine();
    }

    _message.Append(key).Append(": ").Append(value);

    return this;
  }

  /// <summary>
  /// Returns the error message.
  /// </summary>
  /// <returns>The built error message.</returns>
  public string Build() => _message.ToString();
}
