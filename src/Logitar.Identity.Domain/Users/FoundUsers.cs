namespace Logitar.Identity.Domain.Users;

/// <summary>
/// The results of an user search.
/// </summary>
public record FoundUsers
{
  /// <summary>
  /// Gets or sets the user found by unique identifier.
  /// </summary>
  public UserAggregate? ById { get; set; }
  /// <summary>
  /// Gets or sets the user found by unique name.
  /// </summary>
  public UserAggregate? ByUniqueName { get; set; }
  /// <summary>
  /// Gets or sets the user found by email address.
  /// </summary>
  public UserAggregate? ByEmail { get; set; }

  /// <summary>
  /// Gets all found users.
  /// </summary>
  public IEnumerable<UserAggregate> All
  {
    get
    {
      List<UserAggregate> users = new(capacity: 3);

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
  public int Count => All.Count();

  /// <summary>
  /// Returns the first user found, ordered by unique identifier, then by unique name, and then by email address.
  /// </summary>
  /// <returns>The first user found.</returns>
  public UserAggregate First() => All.First();
  /// <summary>
  /// Returns the first user found, ordered by unique identifier, then by unique name, and then by email address.
  /// </summary>
  /// <returns>The first user found, or null if none were found.</returns>
  public UserAggregate? FirstOrDefault() => All.FirstOrDefault();

  /// <summary>
  /// Returns the single user found.
  /// </summary>
  /// <exception cref="InvalidOperationException">More than one users have been found.</exception>
  /// <returns>The single user found.</returns>
  public UserAggregate Single() => All.Single();
  /// <summary>
  /// Returns the single user found.
  /// </summary>
  /// <exception cref="InvalidOperationException">More than one users have been found.</exception>
  /// <returns>The single user found, or null if none were found.</returns>
  public UserAggregate? SingleOrDefault() => All.SingleOrDefault();
}
