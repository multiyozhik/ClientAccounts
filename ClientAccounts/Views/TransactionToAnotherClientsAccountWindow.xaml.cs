using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClientAccounts.Views
{
	/// <summary>
	/// Логика взаимодействия для TransactionToAnotherClientsAccountWindow.xaml
	/// </summary>
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
