using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using TimeAttendanceApp.Context;
using TimeAttendanceApp.Infrostructure.Errors;
using TimeAttendanceApp.Services.FileProccessingService;
using TimeAttendanceApp.Services.ProjectService;
using TimeAttendanceApp.Services.TaskCommentsService;
using TimeAttendanceApp.Services.TaskService;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers()
            .AddNewtonsoftJson();
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        AddServices(builder.Services);

        var app = builder.Build();

        Configurate(app);
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IProjectService, ProjectsService>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<ITaskCommentService, TaskCommentService>();
        services.AddSingleton<IFileProcessingService, FileProcessingService>();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });
    }

    private static void Configurate(WebApplication app)
    {
        app.UseMiddleware<ErrorHandlingMiddleware>();

        if (!app.Environment.IsDevelopment())
        {
            //app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        app.UseCors("AllowAll");
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        //app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}