using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace RowingManager.Models {

    public class PersonModel {

        [BsonId]
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [BsonElement("dob")]
        public DateTime DateOfBirth { get; set; }

        public List<TestModel>? Tests { get; set; }

        [BsonIgnore]
        public int BirthYear { get { return DateOfBirth.Year; } }

        [BsonIgnore]
        public string FullName { get { return $"{FirstName} {LastName}"; } }

        public List<DistanceTestModel> GetDistanceTests(int distance) {

            // List of all DistanceTests for this person
            List<DistanceTestModel> searched = new List<DistanceTestModel>();

            // Return an empty list if the test field is null
            if(Tests == null) { return searched; }

            // Loop through tests and search for tests with the correct type
            foreach(TestModel test in Tests) {
                if(test.GetType() == typeof(DistanceTestModel)) {
                    DistanceTestModel distanceModel = (DistanceTestModel)test;
                    if(distanceModel.Distance == distance) {
                        searched.Add(distanceModel);
                    }
                }
            }

            // Sort by date
            searched.Sort();    // <-- Sort method called on List class (concise due to IComparable)
            return searched;
        }

        public List<MaxWeightTestModel> GetMaxWeightTests(MaxWeightTestModel.Type type) {
            List<MaxWeightTestModel> searched = new List<MaxWeightTestModel>();

            if (Tests == null) { return searched; }

            foreach(TestModel test in Tests) {
                if(test.GetType() == typeof(MaxWeightTestModel)) {
                    MaxWeightTestModel maxWeightTestModel = (MaxWeightTestModel)test;

                    if(maxWeightTestModel.MaxType == type)
                        searched.Add(maxWeightTestModel);
                }
            }

            searched.Sort();
            return searched;
        }

        public List<EnduranceWeightTestModel> GetEnduranceTests() {
            List<EnduranceWeightTestModel> searched = new List<EnduranceWeightTestModel>();

            if(Tests == null) return searched;

            foreach(TestModel test in Tests) {
                if(test.GetType() == typeof(EnduranceWeightTestModel)) {
                    EnduranceWeightTestModel enduranceTestModel = (EnduranceWeightTestModel)test;
                    searched.Add(enduranceTestModel);
                }
            }

            searched.Sort();
            return searched;
        }

        public IntervalModel GetIntervalModel() {
            return new IntervalModel(this);
        }
    }
}
