using System.Diagnostics;
using CSharpEssentials.Json;
using MediatR;
using CompanyName.BuildingBlocks.Application.Abstractions.Contracts;
using Microsoft.Extensions.Logging;

namespace CompanyName.BuildingBlocks.Application.PipelineBehaviors;
public sealed class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ILoggableRequest
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        string responseName = typeof(TResponse).Name;
        long startTime = Stopwatch.GetTimestamp();
        LogRequest(requestName, request);
        TResponse response = await next();
        LogResponse(responseName, response);
        TimeSpan elapsedTime = Stopwatch.GetElapsedTime(startTime);
        logger.LogInformation("Request {RequestName} took {ElapsedTime}", requestName, elapsedTime);
        return response;
    }

    private void LogRequest(string requestName, TRequest request)
    {
        if (request is not IRequestLoggable)
        {
            logger.LogInformation("Handling {RequestName} request", requestName);
            return;
        }
        string requestJson = request.ConvertToJson();
        logger.LogInformation("Handling {RequestName} request: {Request}", requestName, requestJson);
    }

    private void LogResponse(string responseName, TResponse response)
    {
        if (response is not IResponseLoggable)
        {
            logger.LogInformation("Handled {ResponseName} response", responseName);
            return;
        }
        string responseJson = response.ConvertToJson();
        logger.LogInformation("Handled {ResponseName} response: {Response}", responseName, responseJson);
    }
}
