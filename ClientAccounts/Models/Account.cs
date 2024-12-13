using System;

namespace ClientAccounts.Models
{
    // Класс для счета (ID счета, ID владельца, тип счета, срок (в месяцах), ставка %, сумма вклада).
    // Реализована перегрузка операторов == и != для сравнения счетов с override Equals и GetHashCode-методов.

    class Account
	{
		public Guid OwnerID { get; set; } 
		public Guid AccountID { get; set; } 
		public AccountType Type { get; set; } 		
		public int AccountPeriod { get; set; } 		
		public double Rate { get; set; }  				
		public double CurrentSum { get; set; } 
		public override string ToString() => $"Счет {AccountID}";

		public static bool operator ==(Account account1, Account account2) =>
			account1?.AccountID == account2?.AccountID;
		public static bool operator !=(Account account1, Account account2) 
			=> !(account1 == account2);
		public override bool Equals(object? obj)
		{
			if (obj is Account account)
				return AccountID == account?.AccountID;
			return false;
		}
		public override int GetHashCode() => AccountID.GetHashCode();
	}

	/// <summary>
	/// Тип счета - накопительный счет или классический депозит
	/// </summary>
	enum AccountType
	{
		SavingAccount,
		Deposit
	}
}
