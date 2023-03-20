using System;
using FrameworkSDK.Logging;
using FrameworkSDK.MonoGame.Core;
using FrameworkSDK.MonoGame.Graphics.Basic;
using FrameworkSDK.MonoGame.Graphics.Camera2D;
using FrameworkSDK.MonoGame.Graphics.Camera3D;
using FrameworkSDK.MonoGame.Graphics.GraphicsPipeline.Processing;
using FrameworkSDK.MonoGame.Graphics.RenderingTools;
using FrameworkSDK.MonoGame.Graphics.Services;
using FrameworkSDK.MonoGame.Services;
using JetBrains.Annotations;
using NetExtensions.Collections;

// ReSharper disable once CheckNamespace
namespace FrameworkSDK.MonoGame.Graphics.GraphicsPipeline
{
    [UsedImplicitly]
    internal class GraphicsPipelineFactoryService : IGraphicsPipelineFactoryService
    {
        [NotNull] private IGameHeartServices GameHeartServices { get; }
        [NotNull] private IRenderTargetsFactoryService RenderTargetsFactoryService { get; }
        [NotNull] private IDisplayService DisplayService { get; }
        [NotNull] private IGraphicsPipelinePassAssociateService GraphicsPipelinePassAssociateService { get; }
        [NotNull] private IFrameworkLogger FrameworkLogger { get; }
        [NotNull] private ICamera3DProvider Camera3DProvider { get; }
        [NotNull] private IDebugInfoService DebugInfoService { get; }
        [NotNull] private IIndicesBuffersFiller IndicesBuffersFiller { get; }
        [NotNull] private IVideoBuffersFactoryService VideoBuffersFactoryService { get; }
        [NotNull] private ICamera2DProvider Camera2DProvider { get; }

        public GraphicsPipelineFactoryService(
            [NotNull] IGameHeartServices gameHeartServices,
            [NotNull] IRenderTargetsFactoryService renderTargetsFactoryService,
            [NotNull] IDisplayService displayService,
            [NotNull] IGraphicsPipelinePassAssociateService graphicsPipelinePassAssociateService,
            [NotNull] IFrameworkLogger frameworkLogger,
            [NotNull] ICamera3DProvider camera3DProvider,
            [NotNull] IDebugInfoService debugInfoService,
            [NotNull] IIndicesBuffersFiller indicesBuffersFiller,
            [NotNull] IVideoBuffersFactoryService videoBuffersFactoryService,
            [NotNull] ICamera2DProvider camera2DProvider)
        {
            GameHeartServices = gameHeartServices ?? throw new ArgumentNullException(nameof(gameHeartServices));
            RenderTargetsFactoryService = renderTargetsFactoryService ?? throw new ArgumentNullException(nameof(renderTargetsFactoryService));
            DisplayService = displayService ?? throw new ArgumentNullException(nameof(displayService));
            GraphicsPipelinePassAssociateService = graphicsPipelinePassAssociateService ?? throw new ArgumentNullException(nameof(graphicsPipelinePassAssociateService));
            FrameworkLogger = frameworkLogger ?? throw new ArgumentNullException(nameof(frameworkLogger));
            Camera3DProvider = camera3DProvider ?? throw new ArgumentNullException(nameof(camera3DProvider));
            DebugInfoService = debugInfoService ?? throw new ArgumentNullException(nameof(debugInfoService));
            IndicesBuffersFiller = indicesBuffersFiller ?? throw new ArgumentNullException(nameof(indicesBuffersFiller));
            VideoBuffersFactoryService = videoBuffersFactoryService ?? throw new ArgumentNullException(nameof(videoBuffersFactoryService));
            Camera2DProvider = camera2DProvider ?? throw new ArgumentNullException(nameof(camera2DProvider));
        }

        public IGraphicsPipelineBuilder Create(IReadOnlyObservableList<IGraphicComponent> graphicComponents)
        {
            return new GraphicsPipelineBuilder(
                graphicComponents,
                GraphicsPipelinePassAssociateService,
                FrameworkLogger,
                DebugInfoService,
                RenderTargetsFactoryService,
                VideoBuffersFactoryService
                );
        }

        public IGraphicDeviceContext CreateGraphicDeviceContext()
        {
            return new PipelineGraphicDeviceContext(
                GameHeartServices.SpriteBatch,
                GameHeartServices.GraphicsDeviceManager.GraphicsDevice,
                DisplayService,
                Camera3DProvider,
                DebugInfoService,
                IndicesBuffersFiller,
                Camera2DProvider
                );
        }
    }
}