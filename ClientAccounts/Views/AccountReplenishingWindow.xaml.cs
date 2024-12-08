using ClientAccounts.Services;
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
	/// Логика взаимодействия для AccountReplenishingWindow.xaml
	/// </summary>
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
