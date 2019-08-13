﻿using FrameworkSDK.Configuration;
using FrameworkSDK.IoC;
using FrameworkSDK.IoC.Default;
using FrameworkSDK.Localization;
using FrameworkSDK.Logging;
using FrameworkSDK.Modules;
using JetBrains.Annotations;
using NetExtensions;

namespace FrameworkSDK.Constructing
{
    internal class DefaultConfigurationFactory
    {
        public PhaseConfiguration Create()
        {
            var configuration = new PhaseConfiguration();

	        var initializePhase = CreateInitializePhase();
	        var baseSetup = CreateBaseSetupPhase();
	        var registration = CreateRegistrationPhase();
	        var constructing = CreateConstructingPhase();
	        var run = CreateRunPhase();

			configuration.Phases.AddRange(
				initializePhase,
				baseSetup,
				registration,
				constructing,
				run);

			return configuration;
        }

	    private ConfigurationPhase CreateInitializePhase()
	    {
			var initializePhase = new ConfigurationPhase(DefaultConfigurationSteps.Initialization);
		    initializePhase.AddActions(
			    new SimpleConfigurationAction(
				    DefaultConfigurationSteps.InitializationActions.Localization, true,
				    context =>
				    {
					    var localization = new DefaultLocalization();
					    context.SetObject(DefaultConfigurationSteps.ContextKeys.Localization, localization);
				    }),
			    new SimpleConfigurationAction(
				    DefaultConfigurationSteps.InitializationActions.Logging, true,
				    context =>
				    {
					    var logger = new NullLogger();
					    context.SetObject(DefaultConfigurationSteps.ContextKeys.Logger, logger);
					}),
			    new SimpleConfigurationAction(
				    DefaultConfigurationSteps.InitializationActions.Ioc, true,
				    context =>
				    {
					    var serviceContainer = new DefaultServiceContainer();
					    context.SetObject(DefaultConfigurationSteps.ContextKeys.ServiceContainer, serviceContainer);
				    })
		    );
		    return initializePhase;
	    }

	    private ConfigurationPhase CreateBaseSetupPhase()
	    {
			var baseSetupPhase = new ConfigurationPhase(DefaultConfigurationSteps.BaseSetup);
		    baseSetupPhase.AddActions(
			    new SimpleConfigurationAction(
				    DefaultConfigurationSteps.BaseSetupActions.Setup, true,
				    context =>
				    {
					    Strings.Localization = GetObjectFromContext<ILocalization>(context, DefaultConfigurationSteps.ContextKeys.Localization);

					    var logger = GetObjectFromContext<IFrameworkLogger>(context, DefaultConfigurationSteps.ContextKeys.Logger);
						var serviceContainer = GetObjectFromContext<IFrameworkServiceContainer>(context, DefaultConfigurationSteps.ContextKeys.ServiceContainer);
						var moduleLogger = new ModuleLogger(logger, FrameworkLogModule.Application);

					    context.SetObject(DefaultConfigurationSteps.ContextKeys.Logger, moduleLogger);
						context.SetObject(DefaultConfigurationSteps.ContextKeys.ServiceContainer, serviceContainer);
						context.SetObject(DefaultConfigurationSteps.ContextKeys.BaseLogger, logger);

						moduleLogger.Info();
					})
		    );
		    return baseSetupPhase;
		}

	    private ConfigurationPhase CreateRegistrationPhase()
	    {
		    var registrationPhase = new ConfigurationPhase(DefaultConfigurationSteps.Registration);
		    registrationPhase.AddActions(
			    new SimpleConfigurationAction(
				    DefaultConfigurationSteps.RegistrationActions.Core, true,
				    context =>
				    {
					    var logger = context.GetObject<ModuleLogger>(DefaultConfigurationSteps.ContextKeys.Logger);
						logger?.Info(Strings.Info.DefaultServices);

					    var localization = GetObjectFromContext<ILocalization>(context, DefaultConfigurationSteps.ContextKeys.Localization);
					    var loggerService = GetObjectFromContext<IFrameworkLogger>(context, DefaultConfigurationSteps.ContextKeys.BaseLogger);
					    var serviceRegistrator = GetObjectFromContext<IServiceRegistrator>(context, DefaultConfigurationSteps.ContextKeys.ServiceContainer);

						var coreModule = new CoreModule(localization, loggerService);
						serviceRegistrator.RegisterModule(coreModule);
					})
			);
		    return registrationPhase;
	    }

	    private ConfigurationPhase CreateConstructingPhase()
	    {
			var registrationPhase = new ConfigurationPhase(DefaultConfigurationSteps.Constructing);
		    registrationPhase.AddActions(
			    new SimpleConfigurationAction(
				    DefaultConfigurationSteps.ConstructingActions.Core, true,
				    context =>
				    {
					    var logger = context.GetObject<ModuleLogger>(DefaultConfigurationSteps.ContextKeys.Logger);
					    logger?.Info(Strings.Info.ConstructingStart);

					    var serviceContainer = GetObjectFromContext<IFrameworkServiceContainer>(context, DefaultConfigurationSteps.ContextKeys.ServiceContainer);
						var loggerService = GetObjectFromContext<IFrameworkLogger>(context, DefaultConfigurationSteps.ContextKeys.BaseLogger);

					    var serviceLocator = serviceContainer.BuildContainer();
						context.SetObject(DefaultConfigurationSteps.ContextKeys.Locator, serviceLocator);

						AppContext.Initialize(loggerService, serviceLocator);

					    logger?.Info(Strings.Info.ConstructingEnd);
					})
		    );
		    return registrationPhase;
		}

	    private ConfigurationPhase CreateRunPhase()
	    {
			return new ConfigurationPhase(DefaultConfigurationSteps.Run);
	    }

		[NotNull]
	    private static T GetObjectFromContext<T>(NamedObjectsHeap context, string key)
	    {
			return context.GetObject<IFrameworkLogger>(key) ?? throw new AppConstructingException();
		}
	}
}
