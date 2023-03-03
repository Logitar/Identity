using MediatR;

namespace Logitar.Identity.Sessions.Queries;

/// <summary>
/// The query raised to retrieve a single user session.
/// </summary>
/// <param name="Id">The identifier of the user session.</param>
internal record GetSessionQuery(Guid? Id) : IRequest<Session?>;
