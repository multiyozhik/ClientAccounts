using ClientAccounts.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClientAccounts.Services
{
    // Класс репозитория для хранения счетов.

	// Имеется private AccountsList (извне изм. можно только через методы).
	// Репозиторий счетов создается однократно с пом. static BuildAccountsRepository()-метода, 
	// кот. по списку Id клиентов инициализ. и возвр. static поле accountsRepository 
	// с сгенерир. списком счетов (2-4 счета включительно для каждого клиента)

    class AccountsRepository : IAccountRepository
	{
		List<Account>? AccountsList { get; set; } 

		static readonly int[] accountsPeriods = [3, 6, 12, 24];
		static readonly double[] ratesList = [3, 4, 5, 6];		

		static AccountsRepository? accountsRepository;

		public static AccountsRepository BuildAccountsRepository(List<Guid> ClientsIDList)
		{
			if (accountsRepository is null)
			{
				var accountsList = new List<Account>();
				var randomNumber = new Random();
				foreach (Guid clientID in ClientsIDList)
				{
					for (int i = 0; i < randomNumber.Next(2, 4); i++) 					
						accountsList.Add(new Account()
						{
							OwnerID = clientID,
							AccountID = Guid.NewGuid(),
							Type = (AccountType)randomNumber.Next(Enum.GetValues<AccountType>().Length),
							AccountPeriod = accountsPeriods[randomNumber.Next(accountsPeriods.Length - 1)],
							Rate = ratesList[randomNumber.Next(ratesList.Length - 1)],
							CurrentSum = randomNumber.Next(50000, 2000000)
						});
				}
				accountsRepository = new AccountsRepository() { AccountsList = accountsList };
			}
			return accountsRepository;
		}

        public ICollection<Account> GetAccountsList() => AccountsList;
        public void AddToRepository(Account account) => AccountsList?.Add(account);
		public void RemoveFromRepository(Account account) => AccountsList?.Remove(account);
		public void ChangeRepository(Account account, double summa)			
		{
			if (AccountsList is null || AccountsList.Count == 0) return;
			int index = AccountsList.FindIndex(x => x.AccountID == account.AccountID);
			AccountsList[index].CurrentSum = account.CurrentSum + summa;
		}
		public void Save(ICollection<Account> accountsList) => AccountsList = accountsList.ToList();
	}
}
