
using Microsoft.EntityFrameworkCore;
using TimeAttendanceApp.Context;
using TimeAttendanceApp.Infrostructure.DTOs;
using TimeAttendanceApp.Infrostructure.DTOs.TaskDto;
using TimeAttendanceApp.Models;
using TimeAttendanceApp.Infrostructure.Errors;
using System.Threading.Tasks;
using TimeAttendanceApp.Infrostructure.DTOs.TaskDtos;
using FluentValidation;
using TimeAttendanceApp.Infrostructure.DTOs.TaskCommentsDto;
using FluentValidation.Results;
using TimeAttendanceApp.Infrostructure.Validators.TaskValidators;

namespace TimeAttendanceApp.Services.TaskService
{
    public class TaskService : ITaskService
    {
        readonly private ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

    TaskResponseDto CreateResponseDto(Models.Task task)
        {
            TaskResponseDto response = new TaskResponseDto
            {
                id = task.Id,
                name = task.TaskName,
                startDate = task.StartDate,
                cancelDate = task.CancelDate
            };

            return response;
        }
        public async Task<TaskResponseDto?> Create(Guid projectId, TaskRequestDto taskCreateDto)
        {
            TaskRequestDtoValidator validator = new TaskRequestDtoValidator();
            ValidationResult validRes = validator.Validate(taskCreateDto);

            if (!validRes.IsValid)
            {
                throw ServiceException.BadRequest("Validation Error", validRes.Errors);
            }

            Project? project = await _context.Projects
                .FirstOrDefaultAsync(item=>item.Id == projectId);

            if (project == null)
            {
                throw ServiceException.NotFound("Project not find");
            }

            Guid taskId = Guid.NewGuid();
            Models.Task newTask = new Models.Task
            {
                Id = taskId,
                TaskName = taskCreateDto.name,
                Project = project,
                StartDate = taskCreateDto.startDate,
                CancelDate = DateTime.UtcNow
            };

            await _context.Tasks.AddAsync(newTask);
            await _context.SaveChangesAsync();

            Models.Task? resTask = await _context.Tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(item=>item.Id == taskId);

            return CreateResponseDto(resTask);
        }

        public async Task<TaskResponseDto> Delete( Guid taskId)
        {

            Models.Task? Task = await _context.Tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(item=>item.Id==taskId);

            if (Task == null)
            {
                throw ServiceException.NotFound("Task not found");
            }

            await _context.Tasks
                    .Where(item => item.Id == taskId)
                    .ExecuteDeleteAsync();

            return CreateResponseDto(Task);
        }

        public async Task<List<TaskResponseDto>> GetAll(Guid projectId, FilterDto taskGetAllDto)
        {
            Project? Proj = await _context.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == projectId);

            if (Proj == null)
            {
                throw ServiceException.NotFound("Project not found");
            }

            var totalCount = await _context.Tasks
                .AsNoTracking()
                .Where(item=>item.Project.Id == projectId)
                .CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / taskGetAllDto.count);

            if (taskGetAllDto.page < 1)
            {
                taskGetAllDto.page = 1;
            }
            else if (taskGetAllDto.page > totalPages)
            {
                taskGetAllDto.page = totalPages;
            }

            var skipAmount = (taskGetAllDto.page - 1) * taskGetAllDto.count;

            if (skipAmount < 0) { skipAmount = 0; }

            IQueryable<Models.Task> query = _context.Tasks
                .AsNoTracking()
                .Where(item => item.Project.Id == projectId);

            if (taskGetAllDto.OrderType == "desc")
            {
                query = query.
                    OrderByDescending(item => EF.Property<object>(item, taskGetAllDto.OrderTarget));
            }
            else
            {
                query = query.
                    OrderBy(item => EF.Property<object>(item, taskGetAllDto.OrderTarget));
            }

            List<Models.Task>? entities = await query
                .Skip(skipAmount)
                .Take(taskGetAllDto.count)
                .ToListAsync();

            List<TaskResponseDto> response = entities
                .Select(i=>CreateResponseDto(i))
                .ToList();

            return response;

        }

        public async Task<TaskResponseDto> GetOne(Guid taskId)
        {
            Models.Task? resTask = await _context.Tasks
                    .AsNoTracking()
                    .FirstOrDefaultAsync(item => item.Id == taskId);

            if (resTask == null)
            {
                throw ServiceException.NotFound("Project not found");
            }

            return CreateResponseDto(resTask);
        }

        public async Task<TaskResponseDto?> Update(Guid taskId, TaskUpdateDto taskUpdateDto)
        {
            TaskUpdateDtoValidator validator = new TaskUpdateDtoValidator();
            ValidationResult validRes = validator.Validate(taskUpdateDto);

            if (!validRes.IsValid)
            {
                throw ServiceException.BadRequest("Validation Error", validRes.Errors);
            }

            Models.Task? Task = await _context.Tasks
                    .FirstOrDefaultAsync(item => item.Id == taskId);

            if (Task == null)
            {
                throw ServiceException.NotFound("Project not found");
            }

            
            if (taskUpdateDto.name != null)
            {
                Task.TaskName = taskUpdateDto.name;
            }
            if (taskUpdateDto.endDate.HasValue)
            {
                Task.CancelDate = taskUpdateDto.endDate.Value;
            }
            if (taskUpdateDto.startDate.HasValue)
            {
                Task.StartDate = taskUpdateDto.startDate.Value;
            }

            await _context.SaveChangesAsync();

            return CreateResponseDto(Task);
        }
    }
}
