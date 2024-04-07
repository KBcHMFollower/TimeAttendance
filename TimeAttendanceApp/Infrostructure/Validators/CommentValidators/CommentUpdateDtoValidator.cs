using FluentValidation;
using TimeAttendanceApp.Infrostructure.DTOs.TaskCommentsDto;

namespace TimeAttendanceApp.Infrostructure.Validators.CommentValidators
{
    public class CommentsUpdateDtoValidator : AbstractValidator<CommentRequestDto>
    {
        public CommentsUpdateDtoValidator()
        {
            RuleFor(p => p.commentType)
                .NotNull()
                .InclusiveBetween((byte)0, (byte)1)
                .WithMessage("commentType must be in the range from 1 to 0");
            RuleFor(p => p.commentType)
                .Must((dto, commentType) =>
                {
                    if (commentType == 0)
                    {
                        return !string.IsNullOrWhiteSpace(dto.text);
                    }
                    else if (commentType == 1)
                    {
                        return dto.file != null;
                    }

                    return true;
                })
                .WithMessage("CommentType and attached content do not match");
        }
    }
}
