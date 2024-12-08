using ClientAccounts.ViewModels;
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
	/// Логика взаимодействия для AccountTypeSelectionWindow.xaml
	/// </summary>
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
