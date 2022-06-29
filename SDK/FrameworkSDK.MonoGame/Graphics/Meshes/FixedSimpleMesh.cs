using System;
using FrameworkSDK.MonoGame.Graphics.Basic;
using FrameworkSDK.MonoGame.Graphics.Materials;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FrameworkSDK.MonoGame.Graphics.Meshes
{
    public sealed class FixedSimpleMesh : IRenderableMesh
    {
        public IRenderableComponent Parent { get; }
        
        public IMeshGeometry Geometry { get; }
        public IMeshMaterial Material { get; set; } = StaticMaterials.EmptyMaterial;

        public Matrix World { get; set; } = Matrix.Identity;

        public Vector3 Position
        {
            get => _position;
            set => SetPosition(value);
        }

        public Vector3 Scale
        {
            get => _scale;
            set => SetScale(value);
        }

        private Vector3 _position = Vector3.Zero;
        private Vector3 _scale = Vector3.One;

        public FixedSimpleMesh([NotNull] IRenderableComponent parent, [NotNull] IMeshGeometry geometry)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            Geometry = geometry ?? throw new ArgumentNullException(nameof(geometry));
        }

        public static FixedSimpleMesh FromVertices<TVertexType>([NotNull] IRenderableComponent parent, PrimitiveType primitiveType, VertexDeclaration vertexDeclaration,
            [NotNull] TVertexType[] vertices, [NotNull] int[] indices, int primitivesCount) where TVertexType : struct, IVertexType
        {
            var geometry = new StaticMeshGeometry<TVertexType>(vertexDeclaration, primitiveType, vertices, indices, primitivesCount);
            return new FixedSimpleMesh(parent, geometry);
        }
        
        public static FixedSimpleMesh FromVertices<TVertexType>([NotNull] IRenderableComponent parent, PrimitiveType primitiveType, VertexDeclaration vertexDeclaration,
            [NotNull] TVertexType[] vertices, [NotNull] short[] indices, int primitivesCount) where TVertexType : struct, IVertexType
        {
            var geometry = new StaticMeshGeometry<TVertexType>(vertexDeclaration, primitiveType, vertices, indices, primitivesCount);
            return new FixedSimpleMesh(parent, geometry);
        }

        public void SetPosition(Vector3 position)
        {
            _position = position;
            UpdateWordMatrix();
        }

        public void SetScale(Vector3 scale)
        {
            _scale = scale;
            UpdateWordMatrix();
        }

        public void UpdateWordMatrix()
        {
            World = Matrix.Identity * Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);
        }
    }
}