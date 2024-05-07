using System;
using FrameworkSDK.MonoGame.Resources.Generation;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FrameworkSDK.MonoGame.Graphics.GraphicsPipeline.Presets
{
    public class GraphicsPipeline2DDrawingPreset : GraphicPipelineBuilderWrapper, IGraphicsPipelineBuilder, IDisposable
    {
        public static class PipelineActions
        {
            public static string DrawComponents = "Default";
            public static string DrawDebugComponents = "Debug";
            public static string DrawUI = "UI";
            public static string DrawDebugUI = "Debug_UI";
        }
        
        public BeginDrawConfig BeginDrawConfig { get; }

        private readonly IRenderTargetWrapper _defaultGraphicsPipelineRenderTarget;

        internal GraphicsPipeline2DDrawingPreset([NotNull] IGraphicsPipelineBuilder builder, [NotNull] BeginDrawConfig beginDrawConfig, Color clearColor)
            : base(builder)
        {
            BeginDrawConfig = beginDrawConfig ?? throw new ArgumentNullException(nameof(beginDrawConfig));
            
            _defaultGraphicsPipelineRenderTarget = Builder.RenderTargetsFactoryService.CreateDisplaySizedRenderTarget(
                size => size,
                false,
                SurfaceFormat.Color,
                DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

            Builder
                .SetRenderTarget(_defaultGraphicsPipelineRenderTarget)
                .Clear(clearColor)
                .BeginDraw(BeginDrawConfig)
                .DrawComponents(PipelineActions.DrawComponents)
                .DrawComponents(PipelineActions.DrawDebugComponents)
                .DrawComponents(context => context.Camera2DService.GetScreenCamera(), PipelineActions.DrawUI)
                .DrawComponents(context => context.Camera2DService.GetScreenCamera(), PipelineActions.DrawDebugUI);
        }

        // TODO do I need it?
        public void AddActionBeforeFinalDraw(IGraphicsPipelineAction action)
        {
            Builder.AddAction(action);
        }

        public override IGraphicsPipeline Build(IDisposable resources = null)
        {
            return Builder
                .EndDraw()
                .DrawRenderTargetToDisplay(_defaultGraphicsPipelineRenderTarget)
                .Build(this);
        }

        public void Dispose()
        {
            _defaultGraphicsPipelineRenderTarget.Dispose();
        }
    }
}