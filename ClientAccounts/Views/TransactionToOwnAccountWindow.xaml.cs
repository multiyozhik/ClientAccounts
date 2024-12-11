using System.Windows;

namespace ClientAccounts.Views
{
    //Класс для диалогового окна TransactionToOwnAccountWindow - перевод между своими счетами
    public partial class TransactionToOwnAccountWindow : Window
	{
		public TransactionToOwnAccountWindow()
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
