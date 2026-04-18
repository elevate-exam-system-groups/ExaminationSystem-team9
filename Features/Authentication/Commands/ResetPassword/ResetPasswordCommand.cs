using ExaminationSystem.Abstractions;
using MediatR;

namespace ExaminationSystem.Features.Authentication.Commands.ResetPassword;

public record ResetPasswordCommand(
    string Token,
    string NewPassword,
    string ConfirmPassword) : IRequest<Result>;
