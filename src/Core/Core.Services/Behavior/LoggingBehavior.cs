using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Core.Services.Behavior;

public class LoggingBehavior<TRequest> : IRequestPreProcessor<TRequest>
{
    private readonly ILogger _logger;

    public LoggingBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("Net6WebApiTemplate Request: {Name} {@Request}", requestName, request);

        return Task.CompletedTask;
    }
}
