using Newtonsoft.Json;

using System.IO;

namespace SoupV2.Simulation
{
    class Scenario
    {

        public static Scenario LoadScenario(string fileName)
        {
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(@"c:\movie.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                Scenario movie2 = (Scenario)serializer.Deserialize(file, typeof(Scenario));
            }
            return null;
        }


        public Scenario()
        {

        }
    }
}
