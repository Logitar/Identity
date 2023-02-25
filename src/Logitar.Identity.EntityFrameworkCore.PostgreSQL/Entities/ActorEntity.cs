using Logitar.Identity.Actors;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;

/// <summary>
/// The database model representing an actor.
/// </summary>
public class ActorEntity
{
  /// <summary>
  /// The actor serialization options.
  /// </summary>
  private static readonly JsonSerializerOptions _serializerOptions = new();

  /// <summary>
  /// Initializes the static instance of the <see cref="ActorEntity"/> class.
  /// </summary>
  static ActorEntity()
  {
    _serializerOptions.Converters.Add(new JsonStringEnumConverter());
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ActorEntity"/> class.
  /// </summary>
  public ActorEntity()
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ActorEntity"/> class using the specified actor.
  /// </summary>
  /// <param name="actor">The actor representation.</param>
  public ActorEntity(Actor actor)
  {
    Type = actor.Type;
    IsDeleted = actor.IsDeleted;

    DisplayName = actor.DisplayName;
    Email = actor.Email;
    Picture = actor.Picture;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ActorEntity"/> class using the specified API key actor.
  /// </summary>
  /// <param name="apiKey">The API key actor.</param>
  /// <param name="isDeleted">A value indicating whether or not the actor is deleted.</param>
  internal ActorEntity(ApiKeyEntity apiKey, bool isDeleted = false)
  {
    Type = ActorType.ApiKey;
    IsDeleted = isDeleted;

    DisplayName = apiKey.Title;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ActorEntity"/> class using the specified user actor.
  /// </summary>
  /// <param name="user">The user actor.</param>
  /// <param name="isDeleted">A value indicating whether or not the actor is deleted.</param>
  internal ActorEntity(UserEntity user, bool isDeleted = false)
  {
    Type = ActorType.ApiKey;
    IsDeleted = isDeleted;

    DisplayName = user.FullName ?? user.Username;
    Email = user.EmailAddress;
    Picture = user.Picture;
  }

  /// <summary>
  /// Gets or sets the type of the actor.
  /// </summary>
  public ActorType Type { get; set; }
  /// <summary>
  /// Gets or sets a value indicating whether or not the actor is deleted.
  /// </summary>
  public bool IsDeleted { get; set; }

  /// <summary>
  /// Gets or sets the display name of the actor.
  /// </summary>
  public string DisplayName { get; set; } = string.Empty;
  /// <summary>
  /// Gets or sets the email address of the actor.
  /// </summary>
  public string? Email { get; set; }
  /// <summary>
  /// Gets or sets a link (URL) to the picture of the user.
  /// </summary>
  public string? Picture { get; set; }

  /// <summary>
  /// Deserializes an actor entity from the specified JSON string.
  /// </summary>
  /// <param name="json">The JSON serialized actor.</param>
  /// <param name="id">The identifier of the actor.</param>
  /// <returns>The deserialized actor entity.</returns>
  /// <exception cref="InvalidOperationException">The actor could not be deserialized.</exception>
  public static ActorEntity Deserialize(string json, string id)
  {
    return JsonSerializer.Deserialize<ActorEntity>(json, _serializerOptions)
      ?? throw new InvalidOperationException($"The actor 'Id={id}' ('{json}') could not be deserialized.");
  }

  /// <summary>
  /// Serializes the current actor in JSON.
  /// </summary>
  /// <returns>The JSON serialized actor.</returns>
  public string Serialize() => JsonSerializer.Serialize(this, _serializerOptions);
}
