using System;
using FrameworkSDK.MonoGame.Graphics;
using FrameworkSDK.MonoGame.Mvc;
using FrameworkSDK.MonoGame.Services;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FrameworkSDK.MonoGame.SceneComponents
{
    public class BackgroundTextureComponentDataModel
    {
        public Texture2D Texture { get; set; } 
    }
    
    public class BackgroundTextureComponent : View<BackgroundTextureComponentDataModel>
    {
        private IDisplayService DisplayService { get; }

        // ReSharper disable once UnusedParameter.Local
        public BackgroundTextureComponent([NotNull] BackgroundTextureComponentDataModel dataModel, [NotNull] IDisplayService displayService)
        {
            DisplayService = displayService ?? throw new ArgumentNullException(nameof(displayService));
        }
        
        public override void Draw(GameTime gameTime, IDrawContext context)
        {
            context.Draw(DataModel.Texture, DisplayService.GraphicsDevice.Viewport.Bounds, Color.White);
            
            base.Draw(gameTime, context);
        }
    }
}