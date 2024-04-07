using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using TimeAttendanceApp;
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
        WebApplication.CreateBuilder(args)
            .UseControllers()
            .UseDbContext("DefaultConnection")
            .AddServices()
            .Build()
            .Configurate()
            .Run();
    }
}