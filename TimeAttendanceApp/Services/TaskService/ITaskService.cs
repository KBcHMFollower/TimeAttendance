using TimeAttendanceApp.Infrostructure.DTOs;
using TimeAttendanceApp.Models;
using TimeAttendanceApp.Infrostructure.DTOs.TaskDto;
using TimeAttendanceApp.Infrostructure.DTOs.TaskDtos;

namespace TimeAttendanceApp.Services.TaskService
{
    public interface ITaskService
    {
        public Task<TaskResponseDto?> Create(Guid projectId, TaskRequestDto taskCreateDto);
        public Task<TaskResponseDto> GetOne(Guid taskId);
        public Task<List<TaskResponseDto>> GetAll(Guid projectId, FilterDto taskGetAllDto);
        public Task<TaskResponseDto?> Update(Guid taskId, TaskUpdateDto taskUpdateDto);
        public Task<TaskResponseDto> Delete(Guid taskId);
    }
}
