using System;

namespace ClientAccounts.Models
{
	class Account
	{
		public Guid OwnerID { get; set; }    // владелец вклада
		public Guid AccountID { get; set; } //  номер счета уникальный Guid
		public AccountType Type { get; set; }    // тип счета (накопительный счет / классический вклад)			
		public int AccountPeriod { get; set; } // на какой срок (в месяцах)			
		public double Rate { get; set; }        // процентная ставка (%)				
		public double CurrentSum { get; set; }  // сумма вклада
		public override string ToString() => $"Счет {AccountID}";


		// парная перегрузка оператора сравнения счетов
		public static bool operator ==(Account account1, Account account2) =>
			account1?.AccountID == account2?.AccountID;

		public static bool operator !=(Account account1, Account account2) => !(account1 == account2);

		public override bool Equals(object? obj)
		{
			if (obj is Account account)
				return AccountID == account?.AccountID;
			return false;
		}

		public override int GetHashCode() => AccountID.GetHashCode();
	}

	enum AccountType
	{
		SavingAccount,
		Deposit
	}
}
