﻿using FrameworkSDK.DependencyInjection;
using FrameworkSDK.MonoGame.Config;
using FrameworkSDK.MonoGame.Core;
using FrameworkSDK.MonoGame.ExternalComponents;
using FrameworkSDK.MonoGame.Graphics.Camera2D;
using FrameworkSDK.MonoGame.Graphics.Camera3D;
using FrameworkSDK.MonoGame.Graphics.GraphicsPipeline;
using FrameworkSDK.MonoGame.Graphics.GraphicsPipeline.Processing;
using FrameworkSDK.MonoGame.Graphics.Services;
using FrameworkSDK.MonoGame.InputManagement;
using FrameworkSDK.MonoGame.InputManagement.Implementations;
using FrameworkSDK.MonoGame.Mvc;
using FrameworkSDK.MonoGame.Resources;
using FrameworkSDK.MonoGame.Resources.Generation;
using FrameworkSDK.MonoGame.Services.Implementations;

namespace FrameworkSDK.MonoGame.Services
{
    internal class GameModule<TGame> : IServicesModule where TGame : GameApp
    {
        public void RegisterServices(IServiceRegistrator serviceRegistrator)
        {
            serviceRegistrator.RegisterType<GameApp, TGame>();
            serviceRegistrator.RegisterType<IExternalGameComponentsService, FakeExternalGameComponentsService>();
            serviceRegistrator.RegisterType<IAppTerminator, DefaultAppTerminator>();
            
            //Graphics
            serviceRegistrator.RegisterType<IGraphicsPipelineFactoryService, GraphicsPipelineFactoryService>();
            serviceRegistrator.RegisterType<IGraphicsPipelinePassAssociateService, GraphicsPipelinePassAssociateService>();
            serviceRegistrator.RegisterType<IRenderTargetsFactoryService, RenderTargetsFactoryService>();
            serviceRegistrator.RegisterType<IDisplayService, DisplayService>();
            serviceRegistrator.RegisterType<IIndicesBuffersFactory, Int16IndicesBuffersFactory>();
            serviceRegistrator.RegisterType<IIndicesBuffersFiller, Int16IndicesBuffersFiller>();
            serviceRegistrator.RegisterType<IVideoBuffersFactoryService, VideoBuffersFactoryService>();

            //Resources
            serviceRegistrator.RegisterType<IContentContainersFactory, ContentContainersFactory>();
            serviceRegistrator.RegisterType<IResourceReferencesService, ResourceReferencesService>();
            serviceRegistrator.RegisterType<IResourcesService, ResourcesService>();
            serviceRegistrator.RegisterType<ITextureGeneratorApi, TextureGeneratorApi>();
            serviceRegistrator.RegisterType<ITextureGeneratorService, TextureGeneratorServicePublic>();
            serviceRegistrator.RegisterType<IRenderTargetsFactory, RenderTargetsFactory>();
            serviceRegistrator.RegisterType<IDefaultResourcesService, DefaultResourcesService>();
         
            //Mvc
            serviceRegistrator.RegisterType<IScenesController, ScenesController>();
            serviceRegistrator.RegisterType<ICurrentSceneProvider, CurrentSceneProvider>();
            serviceRegistrator.RegisterType<IMvcStrategyService, EmptyMvcStrategyService>();

            //Input
            serviceRegistrator.RegisterType<InputService, InputService>();
            serviceRegistrator.RegisterFactory(typeof(IInputService), (locator, type) => locator.Resolve(typeof(InputService)));
            serviceRegistrator.RegisterType<IInputManager, InputManager>();

            //Camera
            serviceRegistrator.RegisterType<DefaultCamera3DService, DefaultCamera3DService>();
            serviceRegistrator.RegisterFactory(typeof(ICamera3DService), (locator, type) => locator.Resolve(typeof(DefaultCamera3DService)));
            serviceRegistrator.RegisterFactory(typeof(ICamera3DProvider), (locator, type) => locator.Resolve(typeof(DefaultCamera3DService)));
            serviceRegistrator.RegisterType<DefaultCamera2DService, DefaultCamera2DService>();
            serviceRegistrator.RegisterFactory(typeof(ICamera2DService), (locator, type) => locator.Resolve(typeof(DefaultCamera2DService)));
            serviceRegistrator.RegisterFactory(typeof(ICamera2DProvider), (locator, type) => locator.Resolve(typeof(DefaultCamera2DService)));
            
            serviceRegistrator.RegisterType<IDebugInfoService, DefaultDebugInfoService>();
            serviceRegistrator.RegisterType<IGameParameters, DefaultGameParameters>();
            serviceRegistrator.RegisterType<AppStateService, AppStateService>();
            serviceRegistrator.RegisterFactory(typeof(IAppStateService), (locator, type) => locator.Resolve(typeof(AppStateService)));
            serviceRegistrator.RegisterType<IGameHeartServices, GameHeartServicesHolder>();
            serviceRegistrator.RegisterType<IGameHeart, GameHeart>();
        }
    }
}