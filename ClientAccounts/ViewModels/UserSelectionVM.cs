using ClientAccounts.Models;
using ClientAccounts.Views;

namespace ClientAccounts.ViewModels
{
    /// <summary>
    /// Класс UserSelectionVM - это DataContext для окна выбора пользователя UserSelectionWindow (менеджер, консультант). 
	/// В конструктор передается объект ClientsInfoVM.
	/// При выделении SelectedUser в Combobox-элементе открывается ClientsWindow 
	/// (при этом обновляем лист клиентов и устанавливаем Changer, какой пользователь делает изменения).
    /// </summary>
    class UserSelectionVM
	{
		public IUserType[] UserTypes { get; } = new IUserType[] { new Consultant(), new Manager() };

		ClientsInfoVM ClientsInfoVM { get; }
        public IUserType SelectedUser
        {
            set
            {
                ClientsInfoVM.Changer = value;
                ClientsInfoVM.UpdateClientsList();
                new ClientsWindow() { DataContext = ClientsInfoVM }.ShowDialog();
            }
        }

        public UserSelectionVM(ClientsInfoVM clientsInfoVM)
		{
			ClientsInfoVM = clientsInfoVM;
		}
	}
}
