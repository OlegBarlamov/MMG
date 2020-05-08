﻿using System;
using System.Text;
using FrameworkSDK.MonoGame.Resources;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FrameworkSDK.MonoGame.Graphics.RenderingTools
{
    public class PipelineGraphicDeviceContext : IGraphicDeviceContext
    {
        private SpriteBatch SpriteBatch { get; }
        private GraphicsDevice GraphicsDevice { get; }

        private readonly IDrawContext _internalDrawContext;

        public void Dispose()
        {
            _internalDrawContext.Dispose();
        }
        
        public PipelineGraphicDeviceContext([NotNull] SpriteBatch spriteBatch, [NotNull] GraphicsDevice graphicsDevice)
        {
            SpriteBatch = spriteBatch ?? throw new ArgumentNullException(nameof(spriteBatch));
            GraphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));
            
            _internalDrawContext = new DrawContext(SpriteBatch);
        }

        public void BeginDraw(SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null,
            SamplerState samplerState = null, DepthStencilState depthStencilState = null,
            RasterizerState rasterizerState = null, Effect effect = null, Matrix? transformMatrix = null)
        {
            SpriteBatch.Begin(sortMode,blendState,samplerState,depthStencilState,rasterizerState,effect,transformMatrix);
        }

        public void EndDraw()
        {
            SpriteBatch.End();
        }

        public void SetRenderTarget(RenderTarget2D renderTarget2D)
        {
            if (renderTarget2D == null) throw new ArgumentNullException(nameof(renderTarget2D));
            
            GraphicsDevice.SetRenderTarget(renderTarget2D);
        }

        public void Clear(Color color)
        {
            GraphicsDevice.Clear(color);
        }

        public void Draw(Texture2D texture, Vector2? position = null, Rectangle? destinationRectangle = null,
            Rectangle? sourceRectangle = null, Vector2? origin = null, float rotation = 0, Vector2? scale = null,
            Color? color = null, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0)
        {
            _internalDrawContext.Draw(texture, position, destinationRectangle, sourceRectangle, origin, rotation, scale, color, effects, layerDepth);
        }

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin,
            Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            _internalDrawContext.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin,
            float scale, SpriteEffects effects, float layerDepth)
        {
            _internalDrawContext.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth);
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation,
            Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            _internalDrawContext.Draw(texture, destinationRectangle, sourceRectangle, color, rotation, origin, effects, layerDepth);
        }

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            _internalDrawContext.Draw(texture, position, sourceRectangle, color);
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            _internalDrawContext.Draw(texture, destinationRectangle, sourceRectangle, color);
        }

        public void Draw(Texture2D texture, Vector2 position, Color color)
        {
            _internalDrawContext.Draw(texture, position, color);
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
        {
            _internalDrawContext.Draw(texture, destinationRectangle, color);
        }

        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            _internalDrawContext.DrawString(spriteFont, text, position, color);
        }

        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin,
            float scale, SpriteEffects effects, float layerDepth)
        {
            _internalDrawContext.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin,
            Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            _internalDrawContext.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color)
        {
            _internalDrawContext.DrawString(spriteFont, text, position, color);
        }

        public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation,
            Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            _internalDrawContext.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }

        public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation,
            Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            _internalDrawContext.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
        }
    }
}
