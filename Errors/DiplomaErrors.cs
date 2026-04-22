namespace ExaminationSystem.Errors;

public static class DiplomaErrors
{
    public static Error NotFound(Guid Id) =>
        new("Diploma.NotFound", $"Diploma with ID '{Id}' was not found", StatusCodes.Status404NotFound);
    public static Error HasActiveEnrollments =>
        new("Diploma.HasActiveEnrollments", $"We found user enrollment in this diploma", StatusCodes.Status409Conflict);
}
