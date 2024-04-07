using FluentValidation;
using TimeAttendanceApp.Infrostructure.DTOs.ProjectDtos;
using TimeAttendanceApp.Infrostructure.DTOs.TaskDto;

namespace TimeAttendanceApp.Infrostructure.Validators.TaskValidators
{
    public class TaskRequestDtoValidator : AbstractValidator<TaskRequestDto>
    {
        public TaskRequestDtoValidator()
        {
            RuleFor(p => p.name)
                .NotEmpty()
                .MaximumLength(255)
                .NotNull()
                .WithMessage("The name must be up to 255 characters long");
        }
    }
}
