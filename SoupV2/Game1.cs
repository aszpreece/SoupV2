using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.UI;
using SoupV2.UI;
using SoupV2.util;
using System;
using System.Diagnostics;

namespace SoupV2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Desktop _desktop;
		private Window _window;

		private HorizontalProgressBar _horizontalProgressBar;
        private Widget _verticalProgressBar;

        private Texture2D _testTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }


		protected override void LoadContent()
		{
			base.LoadContent();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _testTexture = Content.Load<Texture2D>("bin/DesktopGL/pirateship");


            MyraEnvironment.Game = this;

			_desktop = new Desktop();


            MainUI mainUI = new MainUI();
            _desktop.Root = mainUI;

            mainUI._quit.Selected += (s, a) => Exit();
            // mainUI._newScenario.Selected += (s, a) => _desktop.Root = new ScenarioUI();
        }

        protected override void Update(GameTime gameTime)
        {
            
        }

        private Rotation rot = new Rotation(100);
        private Vector2 scale = new Vector2(1, 1);

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            Vector2 pos = new Vector2(100 , 100);

            rot.Rotate(0.1 * gameTime.ElapsedGameTime.TotalSeconds);
            scale.X = (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds) + 1;
            scale.Y = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds) + 1;

            Debug.WriteLine(rot.Theta);
            // _spriteBatch.Draw(_testTexture, pos, null, Color.White, (float)rot.Theta, pos, SpriteEffects.None, 0);
            //_spriteBatch.Draw(_testTexture, pos, null, Color.White, (float)rot.Theta, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            _spriteBatch.Draw(_testTexture, pos, null, Color.White, (float)rot.Theta, _testTexture.Bounds.Center.ToVector2(), scale, SpriteEffects.None, 0);
            _spriteBatch.End();

            _desktop.Render();

            base.Draw(gameTime);
        }
    }
}
