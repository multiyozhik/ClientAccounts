using ClientAccounts.Models;
using System;

namespace ClientAccounts.ViewModels
{
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