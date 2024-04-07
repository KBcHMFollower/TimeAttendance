using Microsoft.EntityFrameworkCore;
using TimeAttendanceApp.Context;
using TimeAttendanceApp.Infrostructure.Errors;
using TimeAttendanceApp.Services.FileProccessingService;
using TimeAttendanceApp.Services.ProjectService;
using TimeAttendanceApp.Services.TaskCommentsService;
using TimeAttendanceApp.Services.TaskService;

namespace TimeAttendanceApp
{
    public static class StatrUpExtensions
    {
        public static WebApplicationBuilder UseControllers(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers()
                .AddNewtonsoftJson();

            return builder;
        }
        public static WebApplicationBuilder UseDbContext(this WebApplicationBuilder builder, string connectionString)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString(connectionString)));

            return builder;
        }
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IProjectService, ProjectsService>();
            builder.Services.AddScoped<ITaskService, TaskService>();
            builder.Services.AddScoped<ITaskCommentService, TaskCommentService>();
            builder.Services.AddSingleton<IFileProcessingService, FileProcessingService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            return builder;
        }

        public static WebApplication Configurate(this WebApplication app)
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

            return app;
        }

    }
}
