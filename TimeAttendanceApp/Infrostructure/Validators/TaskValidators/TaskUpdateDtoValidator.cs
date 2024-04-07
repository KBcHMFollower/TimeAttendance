using FluentValidation;
using TimeAttendanceApp.Infrostructure.DTOs.TaskDtos;

namespace TimeAttendanceApp.Infrostructure.Validators.TaskValidators
{
    public class TaskUpdateDtoValidator:AbstractValidator<TaskUpdateDto>
    {
        public TaskUpdateDtoValidator() 
        {
            RuleFor(i => i.name)
                .MaximumLength(255)
                .WithMessage("The name must be up to 255 characters long");
            RuleFor(i => i.endDate)
                .Must((dto, endDate) => (endDate > dto.startDate) || !endDate.HasValue)
                .WithMessage("endDate < startDate");
            RuleFor(i => i.name)
                .Must((dto, name) => !string.IsNullOrEmpty(name) || dto.startDate.HasValue || dto.endDate.HasValue)
                .WithMessage("At least one field must not be empty");
        }
    }
}
