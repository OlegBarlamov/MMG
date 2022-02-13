﻿using System;
using FrameworkSDK.DependencyInjection;
using FrameworkSDK.MonoGame.Localization;
using FrameworkSDK.Logging;
using JetBrains.Annotations;

namespace FrameworkSDK.MonoGame
{
    /// <summary>
    /// Use only in static context.
    /// </summary>
    internal static class AppContext
    {
        public static IFrameworkLogger Logger => _logger ?? throw new FrameworkMonoGameException(Strings.Exceptions.AppContextNotInitialized);
        public static IServiceLocator ServiceLocator => _serviceLocator ?? throw new FrameworkMonoGameException(Strings.Exceptions.AppContextNotInitialized);

        private static IServiceLocator _serviceLocator;
        private static IFrameworkLogger _logger;

        internal static void Initialize([NotNull] IFrameworkLogger logger, [NotNull] IServiceLocator serviceLocator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceLocator = serviceLocator ?? throw new ArgumentNullException(nameof(serviceLocator));
        }
    }
}