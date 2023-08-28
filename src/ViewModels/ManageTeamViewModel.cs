using Caliburn.Micro;
using RowingManager.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RowingManager.ViewModels {

    public class ManageTeamViewModel : Screen {

        IWindowManager manager = new WindowManager();

        private BindableCollection<PersonModel> _people;
        private PersonModel _selectedPerson;

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

        public ManageTeamViewModel() {
            Database.DatabaseChanged += OnDatabaseChanged;
        }

        public void AddPerson() { manager.ShowDialogAsync(new AddPersonViewModel()); }
        public void AddTest() { manager.ShowDialogAsync(new AddTestScoreViewModel()); }
        public void RemovePerson() { manager.ShowDialogAsync(new RemovePersonViewModel()); }

        public void GenerateIntervalList() {
            Documents.GenerateIntervalsSheet();
            Process.Start("explorer.exe", "Documents");
        }

        public void ExportPersonTests() {
            if(SelectedPerson == null) {
                Xceed.Wpf.Toolkit.MessageBox.Show("Select person to generate report for.");
                return;
            }

            string path = "Documents\\Individual Reports";
            Documents.GeneratePersonReport(SelectedPerson, path);
            Process.Start("explorer.exe", path);
        }

        public void ExportEntireTeam() {
            string path = Documents.GenerateWholeTeamReport();
            Process.Start("explorer.exe", path);
        }


        public void OnDatabaseChanged(object sender, EventArgs e) {
            NotifyOfPropertyChange(() => People);
        }
    }
}
