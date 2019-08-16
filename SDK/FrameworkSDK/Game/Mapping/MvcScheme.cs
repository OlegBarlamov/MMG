﻿using FrameworkSDK.Game.Controllers;
using FrameworkSDK.Game.Views;
using JetBrains.Annotations;

namespace FrameworkSDK.Game.Mapping
{
    public interface IMvcScheme
    {
        [CanBeNull] object Model { get; }
        [CanBeNull] IController Controller { get; }
        [CanBeNull] IView View { get; }
    }

    public class MvcScheme : IMvcScheme
    {
        [CanBeNull] public object Model { get; set; }
        [CanBeNull] public IController Controller { get; set; }
        [CanBeNull] public IView View { get; set; }
    }
}