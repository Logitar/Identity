using MediatR;

namespace Logitar.Identity;

/// <summary>
/// Represents a pipeline allowing to perform tasks before and after executing a request.
/// </summary>
public class RequestPipeline : IRequestPipeline
{
  /// <summary>
  /// The mediator instance.
  /// </summary>
  private readonly IMediator _mediator;

  /// <summary>
  /// Initializes a new instance of the <see cref="RequestPipeline"/> class using the specified arguments.
  /// </summary>
  /// <param name="mediator">The mediator instance.</param>
  public RequestPipeline(IMediator mediator)
  {
    _mediator = mediator;
  }

  /// <summary>
  /// Executes the specified request and returns a response.
  /// </summary>
  /// <typeparam name="T">The type of the response.</typeparam>
  /// <param name="request">The request to execute.</param>
  /// <param name="cancellationToken">The cancellation token.</param>
  /// <returns>The response.</returns>
  public virtual async Task<T> ExecuteAsync<T>(IRequest<T> request, CancellationToken cancellationToken)
  {
    return await _mediator.Send(request, cancellationToken);
  }
}
