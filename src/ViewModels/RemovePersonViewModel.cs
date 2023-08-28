using Caliburn.Micro;
using RowingManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xceed.Wpf.Toolkit;

namespace RowingManager.ViewModels {

    public class RemovePersonViewModel : Screen {

        private BindableCollection<PersonModel> _people;
        private PersonModel _selectedPerson;

        public RemovePersonViewModel() {
            Database.DatabaseChanged += OnDatabaseChanged;
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
            set { _selectedPerson = value; }
        }

        public void DeletePerson() {
            if(SelectedPerson == null) {
                Xceed.Wpf.Toolkit.MessageBox.Show("Select Person");
                return;
            }

            Database.DeleteRecord<PersonModel>("TeamMembers", SelectedPerson.Id);
            TryCloseAsync();
        }

        public void Close() {
            TryCloseAsync();
        }

        public void OnDatabaseChanged(object? source, EventArgs e) {
            NotifyOfPropertyChange(() => People);
        }
    }
}
