﻿using System;
using System.Collections.Generic;
using FrameworkSDK.Common;
using FrameworkSDK.Localization;
using FrameworkSDK.MonoGame.GameStructure.Scenes;
using FrameworkSDK.MonoGame.GameStructure.Views;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;

namespace FrameworkSDK.MonoGame.GameStructure.Controllers
{
    public abstract class Controller : IController
    {
        public string Name { get; protected set; }

        IView IController.View => _view;

        object IController.Model => _model;
        
        private object _model;
        private IView _view;

        Scene ISceneComponent.OwnedScene => _ownedScene;

        private Scene _ownedScene;

        private readonly List<IController> _children = new List<IController>();

        protected Controller() : this(NamesGenerator.Hash(HashType.SmallGuid, nameof(Controller)))
        {
        }

        protected Controller([NotNull] string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public virtual void Update(GameTime gameTime)
        {
        }

	    public override string ToString()
	    {
		    return Name;
	    }

	    bool IController.IsOwnedModel(object model)
        {
            return ReferenceEquals(model, _model);
        }

        protected virtual void SetModel([NotNull] object model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
        }

        protected virtual void SetView([NotNull] IView view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        protected virtual void Initialize([NotNull] Scene scene)
        {

        }

        protected virtual void OnDetached([NotNull] Scene scene)
        {

        }

        protected void AddChild(IController controller)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));

            var scene = _ownedScene;
            if (scene == null)
                throw new ScenesException(Strings.Exceptions.Scenes.SceneComponentNotAttached, this);

            scene.AddController(controller);
            _children.Add(controller);
        }

        protected void RemoveChild(IController controller)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));

            var scene = _ownedScene;
            if (scene == null)
                throw new ScenesException(Strings.Exceptions.Scenes.SceneComponentNotAttached, this);

            if (!_children.Contains(controller))
                throw new ScenesException(Strings.Exceptions.Scenes.ChildComponentNotExists, this, controller);

            scene.RemoveController(controller);
            _children.Remove(controller);
        }

        protected void AddChild(object model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var scene = _ownedScene;
            if (scene == null)
                throw new ScenesException(Strings.Exceptions.Scenes.SceneComponentNotAttached, this);

            var controller = scene.AddController(model);
            _children.Add(controller);
        }

        protected void RemoveChild(object model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var scene = _ownedScene;
            if (scene == null)
                throw new ScenesException(Strings.Exceptions.Scenes.SceneComponentNotAttached, this);

            var targetController = scene.FindControllerByActiveModel(model);
            if (targetController == null)
                throw new ScenesException(Strings.Exceptions.Scenes.ControllerForModelNotExists, model, scene);

            if (!_children.Contains(targetController))
                throw new ScenesException(Strings.Exceptions.Scenes.ChildComponentNotExists, this, targetController);

            var removedController = scene.RemoveController(model);
            _children.Remove(removedController);
        }

        void IController.SetModel(object model)
        {
            if (_model == null)
                SetModel(model);
        }

        void IController.SetView(IView view)
        {
            if (_view == null)
                SetView(view);
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
    }
}