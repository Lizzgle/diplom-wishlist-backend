using FluentValidation;

namespace Identity.Presentation.Models.Register;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(request => request.UserName).NotEmpty().WithMessage("Username is required");
        RuleFor(request => request.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(request => request.Password).NotEmpty().WithMessage("Password is required");
        RuleFor(request => request.ConfirmPassword).NotEmpty().WithMessage("ConfirmPassword is required");
    }
}