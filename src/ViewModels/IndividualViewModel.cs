using Caliburn.Micro;
using RowingManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Windows.Markup;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace RowingManager.ViewModels {

    public class IndividualViewModel : Screen {

        private BindableCollection<PersonModel> _people;
        private BindableCollection<DistanceTestModel> _tests2000m;
        private BindableCollection<DistanceTestModel> _tests500m;
        private BindableCollection<DistanceTestModel> _tests100m;

        private BindableCollection<EnduranceWeightTestModel> _enduranceWeightTests;

        private BindableCollection<MaxWeightTestModel> _testsMaxBenchPress;
        private BindableCollection<MaxWeightTestModel> _testsMaxBenchPull;
        private BindableCollection<MaxWeightTestModel> _testsMaxLegPress;
        private BindableCollection<MaxWeightTestModel> _testsMaxDeadlift;
        private BindableCollection<MaxWeightTestModel> _testsMaxSquat;


        private PersonModel _selectedPerson;
        private DistanceTestModel _selectedDistanceTest;

        public IndividualViewModel() {
            Database.DatabaseChanged += OnDatabaseChanged;
        }

        public BindableCollection<DistanceTestModel> Tests2000m {
            get {
                if (SelectedPerson == null)
                    return new BindableCollection<DistanceTestModel>();

                _tests2000m = new BindableCollection<DistanceTestModel>(SelectedPerson.GetDistanceTests(2000));
                return _tests2000m;
            }
            set { _tests2000m = value; }
        }
        public BindableCollection<DistanceTestModel> Tests500m {
            get {
                if (SelectedPerson == null)
                    return new BindableCollection<DistanceTestModel>();

                _tests500m = new BindableCollection<DistanceTestModel>(SelectedPerson.GetDistanceTests(500));
                return _tests500m;
            }
            set { _tests500m = value; }
        }

        public BindableCollection<DistanceTestModel> Tests100m {
            get {
                if (SelectedPerson == null)
                    return new BindableCollection<DistanceTestModel>();

                _tests100m = new BindableCollection<DistanceTestModel>(SelectedPerson.GetDistanceTests(100));
                return _tests100m;
            }
            set { _tests100m = value; }
        }
        
        public BindableCollection<MaxWeightTestModel> TestsMaxBenchPress {
            get {
                if (SelectedPerson == null)
                    return new BindableCollection<MaxWeightTestModel>();

                _testsMaxBenchPress = new BindableCollection<MaxWeightTestModel>(SelectedPerson.GetMaxWeightTests(MaxWeightTestModel.Type.BenchPress));
                return _testsMaxBenchPress;
            }
            set { _testsMaxBenchPress = value; }
        }

        public BindableCollection<MaxWeightTestModel> TestsMaxBenchPull {
            get {
                if (SelectedPerson == null)
                    return new BindableCollection<MaxWeightTestModel>();

                _testsMaxBenchPull = new BindableCollection<MaxWeightTestModel>(SelectedPerson.GetMaxWeightTests(MaxWeightTestModel.Type.BenchPull));
                return _testsMaxBenchPull;
            }
            set { _testsMaxBenchPull = value; }
        }

        public BindableCollection<MaxWeightTestModel> TestsMaxLegPress {
            get {
                if (SelectedPerson == null)
                    return new BindableCollection<MaxWeightTestModel>();

                _testsMaxLegPress = new BindableCollection<MaxWeightTestModel>(SelectedPerson.GetMaxWeightTests(MaxWeightTestModel.Type.LegPress));
                return _testsMaxLegPress;
            }
            set { _testsMaxLegPress = value; }
        }

        public BindableCollection<MaxWeightTestModel> TestsMaxDeadlift {
            get {
                if (SelectedPerson == null)
                    return new BindableCollection<MaxWeightTestModel>();

                _testsMaxDeadlift = new BindableCollection<MaxWeightTestModel>(SelectedPerson.GetMaxWeightTests(MaxWeightTestModel.Type.Deadlift));
                return _testsMaxDeadlift;
            }
            set { _testsMaxDeadlift = value; }
        }

        public BindableCollection<MaxWeightTestModel> TestsMaxSquat {
            get {
                if (SelectedPerson == null)
                    return new BindableCollection<MaxWeightTestModel>();

                _testsMaxSquat = new BindableCollection<MaxWeightTestModel>(SelectedPerson.GetMaxWeightTests(MaxWeightTestModel.Type.Squat));
                return _testsMaxSquat;
            }
            set { _testsMaxSquat = value; }
        }

        public BindableCollection<EnduranceWeightTestModel> TestsEnduranceWeight {
            get {
                if (SelectedPerson == null)
                    return new BindableCollection<EnduranceWeightTestModel>();

                _enduranceWeightTests = new BindableCollection<EnduranceWeightTestModel>(SelectedPerson.GetEnduranceTests());
                return _enduranceWeightTests;
            }
            set { _enduranceWeightTests = value; }
        }

        public BindableCollection<PersonModel> People {
            get {
                _people = new BindableCollection<PersonModel>(
                    Database.LoadRecords<PersonModel>("TeamMembers"));

                return _people;
            }
            set { _people = value; }
        }

        public PersonModel SelectedPerson {
            get { return _selectedPerson; }
            set {
                _selectedPerson = value;
                NotifyOfPropertyChange(() => SelectedPerson);

                NotifyOfPropertyChange(() => Tests2000m);
                NotifyOfPropertyChange(() => Tests500m);
                NotifyOfPropertyChange(() => Tests100m);

                NotifyOfPropertyChange(() => TestsEnduranceWeight);

                NotifyOfPropertyChange(() => TestsMaxBenchPress);
                NotifyOfPropertyChange(() => TestsMaxBenchPull);
                NotifyOfPropertyChange(() => TestsMaxLegPress);
                NotifyOfPropertyChange(() => TestsMaxDeadlift);
                NotifyOfPropertyChange(() => TestsMaxSquat);

                NotifyOfPropertyChange(() => Series500mTimesSelected);
                NotifyOfPropertyChange(() => Series500mWattsSelected);
                NotifyOfPropertyChange(() => SeriesTimeProgress2000m);

                NotifyOfPropertyChange(() => SeriesBenchPressProgress);
                NotifyOfPropertyChange(() => SeriesBenchPullProgress);
                NotifyOfPropertyChange(() => SeriesLegPressProgress);
                NotifyOfPropertyChange(() => SeriesDeadliftProgress);
                NotifyOfPropertyChange(() => SeriesSquatProgress);

                NotifyOfPropertyChange(() => SeriesEnduranceProgress);

                NotifyOfPropertyChange(() => SeriesTimeProgress500m);
                NotifyOfPropertyChange(() => SeriesTimeProgress100m);
            }
        }

        public DistanceTestModel Selected2000mDistanceTest {
            get { return _selectedDistanceTest; }
            set {
                _selectedDistanceTest = value;
                NotifyOfPropertyChange(() => Series500mTimesSelected);
                NotifyOfPropertyChange(() => Series500mWattsSelected);
            }
        }
        public DistanceTestModel Selected500mDistanceTest {
            get { return _selectedDistanceTest; }
            set { _selectedDistanceTest = value; }
        }
        public DistanceTestModel Selected100mDistanceTest {
            get { return _selectedDistanceTest; }
            set { _selectedDistanceTest = value; }
        }



        public ISeries[] Series500mTimesSelected {
            get {
                if (Selected2000mDistanceTest != null) {
                    if (Selected2000mDistanceTest.TimeBlocks500m != null) {
                        List<int> Seconds = new List<int>();

                        foreach (TimeSpan time in Selected2000mDistanceTest.TimeBlocks500m) {
                            Seconds.Add((int)time.TotalSeconds);
                        }
                        return new ISeries[] {
                            new LineSeries<int> {
                                Values = Seconds.ToArray(),
                                Stroke = new SolidColorPaint(SKColors.DarkBlue),
                                GeometryStroke = new SolidColorPaint(SKColors.Black),
                                GeometryFill = new SolidColorPaint(SKColors.White),
                                GeometrySize = 10,
                                Fill = new SolidColorPaint(SKColors.Transparent)
                            }
                        };
                    }
                }

                return new ISeries[] {
                    new ColumnSeries<int> { Values = new int[] { 0,0,0,0 } }
                };
            }
        }
        public ISeries[] Series500mWattsSelected {
            get {
                if (Selected2000mDistanceTest != null) {
                    if (Selected2000mDistanceTest.WattBlocks != null) {
                        return new ISeries[] {
                            new LineSeries<int> {
                                Values = Selected2000mDistanceTest.WattBlocks.ToArray(),
                                Stroke = new SolidColorPaint(SKColors.DarkBlue),
                                GeometryStroke = new SolidColorPaint(SKColors.Black),
                                GeometryFill = new SolidColorPaint(SKColors.White),
                                GeometrySize = 10,
                                Fill = new SolidColorPaint(SKColors.Transparent)
                            }
                        };
                    }
                }

                return new ISeries[] {
                    new ColumnSeries<int> {
                        Values = new int[] { 0,0,0,0 }
                    }
                };
            }
        }


        public Axis[] XAxesForSelected { get; set; } = new Axis[] {
            new Axis {
                Name = "500m Times", NameTextSize = 12,
                Labels = new string[] { "500m", "500m", "500m", "500m" }
            }
        };

        public Axis[] XAxesEmptyLabels { get; set; } = new Axis[] {
            new Axis {
                Name = "", NameTextSize = 12,
                Labels = new string[] { "" }
            }
        };

        public Axis[] YAxes500mWatts { get; set; } = new Axis[] {new Axis{ MinLimit=0, MaxLimit=500, Name = "Watts per 500m", NameTextSize=12 }};
        public Axis[] YAxes500mTimes { get; set; } = new Axis[] {new Axis{ MinLimit=0, MaxLimit=2*60,  Name = "Seconds per 500m", NameTextSize=12 }};
        public Axis[] YAxesWeight { get; set; } = new Axis[] {new Axis{ MinLimit=0, MaxLimit=500, Name = "Kilograms", NameTextSize=12 }};
        public Axis[] YAxesWatts { get; set; } = new Axis[] {new Axis{ MinLimit=0, MaxLimit=1000, Name = "Watts", NameTextSize=12 }};
        public Axis[] YAxes2000mTimesProgress { get; set; } = new Axis[] { new Axis { MinLimit=60*5, MaxLimit=60*10, Name="Time (seconds)", NameTextSize=12 } };
        public Axis[] YAxes500mTimesProgress { get; set; } = new Axis[] { new Axis { MinLimit=60, MaxLimit=60*2, Name="Time (seconds)", NameTextSize=12 } };
        public Axis[] YAxes100mTimesProgress { get; set; } = new Axis[] { new Axis { MinLimit=0, MaxLimit=50, Name="Time (seconds)", NameTextSize=12 } };

        public ISeries[] SeriesTimeProgress2000m {
            get { return GetTimeProgressSeries(2000); }
        }

        public ISeries[] SeriesTimeProgress500m {
            get { return GetTimeProgressSeries(500); }
        }

        public ISeries[] SeriesTimeProgress100m {
            get { return GetTimeProgressSeries(100); }
        }

        private ISeries[] GetTimeProgressSeries(int distance) {
            if (SelectedPerson != null) {
                List<DistanceTestModel> tests = SelectedPerson.GetDistanceTests(distance);
                List<int> timeScoresSeconds = new List<int>();
                foreach (DistanceTestModel test in tests) {
                    timeScoresSeconds.Add((int)test.TotalDuration.TotalSeconds);
                }

                if (tests != null) {
                    return new ISeries[] {
                            new LineSeries<int> {
                                Values = timeScoresSeconds.ToArray(),
                                Stroke = new SolidColorPaint(SKColors.DarkBlue),
                                GeometryStroke = new SolidColorPaint(SKColors.Black),
                                GeometryFill = new SolidColorPaint(SKColors.White),
                                GeometrySize = 10,
                                Fill = new SolidColorPaint(SKColors.Transparent)
                            }
                        };
                }
            }

            return new ISeries[] {
                new ColumnSeries<int> {
                    Values = new int[] { 0,0,0,0 }
                }
            };
        }

        public ISeries[] SeriesBenchPressProgress {
            get { return GetMaxWeightSeries(MaxWeightTestModel.Type.BenchPress); }
        }

        public ISeries[] SeriesBenchPullProgress {
            get { return GetMaxWeightSeries(MaxWeightTestModel.Type.BenchPull); }
        }
        public ISeries[] SeriesLegPressProgress {
            get { return GetMaxWeightSeries(MaxWeightTestModel.Type.LegPress); }
        }
        public ISeries[] SeriesDeadliftProgress {
            get { return GetMaxWeightSeries(MaxWeightTestModel.Type.Deadlift); }
        }
        public ISeries[] SeriesSquatProgress {
            get { return GetMaxWeightSeries(MaxWeightTestModel.Type.Squat); }
        }

        private ISeries[] GetMaxWeightSeries(MaxWeightTestModel.Type type) {
            if (SelectedPerson != null) {
                List<MaxWeightTestModel> tests = SelectedPerson.GetMaxWeightTests(type);
                List<int> weightScores = new List<int>();

                foreach (MaxWeightTestModel test in tests) {
                    weightScores.Add(test.Kilograms);
                }

                if (tests != null) {
                    return new ISeries[] {
                        new LineSeries<int> {
                                Values = weightScores.ToArray(),
                                Stroke = new SolidColorPaint(SKColors.DarkBlue),
                                GeometryStroke = new SolidColorPaint(SKColors.Black),
                                GeometryFill = new SolidColorPaint(SKColors.White),
                                GeometrySize = 10,
                                Fill = new SolidColorPaint(SKColors.Transparent)
                            }
                    };
                }
            }

            return new ISeries[] { new ColumnSeries<int> { Values = new int[0] { } }};
        }

        public ISeries[] SeriesEnduranceProgress {
            get {
                if(SelectedPerson != null) {
                    List<EnduranceWeightTestModel> tests = SelectedPerson.GetEnduranceTests();
                    List<double> wattScores = new List<double>();

                    foreach(EnduranceWeightTestModel test in tests) {
                        wattScores.Add(test.Watts);
                    }
                    
                    if(tests != null) {
                        return new ISeries[] {
                            new LineSeries<double> {
                                Values = wattScores.ToArray(),
                                Stroke = new SolidColorPaint(SKColors.DarkBlue),
                                GeometryStroke = new SolidColorPaint(SKColors.Black),
                                GeometryFill = new SolidColorPaint(SKColors.White),
                                GeometrySize = 10,
                                Fill = new SolidColorPaint(SKColors.Transparent)
                            }
                        };
                    }
                }
                return new ISeries[] { new ColumnSeries<int> { Values = new int[0] { } }};
            }
        }

        public void OnDatabaseChanged(object? source, EventArgs e) { // <- IndividualViewModel subscribes to event
            NotifyOfPropertyChange(() => People);   // <- Updates GUI for all elements displaying the 'People' property
        }
    }
}
