namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// Describes a builder of error messages.
/// </summary>
public interface IErrorMessageBuilder
{
  /// <summary>
  /// Adds the specified data to the error message.
  /// </summary>
  /// <param name="key">The key of the data.</param>
  /// <param name="value">The value of the data.</param>
  /// <returns>The error message builder.</returns>
  IErrorMessageBuilder AddData(object key, object? value);

  /// <summary>
  /// Returns the error message.
  /// </summary>
  /// <returns>The built error message.</returns>
  string Build();
}
