using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
//using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace REST_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        protected static IMongoClient _client = new MongoClient("mongodb://uber-ijscoapp:EejDABqPFOpMdYu7wSvsVMQF0vaWp8G12R8AROUbwwr0GGOA0Xa01bSuYLJQswJEtzuA6hCxFGMpONit6dwbzA==@uber-ijscoapp.documents.azure.com:10250/?ssl=true");
        protected static IMongoDatabase _database = _client.GetDatabase("uber-ijscoapp");
        public List<Location> locations(string id)
        {
            /*Simulate database*/
            List<Location> aList = new List<Location>();
            aList.Add(new Location() { Longitude = 5.111111, Latitude = 43.111111});
            aList.Add(new Location() { Longitude = 5.222222, Latitude = 43.222222 });

            List<Location> sendList = new List<Location>();           
            sendList.Add(aList[int.Parse(id)]); 
            return sendList;
            
        }        public Location Save(Location bericht)
        {
            if (WebOperationContext.Current.IncomingRequest.Method == "POST")
            {
                
                //_database.DropCollection("UberCollection");
                //WriteToDb();
                ReadDb();
            }
            return null;
        }        public async void WriteToDb()
        {
            var document = new BsonDocument
                {
                    { "_id", "1996" },
                    { "name", "Jef" }
                };
            var collection = _database.GetCollection<BsonDocument>("UberCollection");
            await collection.InsertOneAsync(document);
        }        public async void ReadDb()
        {
            var collection = _database.GetCollection<BsonDocument>("UberCollection");
            var filter = Builders<BsonDocument>.Filter.Eq("name", "Jef");
            var result = await collection.Find(filter).ToListAsync();
            foreach (var dox in result)
            {
                Debug.WriteLine(dox);
            }
        }
    }
}
