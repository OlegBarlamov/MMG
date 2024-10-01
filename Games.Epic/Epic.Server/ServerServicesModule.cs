using Console.Core;
using Console.FrameworkAdapter;
using Epic.Core;
using Epic.Data;
using Epic.Data.BattleDefinitions;
using Epic.Data.UnitTypes;
using Epic.Data.UserUnits;
using Epic.Server.Services;
using FrameworkSDK.DependencyInjection;

namespace Epic.Server
{
    public class ServerServicesModule : IServicesModule
    {
        public void RegisterServices(IServiceRegistrator serviceRegistrator)
        {
            serviceRegistrator.RegisterType<IConsoleController, LoggerConsoleMessagesViewer>();
            
            serviceRegistrator.RegisterType<ISessionsService, DefaultSessionService>();
            serviceRegistrator.RegisterType<IUsersService, DefaultUsersService>();
            serviceRegistrator.RegisterType<IAuthorizationService, DefaultAuthorizationService>();
            serviceRegistrator.RegisterType<IUserUnitsService, DefaultUserUnitsService>();
            serviceRegistrator.RegisterType<IUnitTypesService, DefaultUnitTypesService>();
            serviceRegistrator.RegisterType<IBattleDefinitionsService, DefaultBattleDefinitionsService>();
            
            serviceRegistrator.RegisterType<ISessionsRepository, InMemorySessionsRepository>();
            serviceRegistrator.RegisterType<IUsersRepository, InMemoryUsersRepository>();
            serviceRegistrator.RegisterType<IUserUnitsRepository, InMemoryUserUnitsRepository>();
            serviceRegistrator.RegisterType<IUnitTypesRepository, InMemoryUnitTypesRepository>();
            serviceRegistrator.RegisterType<IBattleDefinitionsRepository, InMemoryBattleDefinitionsRepository>();
        }
    }
}