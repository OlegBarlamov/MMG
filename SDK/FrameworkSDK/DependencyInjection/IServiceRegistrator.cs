﻿using System;
using JetBrains.Annotations;

namespace FrameworkSDK.DependencyInjection
{
    public interface IServiceRegistrator
    {
        void RegisterInstance([NotNull] Type serviceType, [NotNull] object instance);

        void RegisterType([NotNull] Type serviceType, [NotNull] Type implType, ResolveType resolveType = ResolveType.Singletone);

        void RegisterFactory([NotNull] Type serviceType, [NotNull] ServiceFactoryDelegate factory, ResolveType resolveType = ResolveType.Singletone);
    }
}
