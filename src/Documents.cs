using RowingManager.Models;
using LiveChartsCore.Kernel;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace RowingManager {

    public class Documents {

        private static readonly string[] headingsIntervals = new string[] {
            "Date of 2km",
            "Surname and Name",
            "100% Watts of 2km",
            "I Interval 53%-55% of 2km",
            "II Interval 65% of 2km",
            "III Interval 70%-75%-80% of 2km",
            "IIII Interval 88%-92% of 2km"
        };

        private static readonly string[] headings2000m = new string[] {
            "Date",
            "Total Time",
            "Average Watts",
            "500m 1","500m 2","500m 3","500m 4"
        };

        private static readonly string[] headingsDistance = new string[] {
            "Date",
            "Total Time",
            "Average Watts",
            "Average 500m Time"
        };

        private static readonly string[] headingsEndurance = new string[] {
            "Date",
            "Kilograms",
            "Duration",
            "Lift Distance (cm)",
            "Repetitions",
            "Total Joules",
            "Total Watts"
        };

        private static readonly string[] headingsMaxWeight = new string[] {
            "Date",
            "Max Weight"
        };

        public static void GenerateIntervalsSheet() {
            List<PersonModel> people = Database.LoadRecords<PersonModel>("TeamMembers");

            Directory.CreateDirectory("Documents");
            using (DocX document = DocX.Create($"Documents/Intervals {DateTime.Now.ToString("dd_MM_yy")}.docx")) {

                document.SetDefaultFont(new Font("Times New Roman"));

                var p = document.InsertParagraph($"TEAM MEMBER INTERVALS\n{DateTime.Today.ToString("dd/MM/yyyy")}\n").FontSize(14).Bold();
                p.Alignment = Alignment.center;

                var t = document.AddTable(people.Count + 1, 7);
                t.Alignment = Alignment.center;

                for (int i = 0; i < headingsIntervals.Length; i++) {
                    t.Rows[0].Cells[i].Paragraphs[0].Append(headingsIntervals[i]).Bold();
                    t.Rows[0].Cells[i].Paragraphs[0].Alignment = Alignment.center;
                }

                for (int row = 1; row < people.Count + 1; row++) {
                    PersonModel person = people[row - 1];
                    IntervalModel intervals = person.GetIntervalModel();

                    string[] cellTexts = {
                        intervals.Date.ToString("dd/MM/yy"),
                        person.FullName,
                        $"{intervals.TimeIntervals[0]}/{intervals.WattIntervals[0]}",
                        $"{intervals.WattIntervals[1]}-{intervals.WattIntervals[2]}",
                        $"{intervals.WattIntervals[3]}",
                        $"{intervals.WattIntervals[4]}-{intervals.WattIntervals[5]}-{intervals.WattIntervals[6]}",
                        $"{intervals.WattIntervals[7]}-{intervals.WattIntervals[8]}"
                    };

                    for (int col = 0; col < cellTexts.Length; col++)
                        t.Rows[row].Cells[col].Paragraphs[0].Append(cellTexts[col]);
                }

                document.InsertTable(t);

                document.Save();
            }
        }

        public static string GenerateWholeTeamReport() {
            List<PersonModel> people = Database.LoadRecords<PersonModel>("TeamMembers");
            string path = $"Documents\\Team Report {DateTime.Today:dd_MM_yy}";

            foreach (PersonModel person in people)
                GeneratePersonReport(person, path);

            return path;
        }

        public static void GeneratePersonReport(PersonModel person, string path) {
            Directory.CreateDirectory(path);
            using (DocX document = DocX.Create($"{path}/{person.FullName} Tests {DateTime.Now.ToString("dd_MM_yy")}")) {
                document.SetDefaultFont(new Font("Times New Roman"));

                // Document Title
                var p = document.InsertParagraph($"{person.FullName} Tests\n{DateTime.Today.ToString("dd/MM/yyyy")}").FontSize(14).Bold();
                p.Alignment = Alignment.center;

                // 2km Test Table
                DistanceTestModel[] tests2000m = person.GetDistanceTests(2000).ToArray();
                ScoreTable(document, "2000m Tests", headings2000m, tests2000m,
                    (test) => new string[] {
                        test.Date.ToString("dd/MM/yy"),
                        test.TotalDuration.ToString(),
                        test.AvgWatts.ToString(),
                        $"{test.WattBlocks[0]}/{test.TimeBlocks500m[0]}",
                        $"{test.WattBlocks[1]}/{test.TimeBlocks500m[1]}",
                        $"{test.WattBlocks[2]}/{test.TimeBlocks500m[2]}",
                        $"{test.WattBlocks[3]}/{test.TimeBlocks500m[3]}",
                    }
                );

                // 500m, 100m, ... Test Tables
                foreach (int distance in new int[] { 500, 100 }) {                              // <- Loop over similar tables
                    DistanceTestModel[] tests = person.GetDistanceTests(distance).ToArray();    // <- Get Distance test models
                    ScoreTable(document, $"{distance}m Test", headingsDistance, tests,
                        (test) => new string[] {                // <- Anonomous Lambda expression, custom formatting of rows
                            test.Date.ToString("dd/MM/yy"),     // e.g. date formatting
                            test.TotalDuration.ToString(),
                            test.AvgWatts.ToString(),
                            test.AvgDurationPer500m.ToString()
                        }
                    );
                }

                // Max Weight Tests
                foreach (Tuple<string, MaxWeightTestModel.Type> type in new Tuple<string, MaxWeightTestModel.Type>[] {
                    Tuple.Create("Bench Press", MaxWeightTestModel.Type.BenchPress),
                    Tuple.Create("Bench Pull", MaxWeightTestModel.Type.BenchPull),
                    Tuple.Create("Leg Press", MaxWeightTestModel.Type.LegPress),
                    Tuple.Create("Deadlift", MaxWeightTestModel.Type.Deadlift),
                    Tuple.Create("Squat", MaxWeightTestModel.Type.Squat),
                })
                {
                    MaxWeightTestModel[] tests = person.GetMaxWeightTests(type.Item2).ToArray();
                    ScoreTable(document, $"Max Weight {type.Item1} Test", headingsMaxWeight, tests,
                        (test) => new string[] {
                            test.Date.ToString("dd/MM/yy"),
                            test.Kilograms.ToString()
                        }
                    );
                }

                // Endurance Tests
                EnduranceWeightTestModel[] testsEndurance = person.GetEnduranceTests().ToArray();
                ScoreTable(document, "Endurance Weight Tests", headingsEndurance, testsEndurance,
                    (test) => new string[] {
                        test.Date.ToString("dd/MM/yy"),
                        test.Kilograms.ToString(),
                        test.Duration.ToString(),
                        test.LiftDistance.ToString(),
                        test.Repetitions.ToString(),
                        test.Joules.ToString(),
                        test.Watts.ToString()
                    }
                );

                document.Save();
            }
        }

        private static void ScoreTable<T>(DocX document, string tableTitle, string[] columnHeadings, T[] tests, Func<T, string[]> testStrings) {
            var table = document.AddTable(tests.Length + 1, columnHeadings.Length);
            table.Alignment = Alignment.center;

            // Insert Headings
            for (int i = 0; i < columnHeadings.Length; i++) {
                table.Rows[0].Cells[i].Paragraphs[0].Append(columnHeadings[i]).Bold();
                table.Rows[0].Cells[i].Paragraphs[0].Alignment = Alignment.center;
            }

            // Insert Rows
            for (int row = 1; row < tests.Length + 1; row++) {
                T test = tests[row - 1];
                string[] cells = testStrings(test);

                for (int col = 0; col < cells.Length; col++) {
                    table.Rows[row].Cells[col].Paragraphs[0].Append(cells[col]);
                    table.Rows[row].Cells[col].Paragraphs[0].Alignment = Alignment.center;
                }
            }

            // Push changes to document
            var heading = document.InsertParagraph($"\n{tableTitle}").FontSize(14);
            heading.Alignment = Alignment.center;
            document.InsertTable(table);
        }
    }
}
