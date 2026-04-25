/* File Overview
 * File: ResultExtension.cs
 * Purpose: Cross-cutting abstractions: shared result, error, and pagination primitives.
 * Architecture: Clean Architecture with CQRS and MediatR patterns.
 * Techniques: Dependency Injection, separation of concerns, and maintainable layering conventions.
 * Libraries: See using directives below (commonly ASP.NET Core, MediatR, EF Core, FluentValidation, Mapster).
 */

using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Abstractions;

public static class ResultExtension
{
    public static ObjectResult ToProblem(this Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Cannot convert access result to a problem");

        var problem = Results.Problem(statusCode: result.Error.StatusCode);
        var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

        problemDetails!.Extensions = new Dictionary<string, object?>
        {
            {
                "errors",new[]
                {
                    result.Error.Code,
                    result.Error.Description
                }
            }
         };

        return new ObjectResult(problemDetails);
    }
}

