/* Generated by MyraPad at 26/03/2021 17:35:44 */
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.Brushes;
using Myra.Graphics2D.UI.Properties;

#if MONOGAME || FNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#elif STRIDE
using Stride.Core.Mathematics;
#else
using System.Drawing;
using System.Numerics;
#endif

namespace SoupV2.UI
{
	partial class MainUI: Panel
	{
		private void BuildUI()
		{
			var label1 = new Label();
			label1.Text = "Soup";
			label1.TextColor = ColorStorage.CreateColor(254, 57, 48, 255);
			label1.DisabledTextColor = ColorStorage.CreateColor(64, 64, 64, 255);
			label1.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center;

			_menuLoadScenario = new MenuItem();
			_menuLoadScenario.Text = "Load Scenario";
			_menuLoadScenario.Id = "_menuLoadScenario";

			_newScenario = new MenuItem();
			_newScenario.Text = "New Scenario";
			_newScenario.Id = "_newScenario";

			_quit = new MenuItem();
			_quit.Text = "Quit";
			_quit.Id = "_quit";

			var verticalMenu1 = new VerticalMenu();
			verticalMenu1.Background = new SolidBrush("#404040FF");
			verticalMenu1.Items.Add(_menuLoadScenario);
			verticalMenu1.Items.Add(_newScenario);
			verticalMenu1.Items.Add(_quit);

			_gameSpeedSlider = new HorizontalSlider();
			_gameSpeedSlider.Value = 50;
			_gameSpeedSlider.MinWidth = 200;
			_gameSpeedSlider.Id = "_gameSpeedSlider";

			_gameSpeedLabel = new Label();
			_gameSpeedLabel.Text = "Game Speed";
			_gameSpeedLabel.TextColor = ColorStorage.CreateColor(254, 57, 48, 255);
			_gameSpeedLabel.DisabledTextColor = ColorStorage.CreateColor(64, 64, 64, 255);
			_gameSpeedLabel.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center;
			_gameSpeedLabel.Id = "_gameSpeedLabel";

			_gameSpeedPanel = new Panel();
			_gameSpeedPanel.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Right;
			_gameSpeedPanel.Border = new SolidBrush("#5BC6FAFF");
			_gameSpeedPanel.Id = "_gameSpeedPanel";
			_gameSpeedPanel.Widgets.Add(_gameSpeedSlider);
			_gameSpeedPanel.Widgets.Add(_gameSpeedLabel);

			
			Widgets.Add(label1);
			Widgets.Add(verticalMenu1);
			Widgets.Add(_gameSpeedPanel);
		}

		
		public MenuItem _menuLoadScenario;
		public MenuItem _newScenario;
		public MenuItem _quit;
		public HorizontalSlider _gameSpeedSlider;
		public Label _gameSpeedLabel;
		public Panel _gameSpeedPanel;
	}
}
