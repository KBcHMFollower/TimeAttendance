using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeAttendanceApp.Infrostructure.DTOs.ProjectDtos;
using TimeAttendanceApp.Infrostructure.DTOs;
using TimeAttendanceApp.Models;
using TimeAttendanceApp.Services.ProjectService;
using TimeAttendanceApp.Services.TaskService;
using TimeAttendanceApp.Infrostructure.DTOs.TaskDto;
using TimeAttendanceApp.Infrostructure.DTOs.TaskDtos;

namespace TimeAttendanceApp.Controllers
{
    [ApiController]
    [Route("/projects/{projectId}/tasks")]
    public class TaskController : Controller
    {
        readonly private ITaskService taskService;
        public TaskController(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult?> Create([FromRoute] Guid projectId, [FromBody] TaskDto createDto)
        {
            Models.Task? resTask = await taskService.Create(projectId, createDto);
            return CreatedAtAction("Get", new { id = resTask.Id }, resTask);
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid taskId)
        {
            Models.Task? resTask = await taskService.Delete(taskId);
            return Ok(resTask);
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetOne([FromRoute] Guid taskId)
        {
            Models.Task? resTask = await taskService.GetOne(taskId);
            return Ok(resTask);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromRoute] Guid projectId, [FromQuery] FilterDto FilterDto)
        {
            List<Models.Task>? resTask = await taskService.GetAll(projectId, FilterDto);
            return Ok(resTask);
        }

        [HttpPatch("{taskId}")]
        public async Task<IActionResult> Update([FromRoute] Guid taskId, [FromBody] TaskUpdateDto updateDto)
        {
            Models.Task? resTask = await taskService.Update(taskId, updateDto);
            return Ok(resTask);
        }
    }
}
