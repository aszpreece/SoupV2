using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.Grid;
using SoupV2.util.Maths;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SoupV2.Simulation.Systems
{
    public class VisionSystem : EntitySystem
    {
        private AdjacencyGrid _grid;
        public VisionSystem(EntityPool pool, AdjacencyGrid grid) : base(pool, (e) => e.HasComponents(typeof(TransformComponent), typeof(EyeComponent)))
        {
            _grid = grid;
        }

        public void Update()
        {
            for (int i = 0; i < Compatible.Count; i++)
            {
                var transform = Compatible[i].GetComponent<TransformComponent>();
                var eye = Compatible[i].GetComponent<EyeComponent>();
                var nearby =_grid.GetNearbyEntities(transform.WorldPosition, eye.EyeRange, (e) => e.HasComponent<ColourComponent>());
                eye.Activation = 0;
                foreach (var(distSqr, entity) in nearby)
                {
                    if (distSqr <= 0)
                    {
                        continue;
                    }

                    // Inverse square law
                    // Activation is inverseley proportional to the square of the distance of the object from the eye
                    // In this case we take it as the normalised distance (n)
                    // n = d/r
                    // We want to calculate 1/n^2.
                    // -> n^2 = d^2/r^2
                    // -> 1/n^2 = r^2/d^2, so we don't have to do any sqrts and only one division!
                    var targetTransform = entity.GetComponent<TransformComponent>();

                    float angleBetween = Angles.AngleBetweenPoints(transform.WorldPosition, targetTransform.WorldPosition);

                    float diffAngle = Angles.CalculateAngleDiff(angleBetween, transform.WorldRotation.Theta);

                    float diff = Angles.CalculateAngleDiff(-MathHelper.TwoPi, 0);
                    //eyeRot: {MathHelper.ToDegrees(transform.WorldRotation.Theta)}, diffAngle: {MathHelper.ToDegrees(diffAngle)}, 
#if DEBUG
                   // Debug.WriteLine($"Diff {diffAngle}");
                   //Debug.WriteLine($"angleBetween {MathHelper.ToDegrees(angleBetween)}");
#endif
                    // Check in fov
                    // if not continue;
                    if (Math.Abs(diffAngle) > eye.Fov / 2)
                    {

#if DEBUG
                        //Debug.WriteLine($"Out of fov");
#endif
                        continue;
                    }

                    eye.Activation += (eye.EyeRangeSquared / distSqr);

#if DEBUG
                   // Debug.WriteLine($"Activated");
#endif


                }
                eye.Activation = (float)Math.Tanh(eye.Activation);
                if (Compatible[i].HasComponent<GraphicsComponent>())
                {
                    var graphics = Compatible[i].GetComponent<GraphicsComponent>();
                    graphics.Multiplier = 1 + eye.Activation;
                }

            }
        }

    }
}
