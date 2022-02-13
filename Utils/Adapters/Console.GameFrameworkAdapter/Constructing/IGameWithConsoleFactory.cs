using Console.Core;
using FrameworkSDK;

namespace Console.FrameworkAdapter
{
    public interface IGameWithConsoleFactory : IAppFactory
    {
        IGameWithConsoleFactory UseConsoleMessagesProvider(IConsoleMessagesProvider consoleMessagesProvider);
        IGameWithConsoleFactory UseConsoleMessagesProvider<TConsoleMessageProvider>() where TConsoleMessageProvider : class, IConsoleMessagesProvider;
        IGameWithConsoleFactory UseConsoleCommandExecutor(IConsoleCommandExecutor consoleCommandExecutor);
        IGameWithConsoleFactory UseConsoleCommandExecutor<TConsoleCommandExecutor>() where TConsoleCommandExecutor : class, IConsoleCommandExecutor;
        IGameWithConsoleFactory UseConsoleResourcePackage(IConsoleResourcePackage consoleResourcePackage);
        IGameWithConsoleFactory UseConsoleResourcePackage<TConsoleResourcePackage>() where TConsoleResourcePackage : class, IConsoleResourcePackage;
        
    }
}