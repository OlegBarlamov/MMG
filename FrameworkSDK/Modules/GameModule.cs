﻿using FrameworkSDK.Common.Services.Graphics;
using FrameworkSDK.Game;
using FrameworkSDK.Game.Mapping;
using FrameworkSDK.Game.Mapping.Default;
using FrameworkSDK.Game.Scenes;
using FrameworkSDK.IoC;
using FrameworkSDK.Services.Graphics;

namespace FrameworkSDK.Modules
{
	internal class GameModule<TGameHost> : IServicesModule where TGameHost : IGameHost
	{
		public void Register(IServiceRegistrator serviceRegistrator)
		{
			serviceRegistrator.RegisterType<IScenesController, ScenesController>();
			serviceRegistrator.RegisterType<MappingHost, MappingHost>();
		    serviceRegistrator.RegisterType<IViewResolver, DefaultViewResolver>();
		    serviceRegistrator.RegisterType<IControllerResolver, DefaultControllerResolver>();

			serviceRegistrator.RegisterType<ISpriteBatchProvider, DefaultSpriteBatchProvider>();

            serviceRegistrator.RegisterType<IGameHost, TGameHost>();
            serviceRegistrator.RegisterType<GameShell, GameShell>();
		}
	}
}
