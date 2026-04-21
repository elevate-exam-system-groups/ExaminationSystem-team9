namespace ExaminationSystem.Abstractions.Constants;

public class DefaultRoles
{
    public const string Admin = "Admin";
    public static Guid AdminRoleId = Guid.Parse("C9347073-4ADB-4301-B52B-40A89BFFBEA8");
    public const string AdminRoleConcurrencyStamp = "2B0CE196CB8646D4991E2A068C8BA717";



    public const string Student = "Student";
    public static Guid StudentRoleId = Guid.Parse("B19E6450-8A87-4C06-BDEC-E398D286DAE4");
    public const string StudentRoleConcurrencyStamp = "41B9439FEFF04EE08BA56A4E7952138F";

}
