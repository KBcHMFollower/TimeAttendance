using TimeAttendanceApp.Infrostructure.DTOs.TaskCommentsDto;
using TimeAttendanceApp.Infrostructure.DTOs;
using TimeAttendanceApp.Models;

namespace TimeAttendanceApp.Services.TaskCommentsService
{
    public interface ITaskCommentService
    {
        public Task<CommentResponseDto?> Create(Guid taskId, TaskCommentDto commentCreateDto);
        public Task<CommentResponseDto> GetOne(Guid commentId);
        public Task<List<CommentResponseDto>> GetAll(Guid taskId, FilterDto commentGetAllDto);
        public Task<CommentResponseDto?> Update(Guid commentId, TaskCommentDto commentUpdateDto);
        public Task<CommentResponseDto> Delete(Guid commentId);
    }
}
