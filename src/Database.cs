using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RowingManager.Models;
using MongoDB.Bson.Serialization;

// REFERENCE: elements taken from (Corey, 2019)

namespace RowingManager {
    public class Database {

        private static IMongoDatabase db;

        public delegate void DatabaseChangedEventHandler(object? source, EventArgs args);
        public static event DatabaseChangedEventHandler DatabaseChanged;

        static Database() { // Class Map Registration
            BsonClassMap.RegisterClassMap<TestModel>();
            BsonClassMap.RegisterClassMap<MaxWeightTestModel>();
            BsonClassMap.RegisterClassMap<EnduranceWeightTestModel>();
            BsonClassMap.RegisterClassMap<DistanceTestModel>();
            MongoClient client = new MongoClient();
            db = client.GetDatabase("RowingDB");
        }

        public static void InsertRecord<T>(string table, T record) {
            var collection = db.GetCollection<T>(table);
            collection.InsertOne(record);
            OnDatabaseChanged();
        }

        public static List<T> LoadRecords<T>(string table) {
            var collection = db.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();
        }

        public static T LoadRecordById<T>(string table, Guid id) {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);
            return collection.Find(filter).First();
        }

        public static void UpsertRecord<T>(string table, Guid id, T record) {
            var collection = db.GetCollection<T>(table);
            var result = collection.ReplaceOne(
                new BsonDocument("_id", id),
                record,
                new ReplaceOptions { IsUpsert = true }
            );
            OnDatabaseChanged();
        }

        public static void DeleteRecord<T>(string table, Guid id) {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);
            collection.DeleteOne(filter);
            OnDatabaseChanged();

        }

        static protected void OnDatabaseChanged() {
            if(DatabaseChanged != null)
                DatabaseChanged(null, EventArgs.Empty);
        }
    }
}
