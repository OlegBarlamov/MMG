using System.Collections.Generic;
using FrameworkSDK.MonoGame.Graphics.Basic;
using FrameworkSDK.MonoGame.Mvc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FrameworkSDK.MonoGame.SceneComponents
{
    public class Grid3DComponentData
    {
        public string GraphicsPassName = View.DefaultViewPassName;

        public float AxesLength = 10f;
        public float CellsSize = 1f;
        
        public Color XAxeColor = Color.Red;
        public Color YAxeColor = Color.Blue;
        public Color ZAxeColor = Color.Green;
        public Color CellsColor = Color.WhiteSmoke;
    }

    public class Grid3DComponentView : SingleMeshComponent<Grid3DComponentData>
    {
        protected override IRenderableMesh Mesh { get; } 
        protected override string SingleGraphicsPassName { get; }

        public Grid3DComponentView(Grid3DComponentData model)
        {
            SingleGraphicsPassName = model.GraphicsPassName;

            Mesh = GenerateMesh(model.AxesLength, model.CellsSize, model.XAxeColor, model.YAxeColor, model.ZAxeColor, model.CellsColor);
        }

        private IRenderableMesh GenerateMesh(float axesLength, float cellSize, Color xAxeColor, Color yAxeColor, Color zAxeColor, Color cellsColor)
        {
            var vertices = new List<VertexPositionColor>();
            var indices = new List<int>();
            
            // *** Axes ***
            vertices.AddRange(new[]
            {
                new VertexPositionColor(new Vector3(axesLength, 0, 0), xAxeColor),
                new VertexPositionColor(new Vector3(0, 0, 0), xAxeColor),

                new VertexPositionColor(new Vector3(0, 0, axesLength), zAxeColor),
                new VertexPositionColor(new Vector3(0, 0, 0), zAxeColor),

                new VertexPositionColor(new Vector3(0, axesLength, 0), yAxeColor),
                new VertexPositionColor(new Vector3(0, 0, 0), yAxeColor)
            });
            indices.AddRange(new[] {0, 1, 2, 3, 4, 5});

            var index = 5;
            // *** Cells ***
            var cellsCount = (int)(axesLength / cellSize);
            for (int i = 0; i < cellsCount; i++)
            {
                var value = (i + 1) * cellSize;
                vertices.Add(new VertexPositionColor(new Vector3(value, 0, 0), cellsColor));
                indices.Add(++index);
                vertices.Add(new VertexPositionColor(new Vector3(value, 0, axesLength), cellsColor));
                indices.Add(++index);
                //vertices.Add(new VertexPositionColor(new Vector3(value, 0, 0), cellsColor));
                indices.Add(index-1);
                vertices.Add(new VertexPositionColor(new Vector3(value, axesLength, 0), cellsColor));
                indices.Add(++index);
                
                vertices.Add(new VertexPositionColor(new Vector3(0, value, 0), cellsColor));
                indices.Add(++index);
                vertices.Add(new VertexPositionColor(new Vector3(axesLength, value, 0), cellsColor));
                indices.Add(++index);
                //vertices.Add(new VertexPositionColor(new Vector3(0, value, 0), cellsColor));
                indices.Add(index-1);
                vertices.Add(new VertexPositionColor(new Vector3(0, value, axesLength), cellsColor));
                indices.Add(++index);
                
                vertices.Add(new VertexPositionColor(new Vector3(0, 0, value), cellsColor));
                indices.Add(++index);
                vertices.Add(new VertexPositionColor(new Vector3(axesLength, 0, value), cellsColor));
                indices.Add(++index);
                //vertices.Add(new VertexPositionColor(new Vector3(0, 0, value), cellsColor));
                indices.Add(index-1);
                vertices.Add(new VertexPositionColor(new Vector3(0, axesLength, value), cellsColor));
                indices.Add(++index);
            }

            return new FixedSimpleMesh<VertexPositionColor>(this, PrimitiveType.LineList,
                VertexPositionColor.VertexDeclaration, vertices.ToArray(), indices.ToArray(), indices.Count / 2);
        }
    }
}