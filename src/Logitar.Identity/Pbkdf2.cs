﻿using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Logitar.Identity;

/// <summary>
/// Represents an instance of a PBKDF2 methods. This class contains methods to salt and hash
/// passwords using the PBKDF2 as well as methods to compare passwords.
/// </summary>
internal class Pbkdf2
{
  /// <summary>
  /// The default algorithm used to hash the password.
  /// </summary>
  private const KeyDerivationPrf DefaultAlgorithm = KeyDerivationPrf.HMACSHA256;
  /// <summary>
  /// The default number of cycles the hashing algorithm performs.
  /// </summary>
  private const int DefaultIterationCount = 100000;
  /// <summary>
  /// The default number of bytes in the salt.
  /// </summary>
  private const int DefaultSaltLength = 32;
  /// <summary>
  /// The separator character used to separate values in the string representation.
  /// </summary>
  private const char Separator = '.';

  /// <summary>
  /// The algorithm used to hash the password.
  /// </summary>
  private readonly KeyDerivationPrf _algorithm = DefaultAlgorithm;
  /// <summary>
  /// The algorithm iteration count.
  /// </summary>
  private readonly int _iterationCount = DefaultIterationCount;
  /// <summary>
  /// The password salt.
  /// </summary>
  private readonly byte[] _salt = RandomNumberGenerator.GetBytes(DefaultSaltLength);
  /// <summary>
  /// The password hash.
  /// </summary>
  private readonly byte[] _hash;

  /// <summary>
  /// Initializes a new instance of the <see cref="Pbkdf2"/> class using the specified password.
  /// </summary>
  /// <param name="password">The password to salt and hash.</param>
  public Pbkdf2(string password)
  {
    _hash = ComputeHash(password);
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="Pbkdf2"/> class using the specified arguments.
  /// </summary>
  /// <param name="algorithm">The algorithm used to hash the password.</param>
  /// <param name="iterationCount">The algorithm iteration count.</param>
  /// <param name="salt">The password salt.</param>
  /// <param name="hash">The password hash.</param>
  private Pbkdf2(KeyDerivationPrf algorithm, int iterationCount, byte[] salt, byte[] hash)
  {
    _algorithm = algorithm;
    _iterationCount = iterationCount;
    _salt = salt;
    _hash = hash;
  }

  /// <summary>
  /// Parses the specified PBKDF2 string representation.
  /// </summary>
  /// <param name="s">The string representation.</param>
  /// <returns>The parsed PBKDF2 instance.</returns>
  /// <exception cref="ArgumentException">The string is not a valid PBKDF2 string.</exception>
  public static Pbkdf2 Parse(string s)
  {
    string[] values = s.Split(Separator);
    if (values.Length != 4)
    {
      throw new ArgumentException($"The value '{s}' is not a valid PBKDF2 string.", nameof(s));
    }

    return new Pbkdf2(Enum.Parse<KeyDerivationPrf>(values[0]),
      int.Parse(values[1]),
      Convert.FromBase64String(values[2]),
      Convert.FromBase64String(values[3]));
  }
  /// <summary>
  /// Tries parsing the specified PBKDF2 string representation.
  /// </summary>
  /// <param name="s">The string representation.</param>
  /// <param name="pbkdf2">The parsed PBKDF2 instance.</param>
  /// <returns>A value indicating whether or not the PBKDF2 string could be parsed.</returns>
  public static bool TryParse(string s, out Pbkdf2? pbkdf2)
  {
    try
    {
      pbkdf2 = Parse(s);

      return true;
    }
    catch (Exception)
    {
      pbkdf2 = null;

      return false;
    }
  }
  /// <summary>
  /// Returns a value indicating whether or not the specified password did match this PBKDF2 instance.
  /// </summary>
  /// <param name="password">The password to match.</param>
  /// <returns>True if the password did match.</returns>
  public bool IsMatch(string password)
  {
    return _hash.SequenceEqual(ComputeHash(password, _hash.Length));
  }

  /// <summary>
  /// Computes the hash of a password using this instance's algorithm, iteration count and salt.
  /// </summary>
  /// <param name="password">The password to hash.</param>
  /// <param name="length">The number of bytes to return; if null, the number of bytes in the salt will be used.</param>
  /// <returns>The password hash.</returns>
  private byte[] ComputeHash(string password, int? length = null)
  {
    return KeyDerivation.Pbkdf2(password, _salt, _algorithm, _iterationCount, length ?? _salt.Length);
  }

  /// <summary>
  /// Returns a value indicating whether or not the specified object is equal to the current PBKDF2 password.
  /// </summary>
  /// <param name="obj">The object to compare.</param>
  /// <returns>True if the object is equal to the current PBKDF2 password.</returns>
  public override bool Equals(object? obj) => obj is Pbkdf2 p && p._algorithm == _algorithm
    && p._iterationCount == _iterationCount
    && p._salt.SequenceEqual(_salt)
    && p._hash.SequenceEqual(_hash);
  /// <summary>
  /// Returns an integer correspond to the hash code of the current PBKDF2 password.
  /// </summary>
  /// <returns>The hash code derived from the PBKDF2 password values.</returns>
  public override int GetHashCode() => HashCode.Combine(_algorithm, _iterationCount, _salt, _hash);
  /// <summary>
  /// Returns a string representation of the current PBKDF2 password.
  /// </summary>
  /// <returns>The joined values of the PBKDF2 password.</returns>
  public override string ToString() => string.Join(Separator,
    _algorithm,
    _iterationCount,
    Convert.ToBase64String(_salt),
    Convert.ToBase64String(_hash));
}
