using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using TimeAttendanceApp.Context;
using TimeAttendanceApp.Infrostructure.DTOs;
using TimeAttendanceApp.Infrostructure.DTOs.TaskCommentsDto;
using TimeAttendanceApp.Infrostructure.DTOs.TaskDtos;
using TimeAttendanceApp.Infrostructure.Errors;
using TimeAttendanceApp.Models;

namespace TimeAttendanceApp.Services.TaskCommentsService
{
    public class TaskCommentService : ITaskCommentService
    {

        readonly private ApplicationDbContext _context;

        

        public TaskCommentService(ApplicationDbContext _context) 
        { 
            this._context = _context;
        }

        CommentResponseDto CreateResponseDto(TaskComment comment)
        {
            CommentResponseDto responseDto = new CommentResponseDto
            {
                id = comment.Id,
                taskId = comment.Task.Id,
                commentType = comment.CommentType
            };
            if (comment.CommentType == 0)
            {
                responseDto.text = Encoding.UTF8.GetString(comment.Content);
            }
            else
            {
                responseDto.file = comment.Content;
            }
            return responseDto;
        }

        public async Task<CommentResponseDto?> Create(Guid taskId, TaskCommentDto commentCreateDto)
        {
            Models.Task? task = await _context.Tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(item=>item.Id == taskId);

            if (task == null)
            {
                throw ServiceException.NotFound("Task not found");
            }

            Guid commentId = Guid.NewGuid();
            TaskComment taskComment = new TaskComment {Id =commentId,  CommentType = commentCreateDto.commentType, Task = task };

            if (commentCreateDto.commentType == 0 )
            {
                if (commentCreateDto.text != null)
                {
                    taskComment.Content = Encoding.UTF8.GetBytes(commentCreateDto.text);
                }
                else
                {
                    throw ServiceException.BadRequest("CommentType is text, but field text is empty");
                }
            }
            else
            {
                if (commentCreateDto.file != null)
                {
                    taskComment.Content = commentCreateDto.file;
                }
                else
                {
                    throw ServiceException.BadRequest("CommentType is file, but but field file is empty");
                }
                
            }

            await _context.TaskComments.AddAsync(taskComment);
            await _context.SaveChangesAsync();

            TaskComment? createdComment = await _context.TaskComments
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == commentId);

            return CreateResponseDto(createdComment);
        }

        public async Task<CommentResponseDto> Delete(Guid commentId)
        {
            TaskComment? Comment = await _context.TaskComments
                   .AsNoTracking()
                   .FirstOrDefaultAsync(item => item.Id == commentId);

            if (Comment == null)
            {
                throw ServiceException.NotFound("Comment not found");
            }

            await _context.TaskComments
                    .Where(item => item.Id == commentId)
                    .ExecuteDeleteAsync();

            return CreateResponseDto(Comment);
        }

        public async Task<List<CommentResponseDto>> GetAll(Guid taskId, FilterDto projGetAllDto)
        {
            var totalCount = await _context.TaskComments
                .AsNoTracking()
                .Where(item=>item.Task.Id == taskId)
                .CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / projGetAllDto.count);

            if (projGetAllDto.page < 1)
            {
                projGetAllDto.page = 1;
            }
            else if (projGetAllDto.page > totalPages)
            {
                projGetAllDto.page = totalPages;
            }

            var skipAmount = (projGetAllDto.page - 1) * projGetAllDto.count;

            if (skipAmount < 0) { skipAmount = 0; }

            IQueryable<TaskComment> query = _context.TaskComments
                .AsNoTracking()
                .Where(item => item.Task.Id == taskId);

            if (projGetAllDto.OrderType == "desc")
            {
                query = query.
                    OrderByDescending(item => EF.Property<object>(item, projGetAllDto.OrderTarget));
            }
            else
            {
                query = query.
                    OrderBy(item => EF.Property<object>(item, projGetAllDto.OrderTarget));
            }

            List<TaskComment>? entities = await query
                .Skip(skipAmount)
                .Take(projGetAllDto.count)
                .ToListAsync();

            List<CommentResponseDto> resList = entities
                .Select(item =>CreateResponseDto(item))
                .ToList();

            return resList;
        }

        public async Task<CommentResponseDto> GetOne(Guid commentId)
        {
            TaskComment? resComment = await _context.TaskComments
            .AsNoTracking()
                    .FirstOrDefaultAsync(item => item.Id == commentId);

            if (resComment == null)
            {
                throw ServiceException.NotFound("Project not found");
            }

            return CreateResponseDto(resComment);
        }

        public async Task<CommentResponseDto?> Update(Guid commentId, TaskCommentDto commentUpdateDto)
        {
            TaskComment? Comment = await _context.TaskComments
                   .FirstOrDefaultAsync(item => item.Id == commentId);

            if (Comment == null)
            {
                throw ServiceException.NotFound("Comment not found");
            }


            if (commentUpdateDto.commentType == 0)
            {
                if (commentUpdateDto.text != null)
                {
                    Comment.Content = Encoding.UTF8.GetBytes(commentUpdateDto.text);
                }
                else
                {
                    throw ServiceException.BadRequest("CommentType is text, but field text is empty");
                }
            }
            else
            {
                if (commentUpdateDto.file != null)
                {
                    Comment.Content = commentUpdateDto.file;
                }
                else
                {
                    throw ServiceException.BadRequest("CommentType is text, but field file is empty");
                }
            }

            await _context.SaveChangesAsync();

            return CreateResponseDto(Comment);
        }
    }
}
