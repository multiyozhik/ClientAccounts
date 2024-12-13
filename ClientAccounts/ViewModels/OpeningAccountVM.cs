using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
namespace ClientAccounts.ViewModels
{
    // Класс OpeningAccountVM - DataContext для окна OpeningAccountWindow открытия счета (открыв. по команде из AccountsWindow).
	// В конструктор OpeningAccountVM передается владелец счета OwnerID.
	// С окна собираются данные: в RadioButton тип счета IsDeposit, в ComboBox на сколько месяцев счет, 
	// в зав-ти от периода - захардкодена ставка %, в TextBox сумма счета.
	// Из собранных с формы данных будет сформирован объект Account, кот. будет доб. в репозиторий счетов.

    class OpeningAccountVM : INotifyPropertyChanged
	{
		public Guid OwnerID { get; set; } 		
		public bool IsDeposit { get; set; }
		Dictionary<int, double> RatesDictionary { get; } =
			new Dictionary<int, double> { { 3, 6 }, { 6, 7 }, { 12, 8 }, { 18, 7 } };
		public int[] AccountPeriodsList { get; } = new int[] { 3, 6, 12, 18 }; 

		int accountPeriod;
		public int AccountPeriod
		{
			get => accountPeriod;
			set
			{
				if (RatesDictionary.TryGetValue(value, out var rate))
				{
					accountPeriod = value;
					Rate = rate;
				}
				else
					MessageBox.Show("Ставка счета не определена");
			}
		}

		double rate;
		public double Rate
		{
			get => rate;
			private set
			{
				rate = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rate)));
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public double CurrentSum { get; set; }

		public OpeningAccountVM(Guid ownerID)
		{
			OwnerID = ownerID;
		}
	}
}