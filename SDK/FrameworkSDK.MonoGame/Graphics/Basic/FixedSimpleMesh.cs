using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FrameworkSDK.MonoGame.Graphics.Basic
{
    public sealed class FixedSimpleMesh<T> : IRenderableMesh where T: struct, IVertexType
    {
        public IRenderableComponent Parent { get; }
        public PrimitiveType PrimitiveType { get; }
        public VertexDeclaration VertexDeclaration { get; }

        private readonly T[] _vertices;
        private readonly int[] _indices;
        private readonly int _primitivesCount;

        public Matrix World { get; set; } = Matrix.Identity;
        
        public FixedSimpleMesh(IRenderableComponent parent, PrimitiveType primitiveType, VertexDeclaration vertexDeclaration, T[] vertices, int[] indices, int primitivesCount)
        {
            _vertices = vertices;
            _indices = indices;
            _primitivesCount = primitivesCount;
            Parent = parent;
            PrimitiveType = primitiveType;
            VertexDeclaration = vertexDeclaration;
        }

        public void SetPosition(Vector3 position)
        {
            World = Matrix.CreateTranslation(position);
        }

        public object GetVertices()
        {
            return _vertices;
        }

        public int[] GetIndices()
        {
            return _indices;
        }

        public int GetPrimitivesCount()
        {
            return _primitivesCount;
        }
    }
}