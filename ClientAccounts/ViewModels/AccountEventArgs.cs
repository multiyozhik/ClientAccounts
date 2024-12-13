using ClientAccounts.Models;
using System;

namespace ClientAccounts.ViewModels
{
	//Класс аргументов события изменения счета AccountEvent
	//в ClientAccountsVM.cs определялось типизир. событие public event EventHandler<AccountEventArgs> AccountEvent;
	internal class AccountEventArgs:EventArgs
	{
		public Account Account { get; }
		public string Message { get; }
		public AccountEventArgs(Account account, string message)
		{
			Account = account;
			Message = message;
		}
	}
}