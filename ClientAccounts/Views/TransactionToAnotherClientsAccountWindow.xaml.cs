using System.Windows;

namespace ClientAccounts.Views
{
    //Класс для диалогового окна TransactionToAnotherClientsAccountWindow - перевод на счет другого клиента
    public partial class TransactionToAnotherClientsAccountWindow : Window
	{
		public TransactionToAnotherClientsAccountWindow()
		{
			InitializeComponent();
		}

		private void Transact_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();
		}
	}
}
