using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using SoupV2.Simulation.Events;

namespace SoupV2.DB
{
    public class SoupMongo
    {
        public static string Password { get; set; } = "";

        public static string Username { get; set; } = "";

        public static string IP { get; set; } = "localhost";
        public static string Port { get; set; } = "27017";

        static IMongoCollection<BsonDocument> destination;

        static MongoClient mongoClient;

        public static (bool, string) Connect()
        {
            if (!int.TryParse(Port, out int port))
            {
                return (false, "Port invalid");
            }

            try
            {
                var credential = MongoCredential.CreateCredential("soup_data", Username, Password);
                //Server settings
                var settings = new MongoClientSettings
                {
                    Credential = credential ,
                    Server = new MongoServerAddress("20.241.131.194", port)
                };

                //Get a Reference to the Client Object
                mongoClient = new MongoClient(settings);
                var server = mongoClient.GetDatabase("soup_data");

            } catch (Exception)
            {
                return (false, "Failed to Connect");
            }

            return (true, "Connected");
        }

        public static void Initialize(string experimentName)
        {

                //MongoClient dbClient = new MongoClient($"mongodb://{IP}:Port");
                //var database = dbClient.GetDatabase("soup_data");
                //database.CreateCollection(experimentName, new CreateCollectionOptions { Capped = false });
                //var collection = database.GetCollection<BsonDocument>(experimentName);
                //destination = collection;
        }

        public static void PostEvent(AbstractEventInfo info)
        {

        } 
    }
}
