using ClientAccounts.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClientAccounts.Services
{
	class AccountsRepository : IAccountRepository
	{
		List<Account>? AccountsList { get; set; } 

		static readonly int[] accountsPeriods = new int[] { 3, 6, 12, 24 };
		static readonly double[] ratesList = new double[] { 3, 4, 5, 6 };		

		static AccountsRepository? accountsRepository;

		public static AccountsRepository BuildAccountsRepository(List<Guid> ClientsIDList) // метод возвращает сам класс с сгенерированным списком счетов
		{
			if (accountsRepository is null)
			{
				var accountsList = new List<Account>();
				Random randomNumber = new Random();
				foreach (Guid clientID in ClientsIDList)
				{
					for (int i = 0; i < randomNumber.Next(2, 4); i++) // 2-4 счета включительно у клиента					
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
		public void AddToRepository(Account account) => AccountsList?.Add(account);
		public void RemoveFromRepository(Account account) => AccountsList?.Remove(account);
		public void ChangeRepository(Account account, double summa)			
		{
			int index = AccountsList.FindIndex(x => x.AccountID == account.AccountID);
			AccountsList[index].CurrentSum = account.CurrentSum + summa;
		}
		public ICollection<Account> GetAccountsList() => AccountsList;
		public void Save(ICollection<Account> accountsList) => AccountsList = accountsList.ToList();
	}
}
