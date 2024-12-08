using ClientAccounts.Models;
using ClientAccounts.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using static System.Collections.Specialized.NotifyCollectionChangedAction;
using ClientsRepositoryLib;

namespace ClientAccounts.ViewModels
{
	class ClientsInfoVM : INotifyPropertyChanged
	{
		public string ChangingNotify { get; set; } // уведомление об изменении счета

		ClientVM? selectedClientVM;
		public ClientVM SelectedClientVM
		{
			get => selectedClientVM;
			set
			{
				selectedClientVM = value;
				NotifyPropertyChanged(nameof(SelectedClientVM));
			}
		}
		public bool IsReadOnly => Changer is not Manager;
		public bool IsCanAddNewClient => Changer is Manager;

		public IUserType? Changer { get; internal set; } // по сути selectedUser=Manager или Consultant

		internal ClientAccountsVM ClientAccountsVM { get; }
		IClientsRepository ClientsRepository { get; }
		public ClientsInfoVM(IClientsRepository clientsRepository, ClientAccountsVM clientAccountsVM)
		{
			ClientsRepository = clientsRepository;
			ClientAccountsVM = clientAccountsVM;
			UpdateClientsList();
		}

		internal void UpdateClientsList()
		{
			ClientsVMList = new ObservableCollection<ClientVM>(
				ClientsRepository.GetClientsList().Select(ConvertToClientVM));
			ClientsVMList.CollectionChanged += ClientsList_CollectionChanged;
		}

		public ObservableCollection<ClientVM> ClientsVMList { get; set; }
		void ClientsList_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == Add && e.NewItems is not null)
				foreach (ClientVM item in e.NewItems)
					item.Changer = Changer;
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		void NotifyPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		RelayCommand? showAccountsCommand;
		public RelayCommand ShowAccountsCommand =>
			showAccountsCommand ??= new RelayCommand(ShowAccounts);
		void ShowAccounts(object commandParameter)
		{
			// для передачи имени пользователя в ClientAccountsVM для ведения журнала
			ClientAccountsVM.Changer = Changer;
			ClientAccountsVM.Client = ConvertToClient(SelectedClientVM);
			new AccountsWindow() { DataContext = ClientAccountsVM }.ShowDialog();
		}

		RelayCommand? saveCommand;
		public RelayCommand SaveCommand => saveCommand ??= new RelayCommand(Save);
		void Save(object commandParameter) // сохраняет изменения или в случае некорректного ввода обрабатывает возникшее исключение
		{
			try
			{
				var clientsList = ClientsVMList.Select(ConvertToClient).ToList();
				ClientsRepository.Save(clientsList);
			}
			catch (ClientValidationException clientException)
			{
				MessageBox.Show(clientException.Message, "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
			}

		}

		RelayCommand? closeCommand;
		public RelayCommand CloseCommand => closeCommand ??= new RelayCommand(Close);
		void Close(object commandParameter)
		{
			if (commandParameter is Window clientsWindow) clientsWindow.Close();
		}

		ClientVM ConvertToClientVM(Client client) => new ClientVM()
			{
				Changer = Changer,
				Id = client.Id,
				LastName = client.LastName,
				FirstName = client.FirstName,
				MiddleName = client.MiddleName,
				PhoneNumber = client.PhoneNumber,
				Passport = client.Passport,
				ChangingTime = client.ChangingTime,
				ChangedData = client.ChangedData,
				ChangingType = client.ChangingType,
				LastChanger = client.LastChanger
			};		

		Client ConvertToClient(ClientVM clientVM) // при некорректном вводе данных клиента бросает исключение
		{
			if (clientVM.LastName is null || clientVM.FirstName is null)
				throw new ClientValidationException("Введите фамилию и имя клиента");
			if (clientVM.LastName.Length < 2)
				throw new ClientValidationException("Фамилия клиента менее 2 символов");
			if (!ContainsOnlyLetters(clientVM.LastName))
				throw new ClientValidationException("Фамилия клиента содержит некорректные символы");
			if (!ContainsOnlyLetters(clientVM.FirstName))
				throw new ClientValidationException("Имя клиента содержит некорректные символы");
			if (clientVM.MiddleName is not null && !ContainsOnlyLetters(clientVM.MiddleName))
				throw new ClientValidationException("Отчество клиента содержит некорректные символы");
			if (!ContainsOnlyDigits(clientVM.PhoneNumber))
				throw new ClientValidationException("Номер телефона содержит символы, отличные от цифр");
			if (clientVM.PhoneNumber.Length < 7 || clientVM.PhoneNumber.Length > 11)
				throw new ClientValidationException("Номер телефона должен содержать от 7 до 11 цифр");
			return new Client(
				clientVM.Id,
				clientVM.LastName,
				clientVM.FirstName,
				clientVM.MiddleName,
				clientVM.PhoneNumber,
				clientVM.Passport,
				clientVM.ChangingTime,
				clientVM.ChangedData,
				clientVM.ChangingType,
				clientVM.LastChanger);
		}

		bool ContainsOnlyLetters(string name)
		{
			foreach (char symbol in name)
			{
				if (!char.IsLetter(symbol)) return false;
			}
			return true;
		}

		bool ContainsOnlyDigits(string number)
		{
			foreach (char symbol in number)
			{
				if (!char.IsDigit(symbol)) return false;
			}
			return true;
		}
	}
}
