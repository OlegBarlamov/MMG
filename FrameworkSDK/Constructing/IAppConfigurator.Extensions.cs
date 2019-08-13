﻿using System;
using FrameworkSDK.Configuration;
using FrameworkSDK.IoC;
using FrameworkSDK.Localization;
using FrameworkSDK.Logging;
using JetBrains.Annotations;

namespace FrameworkSDK.Constructing
{
	public static partial class AppConfiguratorExtensions
	{
		public static IAppConfigurator SetupCustomLocalization([NotNull] this IAppConfigurator configurator, [NotNull] Func<ILocalization> localizationFactory)
		{
			if (configurator == null) throw new ArgumentNullException(nameof(configurator));
			if (localizationFactory == null) throw new ArgumentNullException(nameof(localizationFactory));

			var initializationPhase = configurator.GetPhase(DefaultConfigurationSteps.Initialization);
			initializationPhase.AddOrReplace(new SimpleConfigurationAction(
				DefaultConfigurationSteps.InitializationActions.Localization,
				true,
				context =>
				{
					var localization = GetFromFactory(localizationFactory);
					context.SetObject(DefaultConfigurationSteps.ContextKeys.Localization, localization);
				}));

			return configurator;
		}

		public static IAppConfigurator SetupCustomLogger([NotNull] this IAppConfigurator configurator, [NotNull] Func<IFrameworkLogger> loggerFactory)
		{
			if (configurator == null) throw new ArgumentNullException(nameof(configurator));
			if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));

			var initializationPhase = configurator.GetPhase(DefaultConfigurationSteps.Initialization);
			initializationPhase.AddOrReplace(new SimpleConfigurationAction(
				DefaultConfigurationSteps.InitializationActions.Logging,
				true,
				context =>
				{
					var logger = GetFromFactory(loggerFactory);
					context.SetObject(DefaultConfigurationSteps.ContextKeys.Logger, logger);
				}));

			return configurator;
		}

		public static IAppConfigurator SetupCustomServiceContainer([NotNull] this IAppConfigurator configurator, [NotNull] Func<IFrameworkServiceContainer> serviceContainerFactory)
		{
			if (configurator == null) throw new ArgumentNullException(nameof(configurator));
			if (serviceContainerFactory == null) throw new ArgumentNullException(nameof(serviceContainerFactory));

			var initializationPhase = configurator.GetPhase(DefaultConfigurationSteps.Initialization);
			initializationPhase.AddOrReplace(new SimpleConfigurationAction(
				DefaultConfigurationSteps.InitializationActions.Ioc,
				true,
				context =>
				{
					var serviceContainer = GetFromFactory(serviceContainerFactory);
					context.SetObject(DefaultConfigurationSteps.ContextKeys.ServiceContainer, serviceContainer);
				}));

			return configurator;
		}

		public static IAppConfigurator RegisterServices([NotNull] this IAppConfigurator configurator, [NotNull] Action<IServiceRegistrator> registerAction)
		{
			if (configurator == null) throw new ArgumentNullException(nameof(configurator));
			if (registerAction == null) throw new ArgumentNullException(nameof(registerAction));

			var initializationPhase = configurator.GetPhase(DefaultConfigurationSteps.Registration);
			initializationPhase.AddAction(new SimpleConfigurationAction(
				"external_register",
				true,
				context =>
				{
					var serviceRegistrator = GetObjectFromContext<IServiceRegistrator>(context, DefaultConfigurationSteps.ContextKeys.ServiceContainer);
					registerAction.Invoke(serviceRegistrator);
				}));

			return configurator;
		}

		[NotNull]
		private static T GetFromFactory<T>([NotNull] Func<T> factory)
		{
			if (factory == null) throw new ArgumentNullException(nameof(factory));

			var result = factory.Invoke();
			if (result == null)
				throw new AppConstructingException();

			return result;
		}

		[NotNull]
		private static ConfigurationPhase GetPhase([NotNull] this IAppConfigurator configurator, string phaseName)
		{
			var phase = configurator.FindPhase(phaseName);
			if (phase == null)
				throw new AppConstructingException();

			return phase;
		}

		[CanBeNull]
		private static ConfigurationPhase FindPhase([NotNull] this IAppConfigurator configurator, string phaseName)
		{
			return configurator.Configuration.Phases.FindByName(phaseName);
		}

		[NotNull]
		private static T GetObjectFromContext<T>(NamedObjectsHeap context, string key)
		{
			return context.GetObject<IFrameworkLogger>(key) ?? throw new AppConstructingException();
		}
	}
}
