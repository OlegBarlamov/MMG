﻿using System;
using FrameworkSDK.Pipelines;
using FrameworkSDK.IoC;
using FrameworkSDK.Localization;
using FrameworkSDK.Logging;
using JetBrains.Annotations;
using NetExtensions;

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
					context.Heap.SetValue(DefaultConfigurationSteps.ContextKeys.Localization, localization);
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
					context.Heap.SetValue(DefaultConfigurationSteps.ContextKeys.Logger, logger);
				}));

			return configurator;
		}

		public static IAppConfigurator SetupCustomIoc([NotNull] this IAppConfigurator configurator, [NotNull] Func<IServiceContainerFactory> serviceContainerFactoryCreator)
		{
			if (configurator == null) throw new ArgumentNullException(nameof(configurator));
			if (serviceContainerFactoryCreator == null) throw new ArgumentNullException(nameof(serviceContainerFactoryCreator));

			var initializationPhase = configurator.GetPhase(DefaultConfigurationSteps.Initialization);
			initializationPhase.AddOrReplace(new SimpleConfigurationAction(
				DefaultConfigurationSteps.InitializationActions.Ioc,
				true,
				context =>
				{
					var serviceContainerFactory = GetFromFactory(serviceContainerFactoryCreator);
					context.Heap.SetValue(DefaultConfigurationSteps.ContextKeys.Ioc, serviceContainerFactory);
				}));

			return configurator;
		}

		public static IAppConfigurator RegisterServices([NotNull] this IAppConfigurator configurator, [NotNull] Action<IServiceRegistrator> registerAction)
		{
			if (configurator == null) throw new ArgumentNullException(nameof(configurator));
			if (registerAction == null) throw new ArgumentNullException(nameof(registerAction));

			var initializationPhase = configurator.GetPhase(DefaultConfigurationSteps.ExternalRegistration);
			initializationPhase.AddAction(new SimpleConfigurationAction(
				DefaultConfigurationSteps.ExternalRegistrationActions.Registration,
				true,
				context =>
				{
					var serviceRegistrator = GetObjectFromContext<IServiceRegistrator>(context, DefaultConfigurationSteps.ContextKeys.Container);
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
		private static PipelineStep GetPhase([NotNull] this IAppConfigurator configurator, string phaseName)
		{
			var phase = configurator.FindPhase(phaseName);
			if (phase == null)
				throw new AppConstructingException();

			return phase;
		}

	    private static void InsertAfter([NotNull] this IAppConfigurator configurator, PipelineStep phase, PipelineStep insertingPhase)
	    {
            throw new NotImplementedException();
	    }

		[CanBeNull]
		private static PipelineStep FindPhase([NotNull] this IAppConfigurator configurator, string phaseName)
		{
			return configurator.ConfigurationPipeline.Steps.FindByName(phaseName);
		}

		[NotNull]
		private static T GetObjectFromContext<T>(IPipelineContext context, string key)
		{
			return context.Heap.GetObject<IFrameworkLogger>(key) ?? throw new AppConstructingException();
		}
	}
}
