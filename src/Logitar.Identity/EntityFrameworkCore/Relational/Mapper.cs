using Logitar.EventSourcing;
using Logitar.Identity.Contracts;
using Logitar.Identity.Contracts.Actors;
using Logitar.Identity.Contracts.Sessions;
using Logitar.Identity.Contracts.Users;
using Logitar.Identity.EntityFrameworkCore.Relational.Entities;

namespace Logitar.Identity.EntityFrameworkCore.Relational;

internal class Mapper
{
  private readonly Dictionary<ActorId, Actor> _actors = [];
  private readonly Actor _system = new();

  public Mapper()
  {
  }

  public Mapper(IEnumerable<Actor> actors)
  {
    foreach (Actor actor in actors)
    {
      ActorId id = new(actor.Id);
      _actors[id] = actor;
    }
  }

  public virtual Actor ToActor(ActorEntity source) => new()
  {
    Id = source.Id,
    Type = source.Type,
    IsDeleted = source.IsDeleted,
    DisplayName = source.DisplayName,
    EmailAddress = source.EmailAddress,
    PictureUrl = source.PictureUrl
  };

  public Session ToSession(SessionEntity source) => ToSession(source, user: null);
  public Session ToSession(SessionEntity source, User? user)
  {
    Session destination = new()
    {
      Id = new AggregateId(source.AggregateId).ToGuid(),
      IsActive = source.IsActive
    };

    if (user != null)
    {
      destination.User = user;
    }
    else if (source.User != null)
    {
      Session[] sessions = [destination];
      destination.User = ToUser(source.User, sessions);
    }

    MapAggregate(source, destination);

    return destination;
  }

  public User ToUser(UserEntity source) => ToUser(source, sessions: null);
  public User ToUser(UserEntity source, IEnumerable<Session>? sessions)
  {
    User destination = new();

    if (sessions != null)
    {
      destination.Sessions = sessions.ToList();
    }
    else
    {
      destination.Sessions = source.Sessions.Select(session => ToSession(session, destination)).ToList();
    }

    MapAggregate(source, destination);

    return destination;
  }

  private static DateTime AsUniversalTime(DateTime value) => DateTime.SpecifyKind(value, DateTimeKind.Utc);

  private Actor FindActor(string id) => FindActor(new ActorId(id));
  private Actor FindActor(ActorId id) => _actors.TryGetValue(id, out Actor? actor) ? actor : _system;

  private void MapAggregate(AggregateEntity source, Aggregate destination)
  {
    destination.Version = source.Version;
    destination.CreatedBy = FindActor(source.CreatedBy);
    destination.CreatedOn = AsUniversalTime(source.CreatedOn);
    destination.UpdatedBy = FindActor(source.UpdatedBy);
    destination.UpdatedOn = AsUniversalTime(source.UpdatedOn);
  }
}
