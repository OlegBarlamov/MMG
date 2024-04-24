using FrameworkSDK.MonoGame.Physics2D;
using Microsoft.Xna.Framework;
using NetExtensions.Helpers;
using SimplePhysics2D;
using SimplePhysics2D.Fixtures;

namespace SimplePhysics2D.Detectors
{
    public class Circle2CircleCollisionDetector : ICollisionDetector2D<CircleFixture, CircleFixture>
    {
        public Collision2D GetCollision(CircleFixture bodyA, CircleFixture bodyB)
        {
            var c1 = bodyA.Center;
            var c2 = bodyB.Center;
            Vector2.DistanceSquared(ref c1, ref c2, out var squaredDistance);
            if (squaredDistance > MathExtended.Sqr(bodyA.Radius + bodyB.Radius))
                return Collision2D.Empty;
            
            Vector2 tangent = c1 + (c2 - c1) * bodyA.Radius / (bodyA.Radius + bodyB.Radius);
            return new Collision2D(bodyA, bodyB, new [] { tangent } );
        }

        public Collision2D GetCollision(IFixture2D bodyA, IFixture2D bodyB)
        {
            return GetCollision((CircleFixture) bodyA, (CircleFixture) bodyB);
        }
    }
}