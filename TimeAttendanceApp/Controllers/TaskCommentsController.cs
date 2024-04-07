using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeAttendanceApp.Infrostructure.DTOs.ProjectDtos;
using TimeAttendanceApp.Infrostructure.DTOs;
using TimeAttendanceApp.Models;
using TimeAttendanceApp.Services.ProjectService;
using TimeAttendanceApp.Services.TaskCommentsService;
using TimeAttendanceApp.Infrostructure.DTOs.TaskCommentsDto;

namespace TimeAttendanceApp.Controllers
{
    [ApiController]
    [Route("projects/{projectId}/tasks/{taskId}/comments")]
    public class TaskCommentsController : Controller
    {
        readonly private ITaskCommentService commentService;
        public TaskCommentsController(ITaskCommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult?> Create([FromRoute] Guid taskId, [FromForm] CommentRequestDto createDto)
        {
            CommentResponseDto? resComment = await commentService.Create(taskId, createDto);
            return CreatedAtAction("Get", new { id = resComment.id }, resComment);
        }

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid commentId)
        {
            CommentResponseDto? resComment = await commentService.Delete(commentId);
            return Ok(resComment);
        }

        [HttpGet("{commentId}")]
        public async Task<IActionResult> GetOne([FromRoute] Guid commentId)
        {
            CommentResponseDto? resComment = await commentService.GetOne(commentId);
            return Ok(resComment);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromRoute] Guid taskId, [FromQuery] FilterDto FilterDto)
        {
            List<CommentResponseDto>? resComment = await commentService.GetAll(taskId, FilterDto);
            return Ok(resComment);
        }

        [HttpPatch("{commentId}")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Update([FromRoute] Guid commentId, [FromBody] CommentRequestDto updateDto)
        {
            CommentResponseDto? resComment = await commentService.Update(commentId, updateDto);
            return Ok(resComment);
        }

        [HttpGet("{commentId}/download")]
        public async Task<IActionResult> DownloadFile([FromRoute] Guid commentId)
        {
            FileServiceResponse fileRes = await commentService.DownloadFile(commentId);
            string contentType = "application/octet-stream";

            return File(fileRes.memoryStream, contentType, fileRes.fileName);
        }
    }
}
