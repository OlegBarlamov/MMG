using System;
using System.Collections.Generic;
using FrameworkSDK.Logging;
using FrameworkSDK.MonoGame.Graphics.Basic;
using FrameworkSDK.MonoGame.Graphics.GraphicsPipeline.Processing;
using FrameworkSDK.MonoGame.Graphics.RenderingTools;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using NetExtensions.Collections;

namespace FrameworkSDK.MonoGame.Graphics.GraphicsPipeline
{
    internal class GraphicsPipeline : IGraphicsPipeline
    {
        public static IGraphicsPipeline Empty { get; } = new EmptyGraphicsPipeline();

        public IReadOnlyList<IGraphicsPipelineAction> Actions { get; }
        private IReadOnlyObservableList<IGraphicComponent> GraphicComponents { get; }
        private IGraphicsPipelinePassAssociateService PipelinePassAssociateService { get; }

        private readonly ModuleLogger _logger;
        
        private readonly Dictionary<string, List<IGraphicsPipelineAction>> _actionsByPassesMap = new Dictionary<string, List<IGraphicsPipelineAction>>();
        private bool _disposed;

        public GraphicsPipeline(
            [NotNull] IReadOnlyList<IGraphicsPipelineAction> actions,
            [NotNull] IReadOnlyObservableList<IGraphicComponent> graphicComponents,
            [NotNull] IGraphicsPipelinePassAssociateService pipelinePassAssociateService,
            IFrameworkLogger frameworkLogger)
        {
            Actions = actions ?? throw new ArgumentNullException(nameof(actions));
            GraphicComponents = graphicComponents ?? throw new ArgumentNullException(nameof(graphicComponents));
            PipelinePassAssociateService = pipelinePassAssociateService ?? throw new ArgumentNullException(nameof(pipelinePassAssociateService));
            _logger = new ModuleLogger(frameworkLogger, LogCategories.Rendering);
            
            FillActionsMap();
            ProcessAlreadyExistedComponents();
            
            SubscribeToComponentsChanges(GraphicComponents);
        }

        private void FillActionsMap()
        {
            foreach (var graphicsPipelineAction in Actions)
            {
                var passName = graphicsPipelineAction.Name;
                if (string.IsNullOrWhiteSpace(passName)) 
                    continue;
                
                if (!_actionsByPassesMap.ContainsKey(passName)) 
                    _actionsByPassesMap.Add(passName, new List<IGraphicsPipelineAction>());
                
                _actionsByPassesMap[passName].Add(graphicsPipelineAction);
            }
        }

        private void SubscribeToComponentsChanges(IReadOnlyObservableList<IGraphicComponent> components)
        {
            components.ItemAdded += ComponentsOnItemAdded;
            components.ItemRemoved += ComponentsOnItemRemoved;
        }

        private void UnsubscribeToComponentsChanges(IReadOnlyObservableList<IGraphicComponent> components)
        {
            components.ItemAdded -= ComponentsOnItemAdded;
            components.ItemRemoved -= ComponentsOnItemRemoved;
        }
        
        private void ComponentsOnItemAdded(IGraphicComponent component)
        {
            var targetPasses = PipelinePassAssociateService.GetAssociatedPasses(component);
            foreach (var targetPass in targetPasses)
            {
                AddToMap(component, targetPass);
            }
        }
        
        private void ComponentsOnItemRemoved(IGraphicComponent component)
        {
            var targetPasses = PipelinePassAssociateService.GetAssociatedPasses(component);
            foreach (var targetPass in targetPasses)
            {
                RemoveFromMap(component, targetPass);
            }
        }
        
        private void ProcessAlreadyExistedComponents()
        {
            foreach (var component in GraphicComponents)
            {
                ComponentsOnItemAdded(component);
            }
        }
        
        private void AddToMap(IGraphicComponent component, string targetPass)
        {
            if (string.IsNullOrWhiteSpace(targetPass))
                return;

            if (_actionsByPassesMap.TryGetValue(targetPass, out var graphicsPipelineActions))
            {
                foreach (var graphicsPipelineAction in graphicsPipelineActions)
                {
                    graphicsPipelineAction.OnComponentAttached(component);
                }
            }
        }

        private void RemoveFromMap(IGraphicComponent component, string targetPass)
        {
            if (string.IsNullOrWhiteSpace(targetPass))
                return;
            
            if (_actionsByPassesMap.TryGetValue(targetPass, out var graphicsPipelineActions))
            {
                foreach (var graphicsPipelineAction in graphicsPipelineActions)
                {
                    graphicsPipelineAction.OnComponentDetached(component);
                }
            }
        }
        
        public void Dispose()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(GraphicsPipeline));
            _disposed = true;
            
            UnsubscribeToComponentsChanges(GraphicComponents);

            _actionsByPassesMap.Clear();

            foreach (var pipelineAction in Actions)
            {
                try
                {
                    pipelineAction.Dispose();
                }
                catch (Exception e)
                {
                    _logger.Error($"Graphics pipeline action {pipelineAction.Name} dispose error", e);
                }
            }
            
            _logger.Dispose();
        }

        private IGraphicsPipelineAction _iterableAction;
        private int _iterableIndex;
        public void Process(GameTime gameTime, IGraphicDeviceContext graphicDeviceContext)
        {
            for (_iterableIndex = 0; _iterableIndex < Actions.Count; _iterableIndex++)
            {
                _iterableAction = Actions[_iterableIndex];
                if (_iterableAction.IsDisabled)
                    continue;

                _iterableAction.Process(gameTime, graphicDeviceContext);
            }
        }
    }
}