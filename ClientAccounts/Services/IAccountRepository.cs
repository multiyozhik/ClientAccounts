using ClientAccounts.Models;
using System.Collections.Generic;

namespace ClientAccounts.Services
{
	internal interface IAccountRepository
	{
		void AddToRepository(Account newAccount);
		void ChangeRepository(Account selectedAccount, double addingSum);
		ICollection<Account> GetAccountsList();
		void RemoveFromRepository(Account selectedAccount);
		void Save(ICollection<Account> accountsList);
	}
}
