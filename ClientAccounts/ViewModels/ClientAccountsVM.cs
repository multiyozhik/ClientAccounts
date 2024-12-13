using ClientAccounts.Models;
using ClientAccounts.Services;
using ClientAccounts.Views;
using ClientsRepositoryLib;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ClientAccounts.ViewModels
{
	//Класс ClientAccountsVM - DataContext окна AccountsWindow
	//в конструктор передаем репозиторий счетов клиентов, а остал. с-ва
	//DisplayedAccountsList, Client (выбран. клиент), Changer (тип польз-ля) инициализ. при откр. AccountsWindow

	//Для логгирования операций со счетами - уст. nuget-пакет NLog и можем работать с объектом Logger 
	//https://github.com/NLog/NLog/wiki/Tutorial для конфигурирования файл Nlog.config 

	//Команды работают по принципу - откр. соотв. окно, сбор данных с формы, вызов UpdateDisplayedAccountsList(),
	//MessageBox с сообщением об операции, AccountsRepository.ChangeRepository() и UpdateDisplayedAccountsList,
	//вызов события изм. счета AccountEvent?.Invoke() и логгир. в файл Logger.Info(сообщение)
	class ClientAccountsVM : INotifyPropertyChanged
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		public string? Info => SelectedAccount?.GetAccountInfo(); //вывод инфо о счете через метод расширения

		public IUserType? Changer;

		Client? client;
		public Client? Client
		{
			get => client;
			set
			{
				if (client != value)
				{
					client = value;
					UpdateDisplayedAccountsList();
					NotifyPropertyChanged(nameof(Client));
				}
			}
		}
		void UpdateDisplayedAccountsList()
		{
			DisplayedAccountsList = AccountsRepository.GetAccountsList()
				.Where(account => account.OwnerID == Client.Id)
				.ToList();
		}


		List<Account>? displayedAccountsList;
		public List<Account>? DisplayedAccountsList
		{
			get => displayedAccountsList;
			set
			{
				if (displayedAccountsList != value)
				{
					displayedAccountsList = value;
					NotifyPropertyChanged(nameof(DisplayedAccountsList));
				}
			}
		}

		public Account? SelectedAccount
		{
			get => selectedAccount;
			set
			{
				selectedAccount = value;
				NotifyPropertyChanged(nameof(Info));
			}
		}

		//вспомог. метод, чтобы в сеттере свойств не прописывать вручную имена свойств при вызове PropertyChanged
		void NotifyPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler? PropertyChanged;
		public event EventHandler<AccountEventArgs> AccountEvent; //событие при изм. счета типизируем для возвр. аргум.



		//в конструктор передаем репозиторий счетов клиентов
		IAccountRepository AccountsRepository { get; }
		public ClientAccountsVM(IAccountRepository accountsRepository)
		{
			AccountsRepository = accountsRepository;
		}	


		//Команда открыть новый счет

		RelayCommand? openNewAccountCommand;
		public ICommand OpenNewAccountCommand => openNewAccountCommand ??= new RelayCommand(OpenNewAccount);
		void OpenNewAccount(object commandParameter)
		{
			if (client is null) return;
			var openingAccountVM = new OpeningAccountVM(client.Id);
			var openingAccountWindow = new OpeningAccountWindow() { DataContext = openingAccountVM };
			var dialogResult = openingAccountWindow.ShowDialog();
			if (dialogResult != true) return;
			var newAccount = new Account()
			{
				OwnerID = client.Id,
				AccountID = Guid.NewGuid(),
				Type = openingAccountVM.IsDeposit ? AccountType.Deposit : AccountType.SavingAccount,
				AccountPeriod = openingAccountVM.AccountPeriod,
				Rate = openingAccountVM.Rate,
				CurrentSum = openingAccountVM.CurrentSum
			};
			AccountsRepository.AddToRepository(newAccount);
			MessageBox.Show(
				 $"Открыт счет {newAccount.AccountID} клиента: " +
				 $"{client.LastName} {client.FirstName} {client.MiddleName}. " +
				 $"Сумма на счете {newAccount.CurrentSum} руб.",
				 "Сообщение об открытии счета",
				 MessageBoxButton.OK,
				 MessageBoxImage.Information);
			UpdateDisplayedAccountsList(); //чтобы сработал сеттер DisplayedAccountsList, PropertyChanged и обнов. окно 

			AccountEvent?.Invoke(
				this, new AccountEventArgs(newAccount, $"Открыт счет {newAccount} " +
					$"для клиента: {Client?.LastName} " +
					$"{DateTime.Now}"));

			Logger.Info($"Открыт счет. Пользователь {Changer}");
		}


		//Команда пополнить счет

		RelayCommand? replenishCommand;     
		public RelayCommand ReplenishCommand => replenishCommand ??= new RelayCommand(Replenish);
		void Replenish(object commandParameter)
		{
			if (SelectedAccount is not null)
			{
				var accountsReplenishingVM = new AccountsReplenishingVM();
				var accountReplenishingWindow = new AccountReplenishingWindow() { DataContext = accountsReplenishingVM };
				var dialogResult = accountReplenishingWindow.ShowDialog();
				if (dialogResult != true) return;
				AccountsRepository.ChangeRepository(SelectedAccount, accountsReplenishingVM.AddingSum);
				MessageBox.Show(
					 $"Выполнено пополнение счета на {accountsReplenishingVM.AddingSum} руб. " +
					 $"На вашем счете {SelectedAccount.CurrentSum} руб.",
					 "Сообщение о пополнении счета",
					 MessageBoxButton.OK,
					 MessageBoxImage.Information);
				UpdateDisplayedAccountsList();

				AccountEvent?.Invoke(
				this, new AccountEventArgs(SelectedAccount, $"Пополнен счет {SelectedAccount} для клиента: {Client?.LastName}"));
				Logger.Info($"Пополнен счет. Пользователь {Changer}");
			}
		}


		//Команда снять чаасть средств со счета

		RelayCommand? withdrawCommand;
		public RelayCommand WithdrawCommand => withdrawCommand ??= new RelayCommand(Withdraw);
		void Withdraw(object commandParameter)
		{
			if (SelectedAccount is not null)
			{
				var accountsWithdrawingVM = new AccountsWithdrawingVM();
				var withdrawingWindow = new WithdrawingWindow() { DataContext = accountsWithdrawingVM };
				var dialogResult = withdrawingWindow.ShowDialog();

				if (dialogResult != true) return;
				AccountsRepository.ChangeRepository(SelectedAccount, -accountsWithdrawingVM.SubstructingSum); //снять => минусуем																										  							

				MessageBox.Show(
					$"Со счета снято {accountsWithdrawingVM.SubstructingSum} руб. " +
					$"На вашем счете осталось {SelectedAccount.CurrentSum} руб.",
					"Сообщение о частичном снятии средств со счета",
					MessageBoxButton.OK,
					MessageBoxImage.Information);
				UpdateDisplayedAccountsList();

				AccountEvent?.Invoke(
				this, new AccountEventArgs(SelectedAccount, $"Выполнен перевод со счета {SelectedAccount} для клиента: {Client?.LastName}"));
				Logger.Info($"Сняты средства со счета. Пользователь {Changer}");
			}
		}


		//Команда перевод между своими счетами

		RelayCommand? transactToOwnAccountCommand;       
		public ICommand TransactToOwnAccountCommand => transactToOwnAccountCommand ??= new RelayCommand(TransactToOwnAccount);
		void TransactToOwnAccount(object commandParameter)
		{
			if (SelectedAccount is null) return;

			var transactionVM = new TransactionVM
			{
				AccountFrom = SelectedAccount,
				OwnerAccountsTo = DisplayedAccountsList
						.Where(account => account != SelectedAccount)
						.ToList()
			};

			var transactionToOwnAccountWindow = new TransactionToOwnAccountWindow() { DataContext = transactionVM };
			var dialogResult = transactionToOwnAccountWindow.ShowDialog();

			if (dialogResult != true) return;

			AccountsRepository.ChangeRepository(transactionVM.AccountFrom, -transactionVM.TransactionSum);

			AccountsRepository.ChangeRepository(transactionVM.AccountTo, transactionVM.TransactionSum);

			MessageBox.Show($"Выполнен перевод  {transactionVM.TransactionSum} руб. со счета {transactionVM.AccountFrom}" +
				$" на счет {transactionVM.AccountTo}", "Сообщение о переводе средств между счетами клиента",
					MessageBoxButton.OK,
					MessageBoxImage.Information);
			UpdateDisplayedAccountsList();

			AccountEvent?.Invoke(
				this, new AccountEventArgs(SelectedAccount, $"Выполнен перевод  {transactionVM.TransactionSum} руб. со счета {transactionVM.AccountFrom} " +
				$"на счет {transactionVM.AccountTo} для клиента: {Client?.LastName}"));
			Logger.Info($"Выполнен перевод на другой собственный счет. Пользователь {Changer}");
		}


		//Команда перевод на счет другому клиенту

		RelayCommand? transactToAnotherClientsAccountCommand;
		public ICommand TransactToAnotherClientsAccountCommand => transactToAnotherClientsAccountCommand ??= new RelayCommand(TransactToAnotherClientsAccount);
		void TransactToAnotherClientsAccount(object commandParameter)
		{
			if (SelectedAccount is null) return;

			var transactionVM = new TransactionVM
			{
				AccountFrom = SelectedAccount
			};

			var transactionToAnotherClientsAccountWindow = new TransactionToAnotherClientsAccountWindow() { DataContext = transactionVM };
			var dialogResult = transactionToAnotherClientsAccountWindow.ShowDialog();
			if (dialogResult is not true || transactionVM.AccountTo is null) return;

			var anotherClientAccountIdTo = transactionVM.AnotherClientAccountIdTo;

			transactionVM.AccountTo = AccountsRepository.GetAccountsList()
				.ToList()
				.FirstOrDefault(accountTo => accountTo.AccountID.ToString() == anotherClientAccountIdTo);

			AccountsRepository.ChangeRepository(transactionVM.AccountFrom, -transactionVM.TransactionSum);

			if (transactionVM.AccountTo != null)
				AccountsRepository.ChangeRepository(transactionVM.AccountTo, transactionVM.TransactionSum);

			MessageBox.Show($"Выполнен перевод  {transactionVM.TransactionSum} со счета {transactionVM.AccountFrom}" +
				$" на счет {transactionVM.AccountTo}", "Сообщение о переводе средств со счета клиента на счет другого клиента",
					MessageBoxButton.OK,
					MessageBoxImage.Information);
			UpdateDisplayedAccountsList();

			AccountEvent?.Invoke(
			this, new AccountEventArgs(SelectedAccount, $"Выполнен перевод  {transactionVM.TransactionSum} руб. " +
				$"со счета {transactionVM.AccountFrom} клиента: {Client?.LastName}" +
				$"на счет {transactionVM.AccountTo}"));
			Logger.Info($"Выполнен перевод на счет другого клиента. Пользователь {Changer}");
		}


		//Команда закрыть счет

		RelayCommand? closeAccountCommand;
		private Account? selectedAccount;

		public ICommand CloseAccountCommand => closeAccountCommand ??= new RelayCommand(CloseAccount);
		void CloseAccount(object commandParameter)
		{
			if (SelectedAccount is null) return;
			var messageBoxResult = MessageBox.Show(
					"Вы действительно хотите закрыть счет?",
					"Подтверждение закрытия счета",
					MessageBoxButton.YesNo,
					MessageBoxImage.Question);
			if (messageBoxResult == MessageBoxResult.Yes)
			{
				MessageBox.Show(
					 $"Счет {SelectedAccount.AccountID} клиента закрыт. " +
					 $"Снято {SelectedAccount.CurrentSum} тыс. руб.",
					 "Сообщение о закрытии счета",
					 MessageBoxButton.OK,
					 MessageBoxImage.Information);
				AccountsRepository.RemoveFromRepository(SelectedAccount);
				UpdateDisplayedAccountsList();

				Logger.Info($"Счет закрыт. Пользователь {Changer}");
			}
		}
	}
}

