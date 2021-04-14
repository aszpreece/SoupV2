using Newtonsoft.Json;
using SoupV2.Simulation.Events;
using SoupV2.Simulation.EventsAndInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace SoupV2.Simulation.Statistics.StatLoggers
{
    public class FileStatLogger : IStatLogger
    {
        private StatisticsGatherer _gatherer;
        private Stream _outputStream;
        public FileStatLogger(StatisticsGatherer gatherer, Stream outputStream)
        {
            _gatherer = gatherer;
            _outputStream = outputStream;
        }
        public void LogStats()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            JsonSerializer serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.All;
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            using (StreamWriter outputFile = new StreamWriter(_outputStream))
            using (JsonWriter writer = new JsonTextWriter(outputFile))
            {
                // Write start of array

                try
                {
                    writer.WriteStartArray();
                    while (true)
                    {
                        // Continually try to write stats to destination
            
                        if (!_gatherer.InformationToLog.TryDequeue(out AbstractSimulationInfo infoToLog))
                        {
                            // Sleep so the interrupted exception will throw.
                            Thread.Sleep(1);
                            continue;
                        }
                        serializer.Serialize(writer, infoToLog);
                    }

                }
                catch (ThreadInterruptedException e)
                {
                    // Attempt to write remaining stats
                    while (!_gatherer.InformationToLog.IsEmpty)
                    {
                        if (!_gatherer.InformationToLog.TryDequeue(out AbstractSimulationInfo infoToLog))
                        {
                            continue;
                        }
                        serializer.Serialize(writer, infoToLog);
                    }
                }
                finally
                {
                    // Write end of array.
                    writer.WriteEndArray();
                }
            }
        }
    }
}
