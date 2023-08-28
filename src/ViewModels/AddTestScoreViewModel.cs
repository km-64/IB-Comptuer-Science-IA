using Caliburn.Micro;
using RowingManager.Models;
using System;
using System.Collections.Generic;
using Xceed.Wpf.Toolkit;

namespace RowingManager.ViewModels {

	public class AddTestScoreViewModel : Screen {

		private BindableCollection<PersonModel> _people;
		private PersonModel _selectedPerson;

		private DateTime _date;
		private bool _type2000mSelected;
		private bool _type500mSelected;
		private bool _type100mSelected;
		private TimeSpan _totalTime;
		private int _averageWatts;
		private TimeSpan _average500mTime;

		private int _maxWeight;

		public AddTestScoreViewModel() {
			Database.DatabaseChanged += OnDatabaseChanged;
			Type2000mSelected = true;
			BenchPressSelected= true;
			Date = DateTime.UtcNow.Date;
		}

		public PersonModel SelectedPerson {
			get { return _selectedPerson; }
			set { _selectedPerson = value; }
		}

		public BindableCollection<PersonModel> People {
            get {
                _people = new BindableCollection<PersonModel>(Database.LoadRecords<PersonModel>("TeamMembers"));
                return _people;
            }
            set { _people = value; }
        }


        public DateTime Date {
			get { return _date; }
			set {
				_date = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0, DateTimeKind.Utc);
				NotifyOfPropertyChange(() => Date);
			}
		}

		public bool Type2000mSelected {
			get { return _type2000mSelected; }
			set {
				_type2000mSelected = value;
				NotifyOfPropertyChange(() => Type2000mSelected);
				NotifyOfPropertyChange(() => TimeBlocksVisibility);
			}
		}

		public bool Type500mSelected {
			get { return _type500mSelected; }
			set {
				_type500mSelected = value;
				NotifyOfPropertyChange(() => Type500mSelected);
			}
		}

		public bool Type100mSelected {
			get { return _type100mSelected; }
			set {
				_type100mSelected = value;
				NotifyOfPropertyChange(() => Type100mSelected);
			}
		}


		public TimeSpan TotalTime {
			get { return _totalTime; }
			set { _totalTime = value; }
		}

		public int AverageWatts {
			get { return _averageWatts; }
			set { _averageWatts = value; }
		}

		public TimeSpan Average500mTime {
			get { return _average500mTime; }
			set { _average500mTime = value; }
		}

		private int[] _wattBlocks = new int[4] { 0,0,0,0 };
		private TimeSpan[] _timeBlocks = new TimeSpan[4] {
			new TimeSpan(0,0,0), new TimeSpan(0,0,0), new TimeSpan(0,0,0), new TimeSpan(0,0,0),
		};

        #region WattBlocks
        public int WattBlock1 { get { return _wattBlocks[0]; } set { _wattBlocks[0] = value; } }
		public int WattBlock2 { get { return _wattBlocks[1]; } set { _wattBlocks[1] = value; } }
		public int WattBlock3 { get { return _wattBlocks[2]; } set { _wattBlocks[2] = value; } }
		public int WattBlock4 { get { return _wattBlocks[3]; } set { _wattBlocks[3] = value; } }
        #endregion

        #region TimeBlocks
        public TimeSpan TimeBlock1 { get { return _timeBlocks[0]; } set { _timeBlocks[0] = value; } }
		public TimeSpan TimeBlock2 { get { return _timeBlocks[1]; } set { _timeBlocks[1] = value; } }
		public TimeSpan TimeBlock3 { get { return _timeBlocks[2]; } set { _timeBlocks[2] = value; } }
		public TimeSpan TimeBlock4 { get { return _timeBlocks[3]; } set { _timeBlocks[3] = value; } }
		#endregion


		private bool _benchPressSelected;
		private bool _benchPullSelected;
		private bool _legPressSelected;
		private bool _deadliftSelected;
		private bool _squatSelected;

		public bool BenchPressSelected {
			get { return _benchPressSelected; }
			set { _benchPressSelected = value; }
		}

		public bool BenchPullSelected {
			get { return _benchPullSelected; }
			set { _benchPullSelected = value; }
		}

		public bool LegPressSelected {
			get { return _legPressSelected; }
			set { _legPressSelected = value; }
		}

		public bool DeadliftSelected {
			get { return _deadliftSelected; }
			set { _deadliftSelected = value; }
		}

		public bool SquatSelected {
			get { return _squatSelected; }
			set { _squatSelected = value; }
		}

		public int MaxWeight {
			get { return _maxWeight; }
			set { _maxWeight = value; }
		}


		private int _enduranceWeight;
		private TimeSpan _enduranceDuration;
		private int _enduranceRepetitions;

		public int EnduranceWeight {
			get { return _enduranceWeight; }
			set { _enduranceWeight = value; }
		}

		public TimeSpan EnduranceDuration {
			get { return _enduranceDuration; }
			set { _enduranceDuration = value; }
		}

		private int _enduranceLiftDistance;

		public int EnduranceLiftDistance {
			get { return _enduranceLiftDistance; }
			set { _enduranceLiftDistance = value; }
		}


		public int EnduranceRepetitions {
			get { return _enduranceRepetitions; }
			set { _enduranceRepetitions = value; }
		}
		

		public System.Windows.Visibility TimeBlocksVisibility {
			get {
				if(Type2000mSelected == true)
					return System.Windows.Visibility.Visible;
				else
					return System.Windows.Visibility.Collapsed;
			}
		}

		public void AddDistanceTest() {
			if(SelectedPerson == null) {
				MessageBox.Show("Select a person.", "Error");
				return;
			}

			DistanceTestModel test = new DistanceTestModel();

			test.Date = Date;
            test.TotalDuration = TotalTime;
            test.AvgDurationPer500m = Average500mTime;
            test.AvgWatts = AverageWatts;

            if (Type2000mSelected) {
				test.Distance = 2000;

				test.WattBlocks = new List<int>(_wattBlocks);
				test.TimeBlocks500m = new List<TimeSpan>(_timeBlocks);
			}
			else if(Type500mSelected) test.Distance = 500;
			else if(Type100mSelected) test.Distance = 100;

			if (SelectedPerson.Tests == null)
				SelectedPerson.Tests = new List<TestModel>();

			SelectedPerson.Tests.Add(test);

			Database.UpsertRecord<PersonModel>("TeamMembers", SelectedPerson.Id, SelectedPerson);
			TryCloseAsync();
		}

		public void AddMaxWeightTest() {
			if(SelectedPerson == null) {
				MessageBox.Show("Select a person.", "Error");
				return;
			}

			MaxWeightTestModel test = new MaxWeightTestModel();
			test.Date = Date;
			test.Kilograms = MaxWeight;

			if(BenchPressSelected)		test.MaxType = MaxWeightTestModel.Type.BenchPress;
			else if(BenchPullSelected)	test.MaxType = MaxWeightTestModel.Type.BenchPull;
			else if(LegPressSelected)	test.MaxType = MaxWeightTestModel.Type.LegPress;
			else if(DeadliftSelected)	test.MaxType = MaxWeightTestModel.Type.Deadlift;
			else if(SquatSelected)		test.MaxType = MaxWeightTestModel.Type.Squat;

			if (SelectedPerson.Tests == null)
				SelectedPerson.Tests = new List<TestModel>();

			SelectedPerson.Tests.Add(test);

			Database.UpsertRecord<PersonModel>("TeamMembers", SelectedPerson.Id, SelectedPerson);
			TryCloseAsync();
		}

		public void AddEnduranceTest() {
			if(SelectedPerson == null) {
				MessageBox.Show("Select a person.", "Error");
				return;
			}

			EnduranceWeightTestModel test = new EnduranceWeightTestModel();
			test.Date = Date;
			test.Kilograms = EnduranceWeight;
			test.Duration = EnduranceDuration;
			test.LiftDistance = EnduranceLiftDistance;
			test.Repetitions = EnduranceRepetitions;

            if (SelectedPerson.Tests == null)
                SelectedPerson.Tests = new List<TestModel>();

            SelectedPerson.Tests.Add(test);

            Database.UpsertRecord<PersonModel>("TeamMembers", SelectedPerson.Id, SelectedPerson);
            TryCloseAsync();
        }

        public void OnDatabaseChanged(object? source, EventArgs e) {
            NotifyOfPropertyChange(() => People);
        }
    }
}
