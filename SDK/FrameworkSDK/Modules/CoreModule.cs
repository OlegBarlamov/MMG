﻿using System;
using FrameworkSDK.Common;
using FrameworkSDK.IoC;
using FrameworkSDK.Localization;
using FrameworkSDK.Logging;
using FrameworkSDK.Services;
using JetBrains.Annotations;

namespace FrameworkSDK.Modules
{
	internal class CoreModule : IServicesModule
	{
		private IFrameworkLogger Logger { get; }
	    private IFrameworkServiceContainer MainContainer { get; }
	    private IServiceContainerFactory ServiceContainerFactory { get; }
	    private ILocalization Localization { get; }

		public CoreModule([NotNull] ILocalization localization, [NotNull] IFrameworkLogger logger,
		    [NotNull] IFrameworkServiceContainer mainContainer, [NotNull] IServiceContainerFactory serviceContainerFactory)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		    MainContainer = mainContainer ?? throw new ArgumentNullException(nameof(mainContainer));
		    ServiceContainerFactory = serviceContainerFactory ?? throw new ArgumentNullException(nameof(serviceContainerFactory));
		    Localization = localization ?? throw new ArgumentNullException(nameof(localization));
		}

		public void Register(IServiceRegistrator serviceRegistrator)
		{
			serviceRegistrator.RegisterInstance<ILocalization>(Localization);
			serviceRegistrator.RegisterInstance<IFrameworkLogger>(Logger);
		    serviceRegistrator.RegisterInstance<IFrameworkServiceContainer>(MainContainer);
		    serviceRegistrator.RegisterInstance<IServiceContainerFactory>(ServiceContainerFactory);

            serviceRegistrator.RegisterInstance<IRandomService>(new DefaultRandomService(new Random(Guid.NewGuid().GetHashCode())));
            serviceRegistrator.RegisterType<IAppDomainService, AppDomainService>();
		}
	}
}
