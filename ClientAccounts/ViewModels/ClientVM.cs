using ClientAccounts.Models;
using System;
using System.ComponentModel;

namespace ClientAccounts.ViewModels
{
	// ClientVM отличается от Client дополнением свойств (Changer, ChangingTime, ChangedData, ChangingType) и
	// логикой при изменении свойств клиента
	public class ClientVM : IDataErrorInfo, INotifyPropertyChanged
	{
		public IUserType Changer { get; set; }
		public Guid Id { get; set; } = new Guid();
		string? _lastName;
		public string? LastName
		{
			get => _lastName;
			set
			{
				var oldValue = _lastName;
				_lastName = value;
				HandlePropertyChanges(nameof(LastName), "фамилия", oldValue, value);
			}
		}

		string? _firstName;
		public string? FirstName
		{
			get => _firstName;
			set
			{
				var oldValue = _firstName;

				_firstName = value;
				HandlePropertyChanges(nameof(FirstName), "имя", oldValue, value);
			}
		}
		string? _middleName;
		public string? MiddleName
		{
			get => _middleName;
			set
			{
				var oldValue = _middleName;
				_middleName = value;
				HandlePropertyChanges(nameof(MiddleName), "отчество", oldValue, value);
			}
		}

		string? _phoneNumber;
		public string? PhoneNumber
		{
			get => _phoneNumber;
			set
			{
				var oldValue = _phoneNumber;				
				_phoneNumber = value;
				HandlePropertyChanges(nameof(PhoneNumber), "номер телефона", oldValue, value);
			}
		}		

		string? _passport;
		public string? Passport
		{
			get => (Changer is Manager or null) ? _passport : "*******";
			set
			{
				var oldValue = _passport;
				_passport = value;
				HandlePropertyChanges(nameof(Passport), "паспорт", oldValue, value);
			}
		}
		public DateTime ChangingTime { get; internal set; }
		public string? ChangedData { get; internal set; }
		public string? ChangingType { get; internal set; }
		public string? LastChanger { get; internal set; }

		public event PropertyChangedEventHandler? PropertyChanged;

		bool _buttonEnabled;		

		public bool ButtonEnabled
		{
			get => _buttonEnabled;
			set
			{
				_buttonEnabled = value;
				PropertyChanged(this, new PropertyChangedEventArgs(nameof(ButtonEnabled)));
			}
		}
		public string? Error { get; set; }

		public string this[string columnName]
		{
			get
			{
				if (columnName == nameof(PhoneNumber))
					if (String.IsNullOrWhiteSpace(PhoneNumber))
					{
						ButtonEnabled = false;
						return Error = "необходимо указать номер телефона";
					}
					else
					{
						ButtonEnabled = true;
						return Error = "";
					}
				else
				{
					ButtonEnabled = true;
					return "";
				}
			}
		}
		// Метод обработки при изменении каких-либо свойств клиента		
		public void HandlePropertyChanges<T>(string propertyName, string changingType, T oldValue, T newValue)
		{
			ChangingTime = DateTime.Now;
			ChangedData = $"{oldValue} => {newValue}";
			ChangingType = changingType;
			LastChanger = Changer?.ToString();
			if (PropertyChanged is not null)
			{
				var propertyNames = new[] {
					propertyName ,
					nameof(ChangingTime),
					nameof(ChangedData),
					nameof(ChangingType),
					nameof(LastChanger)
				};
				foreach (var propName in propertyNames)
					PropertyChanged(this, new PropertyChangedEventArgs(propName));
			}
		}
	}
}