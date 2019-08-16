﻿using System;

namespace FrameworkSDK.Game.Mapping
{
    public class IncompatibleControllerTypeException : MappingException
    {
	    internal IncompatibleControllerTypeException(string message)
            : base(message)
        {
        }

	    internal IncompatibleControllerTypeException(string message, Exception inner)
            : base(message, inner)
        {
        }

        internal IncompatibleControllerTypeException(string message, Exception inner, params object[] args)
            : this(string.Format(message, args), inner)
        {
        }

        internal IncompatibleControllerTypeException(string message, params object[] args)
            : this(string.Format(message, args))
        {
        }
    }
}
