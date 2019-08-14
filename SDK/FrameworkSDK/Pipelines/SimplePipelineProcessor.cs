﻿using System;
using JetBrains.Annotations;
using NetExtensions;

namespace FrameworkSDK.Pipelines
{
    public sealed class SimplePipelineProcessor : PipelineProcessor, IPipelineProcessor
    {
        protected override void OnPhaseActionProcessFailed(IPipelineAction action, PipelineStep phase, Exception error)
        {
            if (action.IsCritical)
                throw error;
        }

        protected override void OnPhaseProcessFailed(PipelineStep phase, Exception error)
        {
            throw error;
        }

        public SimplePipelineProcessor([NotNull] PipelineContext context)
            : base(context)
        {
        }

        public SimplePipelineProcessor()
            : base(new PipelineContext(new NamedObjectsHeap<object>()))
        {
        }
    }
}