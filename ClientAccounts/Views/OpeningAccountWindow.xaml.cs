using System.Windows;

namespace ClientAccounts.Views
{
    //Класс для диалогового окна OpeningAccountWindow - открытие нового счета
    public partial class OpeningAccountWindow : Window
	{
		public OpeningAccountWindow()
		{
			InitializeComponent();
		}
		
		private void Open_Button(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();
		}
	}
}
