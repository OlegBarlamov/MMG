using AspNetCore.FrameworkAdapter;
using Console.FrameworkAdapter.Constructing;
 using FrameworkSDK.Constructing;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Epic.Server
 {
     public class Program
     {
         public static void Main(string[] args)
         {
             var appBuilder = WebApplication.CreateBuilder(args);
             appBuilder.Services.AddControllers();
 
             var app = appBuilder.UseFramework()
                 .AddServices<ConsoleCommandsExecutingServicesModule>()
                 .AddServices<ServerServicesModule>()
                 .AddComponent<TerminalConsoleSubsystem>()
                 .AddComponent<DebugStartupScript>()
                 .Construct()
                 .Asp();
 
             app.MapControllers();
             app.UseWebSockets()
                 .UseHttpsRedirection();
 
             app.Run();
         }
     }
 }