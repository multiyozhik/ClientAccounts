using ClientAccounts.Models;
using ClientAccounts.Views;
using ClientsRepositoryLib;

namespace ClientAccounts.ViewModels
{
    // Класс UserSelectionVM - это DataContext для окна выбора пользователя UserSelectionWindow (менеджер, консультант). 
	// В конструктор передается объект ClientsInfoVM.
	// При выделении SelectedUser в Combobox-элементе открывается ClientsWindow 
	// (при этом обновляем лист клиентов и устанавливаем Changer, какой пользователь делает изменения).

    class UserSelectionVM
	{
		public IUserType[] UserTypes { get; } = [new Consultant(), new Manager()];

		ClientsInfoVM ClientsInfoVM { get; }

		IUserType selectedUser;
		public IUserType SelectedUser
        {
			get => selectedUser;
			set
			{
				selectedUser = value;

				ClientsInfoVM.Changer = selectedUser;
                ClientsInfoVM.LoadClientsList();

				ClientsInfoVM.IsReadOnly = value is not Manager;
				ClientsInfoVM.IsCanAddNewClient = value is Manager;

				new ClientsWindow() { DataContext = ClientsInfoVM }.ShowDialog();
            }
        }

        public UserSelectionVM(ClientsInfoVM clientsInfoVM)
		{
			ClientsInfoVM = clientsInfoVM;
		}
	}
}
