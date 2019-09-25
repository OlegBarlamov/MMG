﻿namespace FrameworkSDK.MonoGame.Constructing
{
    public static class GameFactory
    {
        public static IGameConfigurator<TGame> CreateGame<TGame>(this AppFactory appFactory) where TGame : IGameHost
        {
            return appFactory.Create<TGame>()
                .UseGameFramework<TGame>();
        }
    }
}
