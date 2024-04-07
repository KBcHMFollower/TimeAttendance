using TimeAttendanceApp.Models;
using TimeAttendanceApp.Infrostructure.DTOs.ProjectDtos;
using TimeAttendanceApp.Infrostructure.DTOs;

namespace TimeAttendanceApp.Services.ProjectService
{
    public interface IProjectService
    {
        public Task<ProjectResponseDto?> Create(ProjectRequestDto projCreateDto);
        public Task<ProjectResponseDto> GetOne(Guid projectId);
        public Task<List<ProjectResponseDto>> GetAll(FilterDto projGetAllDto);
        public Task<ProjectResponseDto?> Update(Guid projectId, ProjectRequestDto projUpdateDto);
        public Task<ProjectResponseDto> Delete(Guid projectId);

    }
}
