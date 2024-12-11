using System.Windows;

namespace ClientAccounts.Views
{
    //Класс для диалогового окна WithdrawingWindow - пополнение счета
    public partial class AccountReplenishingWindow : Window
	{
		public AccountReplenishingWindow()
		{
			InitializeComponent();
		}

		private void Replenish_Button(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();
		}
	}
}
