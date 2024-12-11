using System.Windows;


namespace ClientAccounts.Views
{
    //Класс для диалогового окна WithdrawingWindow - частичное снятие средств со счета
    public partial class WithdrawingWindow : Window
	{
		public WithdrawingWindow()
		{
			InitializeComponent();			
		}

		private void WithdrawButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();			
		}
	}
}
