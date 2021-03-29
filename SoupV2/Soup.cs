using EntityComponentSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
using SoupV2.Simulation.Components;
using SoupV2.Simulation.Physics;
using SoupV2.Simulation.Systems;
using SoupV2.Systems;
using SoupV2.UI;
using SoupV2.util;
using System;
using System.Diagnostics;

namespace SoupV2
{
    public class Soup : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Desktop _desktop;

        private Texture2D _testTexture;

        public Soup()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }


        protected override void Initialize()
        {
            base.Initialize();
        }

        EntityPool _main;
        private RenderSystem _renderSystem;
        private TransformHierarchySystem _transformHierarchySystem;
        private VelocitySystem _velocitySystem;
        private SortAndSweep _physicsSystem;
        private RigidBodySystem _rigidBodySystem;
        private DragSystem _dragSystem;
        private MovementControlSystem _movementControlSystem;

        private float _gameSpeed = 1.0f;
        private float _maxGameSpeed = 5.0f;

        private Camera _camera;

        protected override void LoadContent()
        {
            base.LoadContent();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _testTexture = Content.Load<Texture2D>("bin/DesktopGL/circle");


            MyraEnvironment.Game = this;

            _desktop = new Desktop();


            MainUI mainUI = new MainUI();
            _desktop.Root = mainUI;

            mainUI._quit.Selected += (s, a) => Exit();
            mainUI._gameSpeedSlider.ValueChanged += (s, a) => {
                _gameSpeed = _maxGameSpeed * a.NewValue / 100;
                mainUI._gameSpeedLabel.Text = $"Simulation Speed: {Math.Round(_gameSpeed, 2)}";
            };
             //mainUI._newScenario.Selected += (s, a) => _desktop.Root = new ScenarioUI();

            _main = new EntityPool("Main Pool");
            _renderSystem = new RenderSystem(_main);
            _transformHierarchySystem = new TransformHierarchySystem(_main);
            _velocitySystem = new VelocitySystem(_main);
            _physicsSystem = new SortAndSweep(_main);
            _rigidBodySystem = new RigidBodySystem(_main);
            _dragSystem = new DragSystem(_main, 1.2f);
            _movementControlSystem = new MovementControlSystem(_main);

            var camera = _main.CreateEntity($"cam");

            var transformCam = new TransformComponent(camera)
            {
                LocalPosition = Vector2.Zero,
                LocalRotation = new Rotation(0),
                LocalDepth = 0.1f,
                Scale = new Vector2(1, 1)
            };

            var graphicsCam = new GraphicsComponent()
            {
                Texture = _testTexture,
                Dimensions = new Point(20, 20),
                Color = Color.Yellow
               
            };


            camera.AddComponents(transformCam, graphicsCam);

            Random rand = new Random();
            for (int i = 0; i < 100; i++)
            {
                float radius = (float)rand.NextDouble() * 10f;

                var testEntity = _main.CreateEntity($"test{i}");

                var pos = new Vector2(0 + i + rand.Next(-10, 10), 200 + (i) * 12);
                var vel = new Vector2(50, 0);
                if (i % 2 == 0)
                {
                    vel = new Vector2(-100, 0);
                    pos = new Vector2(800 + i, 200 + i * 12);
                }
 
                var transform = new TransformComponent(testEntity)
                {
                    LocalPosition = pos,
                    LocalRotation = new Rotation(i % 2 == 1 ? 0 : (float)Math.PI),
                    LocalDepth = 0.1f,
                    Scale = new Vector2(1, 1)
                };

                var graphics = new GraphicsComponent()
                {
                    Texture = _testTexture,
                    Dimensions = new Point((int)(radius * 2), (int)(radius * 2)),
                    Color = i % 2 == 0 ? Color.Blue : Color.Yellow
                };

               
                var velocity = new VelocityComponent()
                {
                    RoationalVelocity = 0,
                    Velocity = vel
                };

                var circleCollider = new CircleColliderComponent(transform)
                {
                    Radius = radius
                };

                var rigidbody = new RigidBodyComponent()
                {
                    Mass = 1,
                    Restitution = 0.1f
                };

                var drag = new DragComponent()
                {
                    DragCoefficient = 0.4f
                };

                var movementControl = new MovementControlComponent()
                {
                    MaxMovementForceNewtons = 100.0f,
                    WishForceForward = 1.0f
                };

                testEntity.AddComponents(transform, graphics, velocity, circleCollider, rigidbody, drag, movementControl);

            }

            _camera = new Camera(Window);
        
        }
         

        private float _cameraMoveSpeed = 500;
        private float _cameraZoomSpeed = 5;

        protected override void Update(GameTime gameTime)
        {
            var kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(Keys.Z))
            {
                _camera.Zoom += _cameraZoomSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (kbState.IsKeyDown(Keys.X))
            {
                _camera.Zoom -= _cameraZoomSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kbState.IsKeyDown(Keys.Up))
            {
                _camera.Position += new Vector2(0, -1) * (float)gameTime.ElapsedGameTime.TotalSeconds * _cameraMoveSpeed;
            }
            else if (kbState.IsKeyDown(Keys.Down))
            {
                _camera.Position += new Vector2(0, 1) * (float)gameTime.ElapsedGameTime.TotalSeconds * _cameraMoveSpeed;
            }


            if (kbState.IsKeyDown(Keys.Left))
            {
                _camera.Position += new Vector2(-1, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds * _cameraMoveSpeed;
            }
            else if (kbState.IsKeyDown(Keys.Right))
            {
                _camera.Position += new Vector2(1, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds * _cameraMoveSpeed;
            }


            _transformHierarchySystem.Update();
            _physicsSystem.GetCollisions();
            //_dragSystem.Update();
            _rigidBodySystem.Update(gameTime, _gameSpeed);
            //_movementControlSystem.Update();
            _velocitySystem.Update(gameTime, _gameSpeed);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _renderSystem.Draw(_spriteBatch, _camera);

            _desktop.Render();

            base.Draw(gameTime);
        }
    }
}
