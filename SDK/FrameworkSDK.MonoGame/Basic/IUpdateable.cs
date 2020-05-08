﻿using Microsoft.Xna.Framework;

namespace FrameworkSDK.MonoGame.Basic
{
    public interface IUpdateable
    {
        void Update(GameTime gameTime);
    }

	public interface IUpdatable<out TState>
	{
		TState UpdateState(GameTime gameTime);
	}
}
