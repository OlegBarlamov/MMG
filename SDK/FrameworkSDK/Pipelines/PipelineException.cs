﻿using System;

namespace FrameworkSDK.Pipelines
{
    public class PipelineException : FrameworkException
    {
        internal PipelineException(string message)
            : base(message)
        {
        }

        internal PipelineException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}