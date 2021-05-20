using CommandLine;
using Microsoft.Xna.Framework;
using SoupForm;
using SoupForm.Forms;
using SoupV2.Simulation;
using SoupV2.Simulation.Settings;
using System;
using System.Windows.Forms;

namespace SoupForms
{
    public static class Program
    {
        public class Options
        {
            [Option('h', "headless", Required = false, HelpText = "Runs the simulation without the UI.")]
            public bool Headless { get; set; }

            [Option('s', "statoutfile", Required = false, HelpText = "The destination for the generated statistics file.")]
            public string StatFilePath { get; set; }

            [Option('c', "config", Required = false, HelpText = "The path to the settings file.")]
            public string SettingsPath { get; set; }

            [Option('e', "entityfolder", Required = false, Default = @".", HelpText = "The path to the entity folder to use.")]
            public string EntityFolderPath { get; set; }
        }

        [STAThread]
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(StartProgram);
        }

        static void StartProgram(Options options)
        {
            if (options.Headless)
            {
                HeadlessSimulation(options);
            } else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }

        }

        static void HeadlessSimulation(Options options)
        {
            if (options.StatFilePath is null || options.SettingsPath is null)
            {
                Console.WriteLine("Must provide 'statoutfile' and 'config'");
                return;
            }

            bool error = false;
            SimulationSettings settings = DefaultSimulationSettings.GetSettings();
            EntityDefinitionDatabase database = new EntityDefinitionDatabase();

            if (!(options.SettingsPath is null))
            {
                try
                {
                    settings = LocalStorage.LoadSettingsFile(options.SettingsPath);
                }
                catch (Exception)
                {
                    error = true;
                    Console.WriteLine("Failed to load settings file.");
                }
            }
            if (!(options.SettingsPath is null)) 
            {
            
                try
                {
                    database = LocalStorage.GetDefinitionDatabase(options.EntityFolderPath);
                }
                catch (Exception)
                {
                    error = true;
                    Console.WriteLine("Failed to load entity definition database folder.");
                }
            }
            if (error)
            {
                return;
            }

            Simulation simulation = new Simulation(settings, database);

            simulation.SetUp();
            
            

        }
    }
}
