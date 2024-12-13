using ClientAccounts.ViewModels;
using NLog;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace ClientAccounts.Views
{
	// Класс для окна ClientsWindow.xaml
	/// </summary>
	public partial class ClientsWindow : Window
	{
		public ClientsWindow()
		{
			InitializeComponent();
			AddHandler(Validation.ErrorEvent, new RoutedEventHandler(OnErrorEvent));
		}
		// метод возвращает неактивное состояние кнопки сохранения списка клиентов при ошибках, неполных данных
		private void OnErrorEvent(object sender, RoutedEventArgs e)
		{
			bool IsErrors = false;
			var clientInfoVM = (ClientsInfoVM)DataContext;
			foreach (ClientVM clientVM in clientInfoVM.ClientsVMList)
			{
				if (!String.IsNullOrEmpty(clientVM.Error))
				{
					IsErrors = true;
					break;
				}
			}
			SaveButton.IsEnabled = !IsErrors;
		}

		// в  xaml для Window установлено событие DataContextChanged и определен его обработчик
		// для любого FrameworkElement (Window и т.п.) предусмотрено событие при изменении его контекста
		private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			var clientsInfoVM = (ClientsInfoVM)DataContext;
			clientsInfoVM.ClientAccountsVM.AccountEvent += ClientAccountsVM_AccountEvent;			
			Logger logger = LogManager.GetCurrentClassLogger();
			logger.Log(LogLevel.Info, $"Изменение данных клиента. Пользователь {clientsInfoVM.Changer}");
		}

		// обработчик при изменении счета - вызвать всплывающее окно Popup
		private void ClientAccountsVM_AccountEvent(object? sender, AccountEventArgs e)
		{
			Popup popup = new()
			{
				Width = 300,
				Height = 200,
				PlacementTarget = ClientsData, // расположение относительно целевого элемента
				Placement = PlacementMode.Bottom
			};

			TextBlock popupText = new();
			popupText.Text = e.Message;
			popupText.Background = Brushes.LightGray;
			popupText.Foreground = Brushes.Blue;
			popupText.TextWrapping = TextWrapping.Wrap;
			popup.Child = popupText;
			popup.IsOpen = true;

			var timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromSeconds(5d);
			timer.Tick += TimerTick;
			timer.Start();

			void TimerTick(object sender, EventArgs e)
			{
				var timer = (DispatcherTimer)sender;
				timer.Stop();
				timer.Tick -= TimerTick;
				popup.IsOpen = false;
			}
		}
	}
}


