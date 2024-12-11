using ClientAccounts.Models;
using System.Collections.Generic;

namespace ClientAccounts.Services
{
	/// <summary>
	/// Интерфейс репозитория для хранения счетов
	/// (методы получения списка счетов, доб. счета в репоз., изменения (суммы на счете), удаления счета, сохранения репоз.)
	/// </summary>
	internal interface IAccountRepository
	{
        ICollection<Account> GetAccountsList();
        void AddToRepository(Account newAccount);
		void ChangeRepository(Account selectedAccount, double addingSum);
		void RemoveFromRepository(Account selectedAccount);
		void Save(ICollection<Account> accountsList);
	}
}
