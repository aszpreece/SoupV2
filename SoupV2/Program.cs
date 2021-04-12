using CommandLine;
using Microsoft.Xna.Framework;
using System;

namespace SoupV2
{
    public static class Program
    {
        public class Options
        {
            [Option('h', "headless", Required = false, HelpText = "Runs the simulation without the UI.")]
            public bool Headless { get; set; }

            [Option('s', "statoutfile", Required = false, HelpText = "The destination for the generated statistics file.")]
            public string StatFilePath { get; set; }

            [Option('t', "simticks", Required = false, HelpText = "The amount of ticks to run the simulation for. 60 ticks = 1 second.")]
            public uint SimTicks { get; set; }

            [Option('d', "simticks", Required = false, Default = 1.0f, HelpText = "The amount of time that passes per tick. Default is 1.")]
            public float DeltaTimeStep { get; set; }


        }

        [STAThread]
        static void Main(string[] args)
        {

            Options opts;
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o => opts = o);
            
            using (var game = new Soup())
                game.Run();
        }
    }
}
