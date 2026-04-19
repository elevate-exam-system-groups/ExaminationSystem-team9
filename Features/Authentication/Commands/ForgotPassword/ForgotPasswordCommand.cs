using ExaminationSystem.Abstractions;
using MediatR;

namespace ExaminationSystem.Features.Authentication.Commands.ForgotPassword;

public record ForgotPasswordCommand(string Email) : IRequest<Result>;
