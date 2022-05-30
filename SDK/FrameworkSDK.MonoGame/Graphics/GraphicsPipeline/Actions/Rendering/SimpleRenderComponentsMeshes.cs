using System;
using System.Collections.Generic;
using FrameworkSDK.MonoGame.Graphics.Basic;
using FrameworkSDK.MonoGame.Graphics.RenderingTools;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FrameworkSDK.MonoGame.Graphics.GraphicsPipeline
{
    public class SimpleRenderComponentsMeshes<TVertexType> : GraphicsPipelineActionBase
        where TVertexType : struct, IVertexType
    {
        public Effect Effect { get; }
        public VertexBuffer VertexBuffer { get; }
        public IndexBuffer IndexBuffer { get; }

        public SimpleRenderComponentsMeshes([NotNull] string name,
            [NotNull] Effect effect, [NotNull] VertexBuffer vertexBuffer,
            [NotNull] IndexBuffer indexBuffer) : base(name)
        {
            Effect = effect ?? throw new ArgumentNullException(nameof(effect));
            VertexBuffer = vertexBuffer ?? throw new ArgumentNullException(nameof(vertexBuffer));
            IndexBuffer = indexBuffer ?? throw new ArgumentNullException(nameof(indexBuffer));
        }
        
        private int _index;
        private int _meshesIndex;
        private IReadOnlyList<IRenderableMesh> _meshes;
        private IRenderableMesh _mesh;
        private IGraphicComponent _component;

        public override void Process(GameTime gameTime, IGraphicDeviceContext graphicDeviceContext,
            IReadOnlyList<IGraphicComponent> associatedComponents)
        {
            for (_index = 0; _index < associatedComponents.Count; _index++)
            {
                _component = associatedComponents[_index];
                if (!IsComponentVisibleByActiveCamera(graphicDeviceContext, _component))
                    continue;
                
                _meshes = associatedComponents[_index].MeshesByPass[Name];

                for (_meshesIndex = 0; _meshesIndex < _meshes.Count; _meshesIndex++)
                {
                    foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
                    {
                        _mesh = _meshes[_meshesIndex];

                        ((IEffectMatrices) Effect).World = _mesh.World;
                        pass.Apply();

                        VertexBuffer.SetData((TVertexType[]) _mesh.Geometry.GetVertices());
                        IndexBuffer.SetData(_mesh.Geometry.GetIndices());

                        graphicDeviceContext.GraphicsDevice.SetVertexBuffer(VertexBuffer);
                        graphicDeviceContext.GraphicsDevice.Indices = IndexBuffer;

                        graphicDeviceContext.GraphicsDevice.DrawIndexedPrimitives(_mesh.Geometry.PrimitiveType, 0, 0,
                            _mesh.Geometry.GetPrimitivesCount());
                    }
                    
                    graphicDeviceContext.DebugInfoService.IncrementCounter(DebugInfoRenderingMeshes);
                }
                
                graphicDeviceContext.DebugInfoService.IncrementCounter(DebugInfoRenderingComponents);
            }
        }
    }
}