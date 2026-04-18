using ExaminationSystem.Domain.DTOs.Authentication;
using ExaminationSystem.Domain.Entities.Authentication;
using Mapster;
namespace ExaminationSystem.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        //Auth Mapping
        config.NewConfig<RegisterRequest, ApplicationUser>()
            .Map(dest => dest.UserName, src => $"{src.Email}");
    }
}
