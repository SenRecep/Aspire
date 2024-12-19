using System.Reflection;

using CSharpEssentials;
using CSharpEssentials.Exceptions;
using FluentValidation;

using MediatR;

namespace CompanyName.BuildingBlocks.Application.PipelineBehaviors;
public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private static readonly Type resultType = typeof(Result);
    private static readonly Type genericResultType = typeof(Result<>);
    private readonly Type responseType = typeof(TResponse);

    private readonly IValidator<TRequest>[] validatorArray = [.. validators];
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (validatorArray.Length == 0)
            return await next();

        var context = new ValidationContext<TRequest>(request);
        FluentValidation.Results.ValidationResult[] validationFailures = await Task.WhenAll(
            validatorArray.Select(validator => validator.ValidateAsync(context, cancellationToken)));
        Error[] errors = [.. validationFailures
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .Select(CreateErrorFromValidationFailure)
            .Distinct()];

        if (errors.Length == 0)
            return await next();

        if (responseType == resultType)
            return (TResponse)(object)Result.Failure(errors);

        if (responseType == genericResultType)
            return CreateResponse(errors);

        throw new EnhancedValidationException(errors);
    }
    private TResponse CreateResponse(Error[] errors)
    {
        Type genericType = genericResultType.MakeGenericType(responseType.GenericTypeArguments[0]);
        MethodInfo factoryMethod = genericType.GetMethod(nameof(Result.Failure))!;
        object result = factoryMethod.Invoke(null, [errors]);
        return (TResponse)result;
    }

    private static Error CreateErrorFromValidationFailure(FluentValidation.Results.ValidationFailure validationResult) => Error.Validation(
        code: validationResult.ErrorCode,
        description: validationResult.ErrorMessage,
        metadata: new ErrorMetadata(nameof(validationResult.PropertyName), validationResult.PropertyName));
}
