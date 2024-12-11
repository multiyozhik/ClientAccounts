using ClientAccounts.Models;
using System.Collections.Generic;

namespace ClientAccounts.ViewModels
{
    //Класс DataContext для TransactionToOwnAccountWindow окна перевода между счетами
    internal class TransactionVM
	{
		public double TransactionSum { get; set; }
		public Account? AccountFrom { get; set; } 
		public Account? AccountTo { get; set; } 
		public List<Account>? OwnerAccountsTo { get; set; }
		public string AnotherClientAccountIdTo { get; set; }
	}
}
