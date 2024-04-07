using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using TimeAttendanceApp.Context;
using TimeAttendanceApp.Infrostructure.DTOs;
using TimeAttendanceApp.Infrostructure.DTOs.ProjectDtos;
using TimeAttendanceApp.Infrostructure.DTOs.TaskCommentsDto;
using TimeAttendanceApp.Infrostructure.DTOs.TaskDtos;
using TimeAttendanceApp.Infrostructure.Errors;
using TimeAttendanceApp.Infrostructure.Validators.CommentValidators;
using TimeAttendanceApp.Models;
using TimeAttendanceApp.Services.FileProccessingService;

namespace TimeAttendanceApp.Services.TaskCommentsService
{
    public class TaskCommentService : ITaskCommentService
    {

        readonly private ApplicationDbContext _context;
        readonly private IFileProcessingService _fileProcessingService;


    public TaskCommentService(ApplicationDbContext _context, IFileProcessingService _fileProcessingService) 
            { 
                this._context = _context;
                this._fileProcessingService = _fileProcessingService;
            }

        CommentResponseDto CreateResponseDto(TaskComment comment)
        {
            CommentResponseDto responseDto = new CommentResponseDto
            {
                id = comment.Id,
                commentType = comment.CommentType
            };
            if (comment.CommentType == 0)
            {
                responseDto.text = Encoding.UTF8.GetString(comment.Content);
            }
            else
            {
                responseDto.text = comment.FileName;
            }
            return responseDto;
        }

        public async Task<CommentResponseDto?> Create(Guid taskId, CommentRequestDto commentCreateDto)
        {
            CommentsRequestDtoValidator validator = new CommentsRequestDtoValidator();
            ValidationResult validRes = validator.Validate(commentCreateDto);

            if (!validRes.IsValid)
            {
                throw ServiceException.BadRequest("Validation Error", validRes.Errors);
            }

            Models.Task? task = await _context.Tasks
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
                    taskComment.Content = await _fileProcessingService.ConvertFileToByteArrayAsync(commentCreateDto.file);
                    taskComment.FileName = commentCreateDto.file.FileName;
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

            Models.Task? Task = await _context.Tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == taskId);

            if (Task == null)
            {
                throw ServiceException.NotFound("Project not found");
            }

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


        public async Task<CommentResponseDto?> Update(Guid commentId, CommentRequestDto commentUpdateDto)
        {
            CommentsUpdateDtoValidator validator = new CommentsUpdateDtoValidator();
            ValidationResult validRes = validator.Validate(commentUpdateDto);

            if (!validRes.IsValid)
            {
                throw ServiceException.BadRequest("Validation Error", validRes.Errors);
            }

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
                    Comment.CommentType = 0;
                    Comment.Content = Encoding.UTF8.GetBytes(commentUpdateDto.text);
                    Comment.FileName = null;
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
                    Comment.CommentType = 1;
                    Comment.Content = await _fileProcessingService.ConvertFileToByteArrayAsync(commentUpdateDto.file);
                    Comment.FileName = commentUpdateDto.file.FileName;
                }
                else
                {
                    throw ServiceException.BadRequest("CommentType is text, but field file is empty");
                }
            }

            await _context.SaveChangesAsync();

            return CreateResponseDto(Comment);
        }

        public async Task<FileServiceResponse> DownloadFile(Guid commentId)
        {
            TaskComment? file = await _context.TaskComments
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == commentId);

            if (file == null || file.CommentType == 0) 
            {
                throw ServiceException.NotFound("Comment not found");
            }

            if (file.CommentType == 0)
            {
                throw ServiceException.BadRequest("Comment type is text");
            }

            var memoryStream = new MemoryStream(file.Content);

            FileServiceResponse fileRes = new FileServiceResponse
            {
                memoryStream = memoryStream,
                fileName = file.FileName
            };

            return fileRes;
        }
    }
}
