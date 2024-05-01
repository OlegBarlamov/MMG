using System;
using FrameworkSDK.MonoGame.InputManagement;
using FrameworkSDK.MonoGame.Mvc;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Omegas.Client.MacOs.Models.SphereObject;
using Omegas.Client.MacOs.Services;

namespace Omegas.Client.MacOs.Models
{
    public class PlayerController : SphereObjectGenericController<PlayerData>
    {
        private IInputService InputService { get; }
        public OmegaGameService OmegaGameService { get; }

        private IPlayerGamePadProvider _gamePadProvider;

        private SphereObjectData _bullet;
        private Vector2 _heartOrigin;
        private Vector2 _heartOriginNormal;
        private float _fillFactor = 1f;

        public PlayerController([NotNull] IInputService inputService, [NotNull] OmegaGameService omegaGameService) : base(omegaGameService)
        {
            InputService = inputService ?? throw new ArgumentNullException(nameof(inputService));
            OmegaGameService = omegaGameService ?? throw new ArgumentNullException(nameof(omegaGameService));
        }
        
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            if (_bullet != null)
            {
                var bulletPosition = GetBulletPosition(_bullet.Size);
                _bullet.SetPosition(bulletPosition);
            }
            
            if (_gamePadProvider.IsConnected)
            {
                UpdateLeftThumbStick();

                if (_gamePadProvider.IsButtonDown(Buttons.A) && _bullet != null)
                {
                    OmegaGameService.FillBullet(DataModel, _bullet, _fillFactor, gameTime);
                    _fillFactor += gameTime.ElapsedGameTime.Milliseconds * 0.001f;
                }
                else
                {
                    _fillFactor = 1f;
                }
                
                if (_gamePadProvider.IsButtonPressedOnce(Buttons.A) && _heartOrigin != Vector2.Zero)
                {
                    _bullet = OmegaGameService.CreateBulletWorkpiece(DataModel, _heartOriginNormal);
                }

                if (_gamePadProvider.IsButtonReleasedOnce(Buttons.A) && _bullet != null)
                {
                    if (_heartOrigin == Vector2.Zero)
                    {
                        OmegaGameService.ReleaseBullet(DataModel, _bullet, _heartOriginNormal, _heartOriginNormal);
                        _bullet = null;
                        //OmegaGameService.CancelBullet(DataModel, _bullet);
                        //_bullet = null;
                    }
                    else
                    {
                        OmegaGameService.ReleaseBullet(DataModel, _bullet, _heartOrigin, _heartOriginNormal);
                        _bullet = null;
                    }
                }
            }
        }

        private Vector2 GetBulletPosition(float bulletSize)
        {
            return DataModel.Position + _heartOriginNormal * (DataModel.Size + bulletSize);
        }

        private void UpdateLeftThumbStick()
        {
            var thumbSticksLeft = _gamePadProvider.ThumbSticks.Left;
            if (thumbSticksLeft != Vector2.Zero)
            {
                thumbSticksLeft.Normalize();

                _heartOrigin = thumbSticksLeft * new Vector2(1, -1);
                DataModel.SetHeartRelativePosition(_heartOrigin * (DataModel.Size - DataModel.HeartSize / 2));
                _heartOriginNormal = Vector2.Normalize(_heartOrigin); ;
            }
            else
            {
                _heartOrigin = Vector2.Zero;

                if (DataModel.HeartRelativePosition != Vector2.Zero)
                {
                    DataModel.SetHeartRelativePosition(Vector2.Zero);
                }
            }
        }

        protected override void OnAttached(SceneBase scene)
        {
            base.OnAttached(scene);

            _gamePadProvider = InputService.GamePads.GetGamePad(DataModel.PlayerIndex);
        }
    }
}