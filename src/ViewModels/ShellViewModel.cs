using Caliburn.Micro;
using RowingManager.Models;
using RowingManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

// REFERENCE: elements taken from (Corey, 2017)

namespace RowingManager.ViewModels {

    public class ShellViewModel : Screen {

        public IndividualViewModel IndividualViewModel { get; set; }
        public ManageTeamViewModel ManageTeamViewModel { get; set; }

        IWindowManager manager = new WindowManager();

        public ShellViewModel() {
            IndividualViewModel = new IndividualViewModel();
            ManageTeamViewModel = new ManageTeamViewModel();
        }


        public void AddNewPerson() {
            manager.ShowWindowAsync(new AddPersonViewModel(), null, null);
        }

        public void RemovePerson() {
            manager.ShowWindowAsync(new RemovePersonViewModel(), null, null);
        }


        public void AddNewTest() {
            manager.ShowWindowAsync(new AddTestScoreViewModel(), null, null);
        }

        public void GenerateIntervals() {
            Documents.GenerateIntervalsSheet();
        }

        public void GeneratePersonReport() {
            List<PersonModel> people = Database.LoadRecords<PersonModel>("TeamMembers");
            Documents.GeneratePersonReport(people[people.Count -1], "Documents/Individual Reports");
        }

        public void GenerateTeamReport() {
            Documents.GenerateWholeTeamReport();
            
        }
    }
}
