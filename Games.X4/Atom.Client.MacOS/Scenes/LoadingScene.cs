using System;
using Atom.Client.MacOS.Resources;
using FrameworkSDK.MonoGame.Mvc;
using FrameworkSDK.MonoGame.SceneComponents;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;

namespace Atom.Client.MacOS.Scenes
{
    [UsedImplicitly]
    public class LoadingScene : Scene
    {
        private LoadingSceneResources Resources { get; }

        private readonly BackgroundTextureComponent _background;
        private readonly DrawLabelComponent _loadingLabel;

        public LoadingScene([NotNull] LoadingSceneResources resources) : base(nameof(LoadingScene), resources)
        {
            Resources = resources ?? throw new ArgumentNullException(nameof(resources));

            _background = (BackgroundTextureComponent) AddView(new BackgroundTextureComponentDataModel
            {
                Texture = Resources.LoadingSceneBackgroundTexture
            });

            _loadingLabel = (DrawLabelComponent) AddView(new DrawLabelComponentDataModel
            {
                Font = Resources.Font,
                Color = Color.White,
                Position = new Vector2(10, 10),
                Text = "Loading..."
            });
        }
    }
}