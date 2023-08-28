using Caliburn.Micro;
using RowingManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RowingManager.ViewModels {

    public class AddPersonViewModel : Screen {

		private string _firstName;
		private string _lastName;
		private DateTime _dateOfBirth;

		public AddPersonViewModel() {
			DateOfBirth = new DateTime(2000, 1, 1, 0,0,0,0, DateTimeKind.Utc);
		}

		public string FirstName {
			get { return _firstName; }
			set { _firstName = value; }
		}

		public string LastName {
			get { return _lastName; }
			set { _lastName = value; }
		}

		public DateTime DateOfBirth {
			get { return _dateOfBirth; }
			set {
				_dateOfBirth = value;
				NotifyOfPropertyChange(() => DateOfBirth);
			}
		}

		public void Cancel() {
			TryCloseAsync();
		}

		public void AddPerson(string firstName, string lastName, DateTime dateOfBirth) {
			// Access database, insert record
			PersonModel person = new PersonModel {
				FirstName = firstName,
				LastName = lastName,
				DateOfBirth = dateOfBirth
			};

			Database.InsertRecord<PersonModel>("TeamMembers", person);
			TryCloseAsync();
		}

		public bool CanAddPerson(string firstName, string lastName, DateTime dateOfBirth) {
			if(!String.IsNullOrEmpty(firstName) && !String.IsNullOrEmpty(lastName))
				return true;
			else
				return false;
		}
	}
}
