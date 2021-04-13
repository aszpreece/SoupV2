using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.Controls;
using SoupV2.EntityComponentSystem;
using SoupV2.Simulation;
using SoupV2.util;

namespace SoupForm.Controls
{
    public class SoupGraphicControl : MonoGameControl
    {
        /// <summary>
        /// The current simulation being rendered and updated.
        /// </summary>
        public Simulation CurrentSimulation { get; set; }

        protected override void Initialize()
        {
            // Stop visual studio crashing.
            if (DesignMode)
                return;

            base.Initialize();
            SerializerSettings.InitializeSettings(Editor.Content);
            TextureAtlas.Load(Editor.Content);
            SpriteBatchExtension.InitSpriteBatchExtension(Editor.graphics);
            Editor.BackgroundColor = new Color(20, 19, 40);
            CurrentSimulation = new Simulation(new SimulationSettings());
            CurrentSimulation.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            // Stop visual studio crashing.
            if (DesignMode)
                return;

            var kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(Keys.Up))
            {
                Editor.Cam.Zoom += 4 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (kbState.IsKeyDown(Keys.Down))
            {
                Editor.Cam.Zoom -= 4 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kbState.IsKeyDown(Keys.Up))
            {
                Editor.Cam.Move(new Vector2(0, -1) * (float)gameTime.ElapsedGameTime.TotalSeconds * 100);
            }
            else if (kbState.IsKeyDown(Keys.Down))
            {
                Editor.Cam.Move(new Vector2(0, 1) * (float)gameTime.ElapsedGameTime.TotalSeconds * 100);
            }


            if (kbState.IsKeyDown(Keys.Left))
            {
                Editor.Cam.Move(new Vector2(-1, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds * 100);
            }
            else if (kbState.IsKeyDown(Keys.Right))
            {
                Editor.Cam.Move(new Vector2(1, 0) * (float)gameTime.ElapsedGameTime.TotalSeconds * 100);
            }

            base.Update(gameTime);
            // TODO change to be based on ticks.
            CurrentSimulation.Update(gameTime);
        }

        protected override void Draw()
        {
            // Stop visual studio crashing.
            if (DesignMode)
                return;

            base.Draw();

            Editor.ShowCamPosition = true;
            Editor.Cam.Zoom = 0.1f;

            CurrentSimulation.Draw(Editor.spriteBatch, Editor.Cam.GetTransformation());

            Editor.DrawDisplay();
        }
    }
}
