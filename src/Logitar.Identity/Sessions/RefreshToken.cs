using Logitar.EventSourcing;
using System.Diagnostics.CodeAnalysis;

namespace Logitar.Identity.Sessions;

/// <summary>
/// Represents the data structure containing data of refresh tokens.
/// </summary>
public readonly struct RefreshToken
{
  /// <summary>
  /// The number of bytes in a <see cref="Guid"/>.
  /// </summary>
  private const int GuidByteCount = 16;

  /// <summary>
  /// Initializes a new instance of the <see cref="RefreshToken"/> struct using the specified arguments.
  /// </summary>
  /// <param name="id">The identifier of the user session.</param>
  /// <param name="key">The key of the user session.</param>
  public RefreshToken(Guid id, byte[] key)
  {
    if (id == Guid.Empty)
    {
      throw new ArgumentException("The value cannot be empty.", nameof(id));
    }
    if (key.Length == 0)
    {
      throw new ArgumentException("The value cannot be empty.", nameof(key));
    }

    Id = id;
    Key = key;
  }

  /// <summary>
  /// Gets the identifier of the user session.
  /// </summary>
  public Guid Id { get; }
  /// <summary>
  /// Gets the secret of the user session.
  /// </summary>
  public byte[] Key { get; }

  /// <summary>
  /// Parses the specified string as a refresh token.
  /// </summary>
  /// <param name="s">The string to parse.</param>
  /// <param name="prefix">The expected prefix.</param>
  /// <returns>The parsed refresn token.</returns>
  public static RefreshToken Parse(string s)
  {
    byte[] bytes = Convert.FromBase64String(s.FromUriSafeBase64());

    return new RefreshToken(new Guid(bytes.Take(GuidByteCount).ToArray()),
      bytes.Skip(GuidByteCount).ToArray());
  }
  /// <summary>
  /// Tries parsing the specified string as a refresh token.
  /// </summary>
  /// <param name="s">The string to parse.</param>
  /// <param name="refreshToken">The parsed refresh token.</param>
  /// <returns>A value indicating whether or not the refresh token could be parsed.</returns>
  public static bool TryParse(string s, out RefreshToken refreshToken)
  {
    try
    {
      refreshToken = Parse(s);

      return true;
    }
    catch (Exception)
    {
      refreshToken = default;

      return false;
    }
  }

  /// <summary>
  /// Returns a value indicating whether or not two refresh tokens are equal.
  /// </summary>
  /// <param name="x">The first refresh token.</param>
  /// <param name="y">The second refresh token.</param>
  /// <returns>True if the refresh tokens are equal.</returns>
  public static bool operator ==(RefreshToken x, RefreshToken y) => x.Equals(y);
  /// <summary>
  /// Returns a value indicating whether or not two refresh tokens are different.
  /// </summary>
  /// <param name="x">The first refresh token.</param>
  /// <param name="y">The second refresh token.</param>
  /// <returns>True if the refresh tokens are different.</returns>
  public static bool operator !=(RefreshToken x, RefreshToken y) => !x.Equals(y);

  /// <summary>
  /// Returns a value indicating whether or not the specified object is equal to the current refresh token.
  /// </summary>
  /// <param name="obj">The object to compare.</param>
  /// <returns>True if the object is equal to the current refresh token.</returns>
  public override bool Equals([NotNullWhen(true)] object? obj) => obj is RefreshToken refreshToken
    && refreshToken.Id == Id
    && refreshToken.Key.SequenceEqual(Key);
  /// <summary>
  /// Returns an integer representing the current refresh token hash code, derived from its values.
  /// </summary>
  /// <returns>The current refresh token hash code.</returns>
  public override int GetHashCode() => HashCode.Combine(Id, Key);
  /// <summary>
  /// Returns a string representing the current refresh token from its values.
  /// </summary>
  /// <returns>The string representation.</returns>
  public override string ToString() => Convert.ToBase64String(Id.ToByteArray().Concat(Key).ToArray()).ToUriSafeBase64();
}
