using FluentValidation;
using TimeAttendanceApp.Infrostructure.DTOs.ProjectDtos;

namespace TimeAttendanceApp.Infrostructure.Validators.ProjectValidators
{
        public class ProjectUpdateDtoValidator : AbstractValidator<ProjectRequestDto>
        {
            public ProjectUpdateDtoValidator()
            {
                RuleFor(p => p.name)
                    .NotEmpty()
                    .MaximumLength(255)
                    .NotNull()
                    .WithMessage("The name must be up to 255 characters long");
            }
        }
}
