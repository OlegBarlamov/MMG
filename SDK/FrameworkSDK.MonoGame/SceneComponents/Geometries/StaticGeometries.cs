namespace FrameworkSDK.MonoGame.SceneComponents.Geometries
{
    public static class StaticGeometries
    {
        public static PlaneGeometry Plane { get; } = new PlaneGeometry();
        
        public static IcosahedronGeometry Icosahedron { get; } = new IcosahedronGeometry();
        
        /// <summary>
        /// 162 vertices
        /// 960 indices
        /// </summary>
        public static SphereGeometry Sphere { get; } = new SphereGeometry();
    }
}