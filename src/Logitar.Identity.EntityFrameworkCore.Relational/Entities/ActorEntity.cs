namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public class ActorEntity
{
  public int ActorId { get; private set; }

  public string Id { get; private set; } = string.Empty;
  public ActorType Type { get; private set; }
  public bool IsDeleted { get; private set; }

  public string DisplayName { get; private set; } = string.Empty;
  public string? EmailAddress { get; private set; }
  public string? PictureUrl { get; private set; }

  public ActorEntity(UserEntity user, bool isDeleted)
  {
    Id = user.AggregateId;
    Type = ActorType.User;

    Update(user, isDeleted);
  }

  private ActorEntity()
  {
  }

  public void Update(UserEntity user, bool isDeleted)
  {
    IsDeleted = isDeleted;

    DisplayName = user.FullName ?? user.UniqueName;
    EmailAddress = user.EmailAddress;
    PictureUrl = null; // TODO(fpion): PictureUrl
  }
}
