using EntityComponentSystem;
using Microsoft.Xna.Framework;
using SoupV2.NEAT;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.Grid;
using SoupV2.util.Maths;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SoupV2.Simulation.Systems
{
    public class NoseSystem : EntitySystem
    {
        private AdjacencyGrid _grid;
        public NoseSystem(EntityManager pool, AdjacencyGrid grid) : base(pool, (e) => e.HasComponents(typeof(TransformComponent), typeof(NoseComponent)))
        {
            _grid = grid;
        }

        public void Update()
        {
            //for (int i = 0; i < Compatible.Count; i++)
            Parallel.For(0, Compatible.Count, (i) =>
            {
                var transform = Compatible[i].GetComponent<TransformComponent>();
                var nose = Compatible[i].GetComponent<NoseComponent>();
                var nearby = _grid.GetNearbyEntities(transform.WorldPosition, nose.NoseRange, (e) => e.HasComponent<EdibleComponent>());

                nose.CosActivation = 0;
                nose.SinActivation = 0;

                float closestFound = nose.NoseRangeSquared + 1;

                foreach (var (distSqr, entity) in nearby)
                {
                    if (distSqr <= 0)
                    {
                        continue;
                    }

                    // if the closest only setting is on, only report smell of closest food.
                    if (nose.ClosestOnly)
                    {
                        if (distSqr > closestFound)
                        {
                            continue;
                        }
                        closestFound = distSqr;
                        // reset the other activations
                        nose.CosActivation = 0;
                        nose.SinActivation = 0;
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

                    // Take into account direction.
                    // If the angle difference is small then the Cosine of teh angle will be large.
                    // This means the scent will be stronger when looking at the edible.
                    var distMult = (nose.NoseRangeSquared / distSqr);

                    if (!nose.ConsiderRange)
                    {
                        distMult = 1;
                    }

                    nose.CosActivation += (float)(Math.Cos(diffAngle) * distMult);
                    nose.SinActivation += (float)(Math.Sin(diffAngle) * distMult);

#if DEBUG
                    // Debug.WriteLine($"Activated");
#endif
                }

                if (nose.ConsiderRange)
                {
                    nose.CosActivation = (float)ActivationFunctions.Softsign(nose.CosActivation);
                    nose.SinActivation = (float)ActivationFunctions.Softsign(nose.SinActivation);
                }

                if (Compatible[i].TryGetComponent<GraphicsComponent>(out GraphicsComponent graphics))
                {
                    var newCol = new Color(
                        (float)Math.Max(nose.CosActivation, 0.3),
                        (float)Math.Max(nose.CosActivation, 0.3),
                        (float)Math.Max(nose.CosActivation, 0.3));

                    graphics.Color = Color.Lerp(graphics.Color, newCol, 0.1f);
                }
            });
        }

    }
}
