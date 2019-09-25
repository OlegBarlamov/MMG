﻿using FrameworkSDK.Logging;
using FrameworkSDK.MonoGame.Mvc;
using JetBrains.Annotations;

namespace FrameworkSDK.MonoGame.Mvc
{
    public class MvcScheme : IMvcScheme
    {
        [CanBeNull] public object Model { get; set; }
        [CanBeNull] public IController Controller { get; set; }
        [CanBeNull] public IView View { get; set; }

	    public override string ToString()
	    {
		    return string.Format(NullFormatProvider.Instance, "{0}<->{1}<->{2}", View, Controller, Model);
	    }
    }
}