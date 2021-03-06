/* Generated by MyraPad at 08/04/2021 18:43:51 */
using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.File;
using Myra.Graphics2D.UI.Properties;
using Newtonsoft.Json;
using SoupV2;
using SoupV2.Simulation;
using SoupV2.Simulation.Statistics.StatLoggers;
using System;
using System.IO;

namespace UI
{
	public partial class ExperimentSetUpUI
	{

		SimulationSettings simSettings;

		public ExperimentSetUpUI(Soup soup)
		{
			BuildUI();

			simSettings = new SimulationSettings();

			_backButton.Click += (s, a) =>
			{
				Desktop.Root = new MainMenuUI(soup);
			};

			_editSettingsButton.Click += (s, a) =>
			{
				PropertyGrid propertyGrid = new PropertyGrid
				{
					Object = simSettings,
					Width = 350
				};

				Window window = new Window
				{
					Title = "Experiment Settings",
					Content = propertyGrid
				};

				window.ShowModal(Desktop);

			};

			_settingsDestinationComboBox.Items.Add(new ListItem("None", Color.White, StatLogDestination.NONE));
			_settingsDestinationComboBox.Items.Add(new ListItem("File", Color.White, StatLogDestination.FILE));
			_settingsDestinationComboBox.SelectedIndex = 0;
			_browseFileButton.Enabled = false;

			_settingsDestinationComboBox.SelectedIndexChanged += (s, a) =>
			{
				// DIsable the file browser if we are not saving files.
				if ((StatLogDestination)_settingsDestinationComboBox.SelectedItem.Tag == StatLogDestination.FILE)
				{
					_browseFileButton.Enabled = true;
				} else
				{
					_browseFileButton.Enabled = false;
					_currentFilePathLabel.Text = "";
				}

			};

			_loadSettingsButton.Click += (s, a) => OpenSettingsFile();
			_saveSettingsButton.Click += (s, a) => SaveSettingsFile();

			_browseFileButton.Click += (s, a) => SaveStatsFile();

			_beginButton.Click += (s, a) =>
			{
				string completePath = Path.Combine(_currentFilePathLabel.Text);
				if (File.Exists(completePath)) {
					Dialog.CreateMessageBox("Error", "Already a statistics file with the same name at the destination.").ShowModal(Desktop);
				}
				
				soup.BeginExperiment(simSettings, (StatLogDestination)_settingsDestinationComboBox.SelectedItem.Tag, completePath);

			};

		}

		public void OpenSettingsFile()
		{
			var fileDialog = new FileDialog(FileDialogMode.OpenFile);
			fileDialog.ShowModal(Desktop);

			fileDialog.Closed += (s, a) =>
			{
				if (!fileDialog.Result)
				{
					return;
				}

				try
                {
					using (StreamReader sr = File.OpenText(fileDialog.FilePath))
					{
						JsonSerializer serializer = new JsonSerializer();
						serializer.TypeNameHandling = TypeNameHandling.All;
						simSettings = (SimulationSettings)serializer.Deserialize(sr, typeof(SimulationSettings));
					}
                } catch (Exception e)
                {
					Dialog.CreateMessageBox("Error", $"Something went wrong loading settings file {e.Message}").ShowModal(Desktop);
                }
			};
		}

		public void ChooseStatsFolder()
		{
			var fileDialog = new FileDialog(FileDialogMode.ChooseFolder);
			fileDialog.ShowModal(Desktop);

			fileDialog.Closed += (s, a) =>
			{
				if (!fileDialog.Result)
				{
					return;
				}

				_currentFilePathLabel.Text = fileDialog.FilePath;
			};
		}

		public void SaveSettingsFile()
		{
			var fileDialog = new FileDialog(FileDialogMode.SaveFile);
			fileDialog.ShowModal(Desktop);

			fileDialog.Closed += (s, a) =>
			{
				if (!fileDialog.Result)
				{
					return;
				}

				using (StreamWriter sr = File.CreateText(fileDialog.FilePath))
				{
					JsonSerializer serializer = new JsonSerializer();
					serializer.TypeNameHandling = TypeNameHandling.All;
					serializer.Serialize(sr, simSettings);
				}
			};
		}

		public void SaveStatsFile()
		{
			var fileDialog = new FileDialog(FileDialogMode.SaveFile);
			fileDialog.ShowModal(Desktop);

			fileDialog.Closed += (s, a) =>
			{
				if (!fileDialog.Result)
				{
					return;
				}
				_currentFilePathLabel.Text = fileDialog.FilePath;

			};
		}
	}
}