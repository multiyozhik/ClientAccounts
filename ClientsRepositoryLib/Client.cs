using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientsRepositoryLib
{
	public class Client
	{
		public Guid Id { get; set; }
		public string LastName { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string PhoneNumber { get; set; }
		public string Passport { get; set; }
		public DateTime ChangingTime { get; set; }
		public string ChangedData { get; set; }
		public string ChangingType { get; set; }
		public string LastChanger { get; set; }
		public Client(Guid id, string lastName, string firstName, string middleName,
			string phoneNumber, string passport, DateTime changingTime,
			string changedData, string changingType, string lastChanger)
		{
			Id = id;
			LastName = lastName;
			FirstName = firstName;
			MiddleName = middleName;
			PhoneNumber = phoneNumber;
			Passport = passport;
			ChangingTime = changingTime;
			ChangedData = changedData;
			ChangingType = changingType;
			LastChanger = lastChanger;
		}
	}
}
