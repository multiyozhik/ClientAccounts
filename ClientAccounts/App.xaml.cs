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
    /// Ключевой класс, в котором реализовано DI - конфигурация сервисов с получением коллекции сервисов и провайдера сервиса.
    /// С помощью провайдера получаем UserSelectionVM - DataContext для первого запускаемого окна UserSelectionWindow.
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

			var services = new ServiceCollection()
				.AddSingleton<IClientsRepository>(clientsRepository)
				.AddSingleton<IAccountRepository>(accountsRepository)
				.AddSingleton<UserSelectionVM>()
				.AddSingleton<ClientsInfoVM>()
				.AddSingleton<ClientAccountsVM>()				
			;
			return services;
		}

        //обработчик события Startup App.xaml и запуск первого окна UserSelectionWindow
        private void OnStartup(object sender, StartupEventArgs e)
		{
			var userSelectionVM = serviceProvider.GetService<UserSelectionVM>();
			new UserSelectionWindow { DataContext = userSelectionVM }.Show();
		}
	}
}
