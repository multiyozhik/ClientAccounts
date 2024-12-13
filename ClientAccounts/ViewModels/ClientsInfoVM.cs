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
	// Класс ClientsInfoVM - DataContext для ClientsWindow, кот. открывается из UserSelectionVM.
	// Реализуем INotifyPropertyChanged для того, чтобы все привязанные свойства автомат. обновл. в окнах при их изм.
	// В сеттерах свойств вызыв. событие PropertyChanged, что с-во изменилось. 

	// В конструктор передаем репозиторий клиентов и счетов.
	// Основные свойства - SelectedClientVM, Changer (Manager или Consultant), Observable ClientsVMList

	class ClientsInfoVM : INotifyPropertyChanged
	{
        public IUserType? Changer { get; internal set; }
		public bool IsReadOnly { get; set; }
		public bool IsCanAddNewClient { get; set; }

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


		//в констуктор передаем репозитории клиентов и их счетов
		IClientsRepository ClientsRepository { get; }
		public ClientAccountsVM ClientAccountsVM { get; }
        public ClientsInfoVM(IClientsRepository clientsRepository, ClientAccountsVM clientAccountsVM)
		{
			ClientsRepository = clientsRepository;
			ClientAccountsVM = clientAccountsVM;
			LoadClientsList();
		}
        public void LoadClientsList()
		{
			ClientsVMList = new ObservableCollection<ClientVM>(
				ClientsRepository.GetClientsList().Select(ConvertToClientVM));
			ClientsVMList.CollectionChanged += ClientsList_CollectionChanged; 
		}


		// ObservableCollection<ClientVM> ClientsVMList с подпиской обработчика для CollectionChanged-события,
		// при доб. новых клиентов нужно уст. для них значение Changer 
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
			if (clientVM.Id == System.Guid.Empty) //иначе при доб. менеджером нов.клиента Id = 0000-0000...
				clientVM.Id = System.Guid.NewGuid();
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
