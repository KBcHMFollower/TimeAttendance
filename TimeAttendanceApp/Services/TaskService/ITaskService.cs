using TimeAttendanceApp.Infrostructure.DTOs;
using TimeAttendanceApp.Models;
using TimeAttendanceApp.Infrostructure.DTOs.TaskDto;
using TimeAttendanceApp.Infrostructure.DTOs.TaskDtos;

namespace TimeAttendanceApp.Services.TaskService
{
    public interface ITaskService
    {
        public Task<Models.Task?> Create(Guid projectId, TaskDto taskCreateDto);
        public Task<Models.Task> GetOne(Guid taskId);
        public Task<List<Models.Task>> GetAll(Guid projectId, FilterDto taskGetAllDto);
        public Task<Models.Task?> Update(Guid taskId, TaskUpdateDto taskUpdateDto);
        public Task<Models.Task> Delete(Guid taskId);
    }
}
