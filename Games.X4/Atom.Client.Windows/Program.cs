﻿using System;
using Atom.Client.AppComponents;
using Atom.Client.Resources;
using Console.FrameworkAdapter;
using Console.LoggingAdapter;
using FrameworkSDK;
using FrameworkSDK.Constructing;
using FrameworkSDK.MonoGame.Config;
using FrameworkSDK.MonoGame.Constructing;
using FrameworkSDK.MonoGame.Resources.Generation;
using Logging.FrameworkAdapter;
using NetExtensions.Geometry;

namespace Atom.Client.Windows
{
    internal class Program
    {
        [STAThread]
        public static void Main()
        {
            var loggerConsoleMessageProvider = new LoggerConsoleMessagesProvider();
            using (var game = new DefaultAppFactory()
                .SetupLogSystem(loggerConsoleMessageProvider, true)
                .AddComponent<ScenesContainerComponent>()
                .AddServices<MainServicesModule>()
                .UseGame<X4GameApp>()
                    .UseGameParameters(new DefaultGameParameters
                    {
                        IsMouseVisible = false,
                        BackBufferSize = new SizeInt(1280, 768)
                    })
                    .UseMvc()
                    .PreloadResourcePackage<ColorsTexturesPackage>()
                    .PreloadResourcePackage<LoadingSceneResources>()
                    .UseGameComponents()
                        .UseInGameConsole()
                        .UseConsoleMessagesProvider(loggerConsoleMessageProvider)
                        .UseConsoleCommandExecutor<ExecutableConsoleCommandsExecutor>()
                .Construct())
            {
                game.Run();
            }
        }
    }
}