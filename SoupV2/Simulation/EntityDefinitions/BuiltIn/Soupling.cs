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
    public class Soupling
    {
        public static Entity GetCritter(Color color)
        {
            float radius = 10f;

            var critter = new Entity();

            var pos = new Vector2(0, 0);

            var transform = new TransformComponent(critter)
            {
                LocalPosition = pos,
                LocalRotation = new Rotation(MathHelper.PiOver4),
                LocalDepth = 0.1f,
                Scale = new Vector2(1, 1)
            };

            var graphics = new GraphicsComponent(critter)
            {
                TexturePath = TextureAtlas.CirclePath,
                Dimensions = new Point((int)(radius * 2), (int)(radius * 2)),
                Color = color
            };


            var velocity = new VelocityComponent(critter)
            {
                RotationalVelocity = 0,
                Velocity = Vector2.Zero
            };

            var circleCollider = new CircleColliderComponent(critter)
            {
                Radius = radius
            };


            var rigidbody = new RigidBodyComponent(critter)
            {
                Mass = 0.05f,
                Restitution = 0.1f
            };

            var drag = new DragComponent(critter)
            {
                MovementDragCoefficient = 0.1f,
                RotationDragCoefficient = 10f

            };

            var movementControl = new MovementControlComponent(critter)
            {
                MaxMovementForceNewtons = 10.0f,
                MaxRotationForceNewtons = 10.0f,
                MovementMode = MovementMode.TwoWheels,
            };

            var colour = new VisibleColourComponent(critter)
            {

            };

            var energy = new EnergyComponent(critter)
            {
                Energy = 4f
            };

            var reproduction = new ReproductionComponent(critter)
            {
                Efficency = 0.7f,
                ReproductionEnergyCost = 8f,
                Reproduce = 0,
                ReproductionThreshold = 0.5f,
                RequiredRemainingEnergy = 1f,
                ChildDefinitionId = "Soupling"
            };

            var health = new HealthComponent(critter)
            {
                Health = 100,
                MaxHealth = 100,
                
            };

            var age = new OldAgeComponent(critter)
            {
                MaxAge = 5 * 60
            };

            var brain = new BrainComponent(critter)
            {
                InputMap = new Dictionary<string, string>
                    {
                        {"eye1R", "eye1.EyeComponent.ActivationR" },
                        {"eye1G", "eye1.EyeComponent.ActivationG" },
                        {"eye1B", "eye1.EyeComponent.ActivationB" },

                        {"eye2R", "eye2.EyeComponent.ActivationR" },
                        {"eye2G", "eye2.EyeComponent.ActivationG" },
                        {"eye2B", "eye2.EyeComponent.ActivationB" },

                        {"eye3R", "eye3.EyeComponent.ActivationR" },
                        {"eye3G", "eye3.EyeComponent.ActivationG" },
                        {"eye3B", "eye3.EyeComponent.ActivationB" },

                        {"eye4R", "eye4.EyeComponent.ActivationR" },
                        {"eye4G", "eye4.EyeComponent.ActivationG" },
                        {"eye4B", "eye4.EyeComponent.ActivationB" },

                        {"mouth", "mouth.MouthComponent.Eating" },
                        {"nosecos", "nose.NoseComponent.CosActivation" },
                        {"nosesin", "nose.NoseComponent.SinActivation" },
                        {"health", "HealthComponent.HealthPercent" },
                        {"myRed", "VisibleColourComponent.RealR" },
                        {"myGreen", "VisibleColourComponent.RealG" },
                        {"myBlue", "VisibleColourComponent.RealB" },
                        {"Random", "Random" },
                        {"Bias", "Bias" },
                    },
                //OutputMap = new Dictionary<string, string>
                //    {
                //        {"forwardback", "MovementControlComponent.WishForceForward" },
                //        {"rotation", "MovementControlComponent.WishRotForce" },
                //        {"reproduce", "ReproductionComponent.Reproduce" },
                //        {"red", "VisibleColourComponent.R" },
                //        {"green", "VisibleColourComponent.G" },
                //        {"blue", "VisibleColourComponent.B" },
                //        {"attack", "weapon.WeaponComponent.Activation" }
                //    }
                OutputMap = new Dictionary<string, string>
                    {
                        {"wheelLeft", "MovementControlComponent.WishWheelLeftForce" },
                        {"wheelRight", "MovementControlComponent.WishWheelRightForce" },
                        {"reproduce", "ReproductionComponent.Reproduce" },
                        {"red", "VisibleColourComponent.R" },
                        {"green", "VisibleColourComponent.G" },
                        {"blue", "VisibleColourComponent.B" },
                        {"attack", "weapon.WeaponComponent.Activation" }
                    }
            };

            critter.AddComponents(transform, graphics, velocity, circleCollider, rigidbody, drag, movementControl, reproduction, colour, energy, health, age, brain);

            var eye1 = Eye.GetEye(Color.White, MathHelper.ToRadians(30));
            eye1.Tag = "eye1";
            critter.AddChild(eye1);

            var eye2 = Eye.GetEye(Color.White, MathHelper.ToRadians(-30));
            eye2.Tag = "eye2";
            critter.AddChild(eye2);

            var eye3 = Eye.GetEye(Color.White, MathHelper.ToRadians(90));
            eye3.Tag = "eye3";
            critter.AddChild(eye3);

            var eye4 = Eye.GetEye(Color.White, MathHelper.ToRadians(-90));
            eye4.Tag = "eye4";
            critter.AddChild(eye4);


            var mouth = Mouth.GetMouth(Color.White);
            critter.AddChild(mouth);
            mouth.GetComponent<TransformComponent>().LocalPosition = new Vector2(15, 0);

            var nose = Nose.GetNose(Color.White, 0, 10);
            nose.Tag = "nose";
            critter.AddChild(nose);

            var weapon = Weapon.GetWeapon(Color.White);
            weapon.Tag = "weapon";
            critter.AddChild(weapon);
            weapon.GetComponent<TransformComponent>().LocalPosition = new Vector2(30, 0);

            return critter;
        }
    }
}
