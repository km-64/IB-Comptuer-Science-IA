using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowingManager.Models {

    public abstract class TestModel : IComparable<TestModel> {

        public DateTime Date { get; set; }

        public int CompareTo(TestModel? other) {
            if(this == other) return 1;
            return DateTime.Compare(Date, other.Date);
        }
    }

    [BsonDiscriminator("MaxWeightTestModel")]
    public class MaxWeightTestModel : TestModel {

        public enum Type {
            BenchPress,
            LegPress,
            BenchPull,
            Deadlift,
            Squat
        }

        public int Kilograms { get; set; }
        public Type MaxType { get; set; }
    }

    [BsonDiscriminator("EnduranceWeightTestModel")]
    public class EnduranceWeightTestModel : TestModel {

        public int Kilograms { get; set; }
        public TimeSpan Duration { get; set; }
        public int LiftDistance { get; set; } // in cm
        public int Repetitions { get; set; }

        [BsonIgnore]
        public double Joules {
            get { return Math.Round((((double)Kilograms * 9.81) * ((double)LiftDistance / 100)) * (double)Repetitions); }
        }

        [BsonIgnore]
        public double Watts {
            get { return Math.Round (Joules / (double)Duration.TotalSeconds); }
        }
    }

    [BsonDiscriminator("DistanceTestModel")]
    public class DistanceTestModel : TestModel {

        public int Distance { get; set; }
        public TimeSpan TotalDuration { get; set; }
        public int AvgWatts { get; set; }
        public TimeSpan AvgDurationPer500m { get; set; }

        public List<int>? WattBlocks { get; set; }
        public List<TimeSpan>? TimeBlocks500m { get; set; }
    }
}
