using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoupV2.EntityComponentSystem;
using SoupV2.Simulation.Brain;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.Physics;
using SoupV2.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoupV2.Simulation.EntityDefinitions
{
    class Critter
    {
        public static EntityDefinition GetCritter(Texture2D texture, Color color)
        {
            float radius = 10f;

            var testEntity = new Entity();

            var vel = new Vector2(-100, 0);
            var pos = new Vector2(0, 0);

            var transform = new TransformComponent(testEntity)
            {
                LocalPosition = pos,
                LocalRotation = new Rotation(MathHelper.PiOver4),
                LocalDepth = 0.1f,
                Scale = new Vector2(1, 1)
            };

            var graphics = new GraphicsComponent(testEntity)
            {
                Texture = texture,
                Dimensions = new Point((int)(radius * 2), (int)(radius * 2)),
                Color = color
            };


            var velocity = new VelocityComponent(testEntity)
            {
                RotationalVelocity = 0,
                Velocity = Vector2.Zero
            };

            var circleCollider = new CircleColliderComponent(testEntity)
            {
                Radius = radius
            };


            var rigidbody = new RigidBodyComponent(testEntity)
            {
                Mass = 0.1f,
                Restitution = 0.1f
            };

            var drag = new DragComponent(testEntity)
            {
                DragCoefficient = 0.1f
            };

            var movementControl = new MovementControlComponent(testEntity)
            {
                MaxMovementForceNewtons = 1.0f,
                WishForceForward = 0.0f,
                MaxRotationForceNewtons = 0.5f,
            };

            var colour = new ColourComponent(testEntity)
            {

            };

            var energy = new EnergyComponent(testEntity)
            {
                Energy = 3
            };

            var reproduction = new ReproductionComponent(testEntity)
            {
                Reproduce = 0,
                ReproductionThreshold = 1,
                ChildDefinitionId = "Critterling"
            };

            var brain = new BrainComponent(testEntity)
            {
                InputMap = new Dictionary<string, string>
                    {
                        {"eye1", "eye1.EyeComponent.Activation" },
                        {"eye2", "eye2.EyeComponent.Activation" }
                    },
                OutputMap = new Dictionary<string, string>
                    {
                        {"forwardback", "MovementControlComponent.WishForceForward" },
                        {"rotation", "MovementControlComponent.WishRotForce" }
                    }
            };

            testEntity.AddComponents(transform, graphics, velocity, circleCollider, rigidbody, drag, movementControl, reproduction, colour, energy, brain);

            var eye = Entity.FromDefinition(Eye.GetEye(Color.White, MathHelper.ToRadians(30)));
            eye.Tag = "eye1";
            testEntity.AddChild(eye);

            eye = Entity.FromDefinition(Eye.GetEye(Color.White, MathHelper.ToRadians(-30)));
            eye.Tag = "eye2";
            testEntity.AddChild(eye);


            var mouth = Entity.FromDefinition(Mouth.GetMouth(Color.White));
            testEntity.AddChild(mouth);
            mouth.GetComponent<TransformComponent>().LocalPosition = new Vector2(15, 0);


            return testEntity.ToDefinition();
        }

        public static EntityDefinition GetGrabber(Texture2D texture, Color color)
        {
            float radius = 5f;

            var grabber = new Entity("grabber");

            var transform = new TransformComponent(grabber)
            {
                LocalPosition = Vector2.Zero,
                LocalRotation = new Rotation(0),
                LocalDepth = 0.1f,
                Scale = new Vector2(1, 1)
            };

            var graphics = new GraphicsComponent(grabber)
            {
                Texture = texture,
                Dimensions = new Point((int)(radius * 2), (int)(radius * 2)),
                Color = color
            };
            grabber.AddComponents(transform, graphics);
            return grabber.ToDefinition();

        }
    }
}
