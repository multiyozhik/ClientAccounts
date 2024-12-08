using ClientAccounts.Models;
using ClientAccounts.Views;

namespace ClientAccounts.ViewModels
{
	class UserSelectionVM
	{
		// для Combobox список пользователей (менеджер или консультант) выдаем через Binding
		public IUserType[] UserTypes { get; } = new IUserType[] { new Consultant(), new Manager() };

		ClientsInfoVM ClientsInfoVM { get; }

		public UserSelectionVM(ClientsInfoVM clientsInfoVM)
		{
			ClientsInfoVM = clientsInfoVM;
		}

		public IUserType SelectedUser
		{								
			set
			{				
				ClientsInfoVM.Changer = value;
				ClientsInfoVM.UpdateClientsList();
				new ClientsWindow(){ DataContext = ClientsInfoVM }.ShowDialog();
			}
		}
	}
}
