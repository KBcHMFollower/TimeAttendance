using Microsoft.EntityFrameworkCore;
using TimeAttendanceApp.Context;
using TimeAttendanceApp.Models;
using TimeAttendanceApp.Infrostructure.DTOs.ProjectDtos;
using TimeAttendanceApp.Infrostructure.Errors;
using TimeAttendanceApp.Infrostructure.DTOs;
using FluentValidation;
using FluentValidation.Results;
using TimeAttendanceApp.Infrostructure.Validators.ProjectValidators;

namespace TimeAttendanceApp.Services.ProjectService
{
    public class ProjectsService:IProjectService
    {
        readonly private ApplicationDbContext _context;

        public ProjectsService(ApplicationDbContext context)
        {
            _context = context;
        }

        ProjectResponseDto CreateResponseDto(Project project)
        {
            ProjectResponseDto projectResponseDto = new ProjectResponseDto
            {
                name = project.ProjectName,
                id = project.Id,
            };

            return projectResponseDto;
        }

    public async Task<ProjectResponseDto?> Create(ProjectRequestDto projCreateDto)
        {
            ProjectRequestDtoValidator validator = new ProjectRequestDtoValidator();
            ValidationResult validRes = validator.Validate(projCreateDto);

            if (!validRes.IsValid)
            {
                throw ServiceException.BadRequest("Validation Error", validRes.Errors);
            }

            Guid projectId = Guid.NewGuid();
            Project? project = new Project {Id = projectId, ProjectName = projCreateDto.name};

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            Project? createdProj = await _context.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(item=>item.Id == projectId);

            return CreateResponseDto(createdProj);
        }

        public async Task<ProjectResponseDto> Delete(Guid projectId)
        {
            Project? Proj = await _context.Projects
                    .AsNoTracking()
                    .FirstOrDefaultAsync(item => item.Id == projectId);

            if (Proj == null)
            {
                throw ServiceException.NotFound("Project not found");
            }

            await _context.Projects
                    .Where(item => item.Id == projectId)
                    .ExecuteDeleteAsync();

            return CreateResponseDto(Proj);
        }

        public async Task<List<ProjectResponseDto>> GetAll(FilterDto projGetAllDto)
        {
            var totalCount = await _context.Projects
                .AsNoTracking()
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

            IQueryable<Project> query = _context.Projects
                .AsNoTracking();

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

            List<Project>? entities = await query
                .Skip(skipAmount)
                .Take(projGetAllDto.count)
                .ToListAsync();

            List<ProjectResponseDto> response = entities
                .Select(i=>CreateResponseDto(i))
                .ToList();

            return response;
        }

        public async Task<ProjectResponseDto> GetOne(Guid projectId)
        {
            Project? resProj = await _context.Projects
                    .AsNoTracking()
                    .FirstOrDefaultAsync(item => item.Id == projectId);

            if ( resProj == null)
            {
                throw ServiceException.NotFound("Project not found");
            }

            return CreateResponseDto(resProj);
        }

        public async Task<ProjectResponseDto?> Update(Guid projectId, ProjectRequestDto projUpdateDto)
        {
            ProjectUpdateDtoValidator validator = new ProjectUpdateDtoValidator();
            ValidationResult validRes = validator.Validate(projUpdateDto);

            if (!validRes.IsValid)
            {
                throw ServiceException.BadRequest("Validation Error", validRes.Errors);
            }

            Project? Proj = await _context.Projects
                    .AsNoTracking()
                    .FirstOrDefaultAsync(item => item.Id == projectId);

            if (Proj == null)
            {
                throw ServiceException.NotFound("Project not found");
            }

            await _context.Projects
                .Where(item=>item.Id == projectId)
                .ExecuteUpdateAsync(proj =>
                    proj.SetProperty(i => i.ProjectName, projUpdateDto.name));

            Proj = await _context.Projects
                    .AsNoTracking()
                    .FirstOrDefaultAsync(item => item.Id == projectId);

            return CreateResponseDto(Proj);
        }
    }
}
