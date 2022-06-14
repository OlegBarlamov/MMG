using System.Collections.Generic;
using Console.Core.Commands;

namespace Console.FrameworkAdapter
{
    public interface IExecutableCommandsCollection
    {
        void AddCommand(IExecutableConsoleCommand command);
        void RemoveCommand(IExecutableConsoleCommand command);
        IReadOnlyDictionary<string, IExecutableConsoleCommand> GetAvailableCommands();
        void PreloadCommands();
    }
}