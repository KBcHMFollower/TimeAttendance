using Microsoft.EntityFrameworkCore;
using TimeAttendanceApp.Context;
using TimeAttendanceApp.Models;
using TimeAttendanceApp.Infrostructure.DTOs.ProjectDtos;
using TimeAttendanceApp.Infrostructure.Errors;
using TimeAttendanceApp.Infrostructure.DTOs;

namespace TimeAttendanceApp.Services.ProjectService
{
    public class ProjectsService:IProjectService
    {
        readonly private ApplicationDbContext _context;

        public ProjectsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Project?> Create(ProjDto projCreateDto)
        {
            Guid projectId = Guid.NewGuid();
            Project? project = new Project {Id = projectId, ProjectName = projCreateDto.name};

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            Project? createdProj = await _context.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(item=>item.Id == projectId);

            return createdProj;
        }

        public async Task<Project> Delete(Guid projectId)
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

            return Proj;
        }

        public async Task<List<Project>> GetAll(FilterDto projGetAllDto)
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

            return entities;
        }

        public async Task<Project> GetOne(Guid projectId)
        {
            Project? resProj = await _context.Projects
                    .AsNoTracking()
                    .FirstOrDefaultAsync(item => item.Id == projectId);

            if ( resProj == null)
            {
                throw ServiceException.NotFound("Project not found");
            }

            return resProj;
        }

        public async Task<Project?> Update(Guid projectId, ProjDto projUpdateDto)
        {
            Project? Proj = await _context.Projects
                    .AsNoTracking()
                    .FirstOrDefaultAsync(item => item.Id == projectId);

            if (Proj == null)
            {
                throw ServiceException.NotFound("Project not found");
            }

            await _context.Projects
                .ExecuteUpdateAsync(proj =>
                    proj.SetProperty(i => i.ProjectName, projUpdateDto.name));

            Proj = await _context.Projects
                    .AsNoTracking()
                    .FirstOrDefaultAsync(item => item.Id == projectId);

            return Proj;
        }
    }
}
