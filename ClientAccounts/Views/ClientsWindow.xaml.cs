using System;
using System.Windows;
using System.Windows.Controls;
using ClientAccounts.ViewModels;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;
using NLog;

namespace ClientAccounts.Views
{
	/// <summary>
	/// Логика взаимодействия для ClientsWindow.xaml
	/// </summary>
	public partial class ClientsWindow : Window
	{
		public ClientsWindow()
		{
			InitializeComponent();
			this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(OnErrorEvent));
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
			// связывается событие изменения счета с обработчиком
			clientsInfoVM.ClientAccountsVM.AccountEvent += ClientAccountsVM_AccountEvent;			
			// передаем наличие изменений клинтов в сообщение для журнала логирования
			Logger logger = LogManager.GetCurrentClassLogger();
			logger.Log(LogLevel.Info, $"Изменение данных клиента. Пользователь {clientsInfoVM.Changer}");
		}

		// обработчик при вызове события изменения счета - вызвать всплывающее окно Popup
		private void ClientAccountsVM_AccountEvent(object? sender, AccountEventArgs e)
		{
			Popup popup = new Popup();
			popup.Width = 300;
			popup.Height = 200;
			popup.PlacementTarget = ClientsData; // расположение относительно целевого элемента
			popup.Placement = PlacementMode.Bottom;

			TextBlock popupText = new TextBlock();
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


