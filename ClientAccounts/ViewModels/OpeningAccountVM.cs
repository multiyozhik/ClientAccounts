using ClientAccounts.Models;
using ClientAccounts.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace ClientAccounts.ViewModels
{
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