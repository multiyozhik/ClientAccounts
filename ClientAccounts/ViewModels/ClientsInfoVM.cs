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
    /// <summary>
    /// Класс ClientsInfoVM - DataContext для ClientsWindow, кот. открывается из UserSelectionVM.
	/// Реализуем INotifyPropertyChanged для того, чтобы все привязанные свойства автомат. обновлялись в окнах при их изменении.
	/// В сеттерах свойств вызываем событие PropertyChanged, что свойство изменилось. 
	/// 
	/// В конструктор передаем репозиторий клиентов и счетов.
	/// Кроме того, определены свойства выбранного клиента, Changer (Manager или Consultant), 
	/// ObservableCollection<ClientVM> ClientsVMList
    /// </summary>
    class ClientsInfoVM : INotifyPropertyChanged
	{
        public IUserType? Changer { get; internal set; }

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
		IClientsRepository ClientsRepository { get; }
        public ClientAccountsVM ClientAccountsVM { get; }
        public ClientsInfoVM(IClientsRepository clientsRepository, ClientAccountsVM clientAccountsVM)
		{
			ClientsRepository = clientsRepository;
			ClientAccountsVM = clientAccountsVM;
			UpdateClientsList();
		}

        public void UpdateClientsList()
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

        //команда Показать счета (открыть окно со счетами и пробросить дальше значение Changer и выбранного Client)
        RelayCommand? showAccountsCommand;
		public RelayCommand ShowAccountsCommand => 
			showAccountsCommand ??= new RelayCommand(ShowAccounts);
		void ShowAccounts(object commandParameter)
		{
			ClientAccountsVM.Changer = Changer;
			ClientAccountsVM.Client = ConvertToClient(SelectedClientVM); 
			new AccountsWindow() { DataContext = ClientAccountsVM }.ShowDialog();
		}

        //команда Сохранить изменения списка клиентов и репозитория
        RelayCommand? saveCommand;
		public RelayCommand SaveCommand => saveCommand ??= new RelayCommand(Save);
		void Save(object commandParameter) 
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

		//команда закрытия окна клиентов
		RelayCommand? closeCommand;
		public RelayCommand CloseCommand => closeCommand ??= new RelayCommand(Close);
		void Close(object commandParameter)
		{
			if (commandParameter is Window clientsWindow) clientsWindow.Close();
		}

        //метод преобраз. объектов Client в ClientVM - нужен в конструкторе для UpdateClientsList() обновл. списка для окна 
        ClientVM ConvertToClientVM(Client client) => new()
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

        //метод нужен в команде ShowAccountsCommand при преобразовании строки выбранного клиента из таблицы в Client
        Client ConvertToClient(ClientVM clientVM) 
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

        static bool ContainsOnlyLetters(string name)
		{
			foreach (char symbol in name)
			{
				if (!char.IsLetter(symbol)) return false;
			}
			return true;
		}

        static bool ContainsOnlyDigits(string number)
		{
			foreach (char symbol in number)
			{
				if (!char.IsDigit(symbol)) return false;
			}
			return true;
		}
	}
}
