namespace Logitar.Identity.Core.Users;

/// <summary>
/// The results of a user search.
/// </summary>
public record FoundUsers
{
  /// <summary>
  /// Gets or sets the user found by unique identifier.
  /// </summary>
  public User? ById { get; set; }
  /// <summary>
  /// Gets or sets the user found by unique name.
  /// </summary>
  public User? ByUniqueName { get; set; }
  /// <summary>
  /// Gets or sets the user found by email address.
  /// </summary>
  public User? ByEmail { get; set; }

  /// <summary>
  /// Gets all found users.
  /// </summary>
  public IReadOnlyCollection<User> All
  {
    get
    {
      List<User> users = new(capacity: 3);
      if (ById != null)
      {
        users.Add(ById);
      }
      if (ByUniqueName != null)
      {
        users.Add(ByUniqueName);
      }
      if (ByEmail != null)
      {
        users.Add(ByEmail);
      }
      return users.AsReadOnly();
    }
  }

  /// <summary>
  /// Returns the number of found users.
  /// </summary>
  public int Count => All.Count;

  /// <summary>
  /// Returns the first user found, ordered by unique identifier, then by unique name, and then by email address.
  /// </summary>
  /// <returns>The first user found.</returns>
  public User First() => All.First();
  /// <summary>
  /// Returns the first user found, ordered by unique identifier, then by unique name, and then by email address.
  /// </summary>
  /// <returns>The first user found, or null if none were found.</returns>
  public User? FirstOrDefault() => All.FirstOrDefault();

  /// <summary>
  /// Returns the single user found.
  /// </summary>
  /// <exception cref="InvalidOperationException">More than one users have been found.</exception>
  /// <returns>The single user found.</returns>
  public User Single() => All.Single();
  /// <summary>
  /// Returns the single user found.
  /// </summary>
  /// <exception cref="InvalidOperationException">More than one users have been found.</exception>
  /// <returns>The single user found, or null if none were found.</returns>
  public User? SingleOrDefault() => All.SingleOrDefault();
}
