using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace SoupV2.DB
{
    public class MongoDb
    {
        public static void Initialize()
        {
            MongoClient dbClient = new MongoClient("mongodb://localhost:27017");
            var dbList = dbClient.ListDatabases().ToList();

        }
    }
}
