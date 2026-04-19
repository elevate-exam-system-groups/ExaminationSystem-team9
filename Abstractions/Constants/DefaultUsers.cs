namespace ExaminationSystem.Abstractions.Constants;

public static class DefaultUsers
{
    public static Guid AdminId = Guid.Parse("84E9BE3E-D03D-457C-AA82-32CD9A6CC17F");
    public const string AdminEmail = "admin@examination.com";
    public const string AdminPassword = "P@ssword123";
    public const string AdminSecurityStamp = "EB8550F122F348CE921A3030029B27B7";
    public const string AdminConcurrencyStamp = "EB8550F122F348CE921A3030029B27B7";
}