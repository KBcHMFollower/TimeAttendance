using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeAttendanceApp.Infrostructure.DTOs.ProjectDtos;
using TimeAttendanceApp.Infrostructure.DTOs;
using TimeAttendanceApp.Models;
using TimeAttendanceApp.Services.ProjectService;

namespace TimeAttendanceApp.Controllers
{
    [ApiController]
    [Route("projects")]
    public class ProjectController : Controller
    {
        readonly private IProjectService taskService;
        public ProjectController(IProjectService taskService)
        {
            this.taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult?> Create([FromBody] ProjDto createDto)
        {
            Project? resProj = await taskService.Create(createDto);
            return CreatedAtAction("Get", new { id = resProj.Id }, resProj);
        }

        [HttpDelete("{projectId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid projectId)
        {
            Project? resProj = await taskService.Delete(projectId);
            return Ok(resProj);
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetOne([FromRoute] Guid projectId)
        {
            Project? resProj = await taskService.GetOne(projectId);
            return Ok(resProj);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] FilterDto FilterDto)
        {
            List<Project>? resProj = await taskService.GetAll(FilterDto);
            return Ok(resProj);
        }

        [HttpPatch("{projectId}")]
        public async Task<IActionResult> Update([FromRoute] Guid projectId, [FromBody] ProjDto updateDto)
        {
            Project? resProj = await taskService.Update(projectId, updateDto);
            return Ok(resProj);
        }
    }
}
