using ExaminationSystem.Features.Quizzes.Commands.CreateQuiz;
using ExaminationSystem.Features.Quizzes.Common;
using ExaminationSystem.Domain.DTOs.Authentication;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Entities.Authentication;
using ExaminationSystem.Domain.Enums;
using Mapster;
namespace ExaminationSystem.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        //Auth Mapping
        config.NewConfig<RegisterRequest, ApplicationUser>()
            .Map(dest => dest.UserName, src => $"{src.Email}");

        // من Command لـ Entity
        config.NewConfig<CreateQuizCommand, Quiz>()
            .Map(dest => dest.Id, src => Guid.NewGuid())
            .Map(dest => dest.Status, src => DiplomaStatus.Draft);  

        // من Entity لـ Response
        config.NewConfig<Quiz, QuizResponse>()
            .Map(dest => dest.QuizId, src => src.Id)
            .Map(dest => dest.QuestionCount, src => src.Questions.Count);
    }
}
