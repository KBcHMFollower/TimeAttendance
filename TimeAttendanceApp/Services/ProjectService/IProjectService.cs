using TimeAttendanceApp.Models;
using TimeAttendanceApp.Infrostructure.DTOs.ProjectDtos;
using TimeAttendanceApp.Infrostructure.DTOs;

namespace TimeAttendanceApp.Services.ProjectService
{
    public interface IProjectService
    {
        public Task<Project?> Create(ProjDto projCreateDto);
        public Task<Project> GetOne(Guid projectId);
        public Task<List<Project>> GetAll(FilterDto projGetAllDto);
        public Task<Project?> Update(Guid projectId, ProjDto projUpdateDto);
        public Task<Project> Delete(Guid projectId);

    }
}
