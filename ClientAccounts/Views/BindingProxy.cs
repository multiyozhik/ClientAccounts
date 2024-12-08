using System.Windows;

namespace ClientAccounts.Views
{
	// для работы Binding в условиях mvvm (привязки списка клиентов из VM) используем Freezable класс
	// https://thomaslevesque.com/2011/03/21/wpf-how-to-bind-to-data-when-the-datacontext-is-not-inherited/

	public class BindingProxy : Freezable 
	{
		// DependencyProperty.Register метод регистрирует свойство зависимостей
		// с указанием имени свойства (Data), типа свойства (object) и типа владельца (BindingProxy)
		public static readonly DependencyProperty DataProperty =
		   DependencyProperty.Register("Data", typeof(object),
			  typeof(BindingProxy));

		// в XAML в <Window.Resources> используем Data dependency property		
		public object Data
		{
			get { return GetValue(DataProperty); }
			set { SetValue(DataProperty, value); }
		}

		#region Overrides of Freezable

		// если Freezable.CreateInstanceCore метод реализуется в производном классе,
		// создает новый экземпляр производного класса
		protected override Freezable CreateInstanceCore()
		{
			return new BindingProxy();
		}

		#endregion
	}
}
