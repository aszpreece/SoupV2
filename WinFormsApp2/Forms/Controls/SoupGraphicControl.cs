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

        public delegate void GraphicsInitializedEvent();
        public event GraphicsInitializedEvent GraphicsInitialized;

        protected override void Initialize()
        {
            // Stop visual studio crashing.
            if (DesignMode)
                return;

            base.Initialize();

            SpriteBatchExtension.InitSpriteBatchExtension(Editor.graphics);
            Editor.BackgroundColor = new Color(20, 19, 40);
            Editor.Cam.Zoom = 0.1f;
            GraphicsInitialized?.Invoke();
        }


        protected override void Update(GameTime gameTime)
        {
            // Stop visual studio crashing.
            if (DesignMode)
                return;

            base.Update(gameTime);
            // TODO change to be based on ticks.
            CurrentSimulation?.Update(gameTime);
        }

        protected override void Draw()
        {
            // Stop visual studio crashing.
            if (DesignMode)
                return;

            base.Draw();

            Editor.ShowCamPosition = true;


            CurrentSimulation?.Draw(Editor.spriteBatch, Editor.Cam.GetTransformation());

            Editor.DrawDisplay();
        }
    }
}
