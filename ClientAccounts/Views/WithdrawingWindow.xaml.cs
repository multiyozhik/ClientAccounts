using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ClientAccounts.ViewModels;


namespace ClientAccounts.Views
{
	/// <summary>
	/// Логика взаимодействия для WithdrawingWindow.xaml
	/// </summary>
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
