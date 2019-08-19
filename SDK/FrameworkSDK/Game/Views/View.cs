﻿using System;
using System.Collections.Generic;
using FrameworkSDK.Common;
using FrameworkSDK.Game.Controllers;
using FrameworkSDK.Game.Scenes;
using FrameworkSDK.Localization;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;

namespace FrameworkSDK.Game.Views
{
    public abstract class View : IView
    {
        public string Name { get; protected set; }

        Scene ISceneComponent.OwnedScene => _ownedScene;

        object IView.DataModel => _dataModel;

        IController IView.Controller => _controller;

        private object _dataModel;
        private IController _controller;
        private Scene _ownedScene;

        private readonly List<IView> _children = new List<IView>();

        protected View()
            : this(NamesGenerator.Hash(HashType.SmallGuid, nameof(View)))
        {
        }

        protected View([NotNull] string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public void Destroy()
        {
            throw new NotImplementedException();
        }

	    public override string ToString()
	    {
		    return Name;
	    }

	    public virtual void Draw(GameTime gameTime)
        {

        }

        protected virtual void SetDataModel([NotNull] object dataModel)
        {
            _dataModel = dataModel ?? throw new ArgumentNullException(nameof(dataModel));
        }

        protected virtual void SetController([NotNull] IController controller)
        {
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        protected virtual void Initialize([NotNull] Scene scene)
        {

        }

        protected virtual void OnDetached([NotNull] Scene scene)
        {

        }

        protected void AddChild([NotNull] IView childView)
        {
            if (childView == null) throw new ArgumentNullException(nameof(childView));

            var scene = _ownedScene;
            if (scene == null)
                throw new ScenesException(Strings.Exceptions.Scenes.SceneComponentNotAttached, this);

            scene.AddView(childView);
            _children.Add(childView);
        }

        protected void RemoveChild([NotNull] IView childView)
        {
            if (childView == null) throw new ArgumentNullException(nameof(childView));

            var scene = _ownedScene;
            if (scene == null)
                throw new ScenesException(Strings.Exceptions.Scenes.SceneComponentNotAttached, this);

            if (!_children.Contains(childView))
                throw new ScenesException(Strings.Exceptions.Scenes.ChildComponentNotExists, this, childView);

            scene.RemoveView(childView);
            _children.Remove(childView);
        }

        void ISceneComponent.OnAddedToScene(Scene scene)
        {
            _ownedScene = scene ?? throw new ArgumentNullException(nameof(scene));

            Initialize(_ownedScene);
        }

        void ISceneComponent.OnRemovedFromScene(Scene scene)
        {
            foreach (var child in _children)
                RemoveChild(child);

            _ownedScene = null;
            OnDetached(scene);
        }

        void IView.SetDataModel(object dataModel)
        {
            if (_dataModel == null)
                SetDataModel(dataModel);
        }

        void IView.SetController(IController controller)
        {
            if (_controller == null)
                SetController(controller);
        }
    }
}
