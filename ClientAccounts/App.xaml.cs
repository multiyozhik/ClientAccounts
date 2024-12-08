using ClientAccounts.Models;
using ClientAccounts.Services;
using ClientAccounts.ViewModels;
using ClientAccounts.Views;
using ClientsRepositoryLib;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Linq;
using System.Windows;

namespace ClientAccounts
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		readonly IServiceProvider serviceProvider = ConfigureServices().BuildServiceProvider();

		private static IServiceCollection ConfigureServices()
		{
			var clientsRepository = new ClientsRepository();
			var clientsList = clientsRepository.GetClientsList();
			var clientsIDList = clientsList.Select(client => client.Id).ToList();
			var accountsRepository = AccountsRepository.BuildAccountsRepository(clientsIDList);

			// паттерн Builder
			var services = new ServiceCollection()
				.AddSingleton<IClientsRepository>(clientsRepository)
				.AddSingleton<IAccountRepository>(accountsRepository)
				.AddSingleton<UserSelectionVM>()
				.AddSingleton<ClientsInfoVM>()
				.AddSingleton<ClientAccountsVM>()				
			;
			return services;
		}

		// для запуска первого окна приложения
		private void OnStartup(object sender, StartupEventArgs e)
		{
			var userSelectionVM = serviceProvider.GetService<UserSelectionVM>();
			new UserSelectionWindow { DataContext = userSelectionVM }.Show();
		}
	}
}
