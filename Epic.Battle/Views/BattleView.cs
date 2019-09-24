﻿using Epic.Battle.Controllers;
using Epic.Battle.Models;
using FrameworkSDK.MonoGame.GameStructure.Scenes;
using FrameworkSDK.MonoGame.GameStructure.Views;

namespace Epic.Battle.Views
{
    public class BattleView : View<BattleModel, BattleController>
    {
        protected override void Initialize(Scene scene)
        {
            base.Initialize(scene);

            AddChild(DataModel.Field);
        }
    }
}
