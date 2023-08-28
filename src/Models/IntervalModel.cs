using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RowingManager.Models {

    public class IntervalModel {

        public DateTime Date { get; set; }

        public int[] WattIntervals { get; set; } = new int[9];
        public TimeSpan[] TimeIntervals { get; set; } = new TimeSpan[9];

        public readonly double[] IntervalPercentages = {
            1.0d,                   // 100%
            0.53d, 0.55d,           //    I
            0.65d,                  //   II
            0.70d, 0.75d, 0.80d,    //  III
            0.88d, 0.92d            //   IV
        };

        public IntervalModel(PersonModel person) {
            List<DistanceTestModel> tests = person.GetDistanceTests(2000);
            if (tests.Count == 0) return;

            DistanceTestModel latest = tests[0];
            foreach (DistanceTestModel test in tests) {
                if(test.Date > latest.Date)
                    latest = test;
            }

            this.Date = latest.Date;

            for (int i = 0; i < 9; i++) {
                TimeIntervals[i] = latest.AvgDurationPer500m * IntervalPercentages[i];
                WattIntervals[i] = (int)Math.Round((double)latest.AvgWatts * IntervalPercentages[i]);
            }
        }
    }
}
