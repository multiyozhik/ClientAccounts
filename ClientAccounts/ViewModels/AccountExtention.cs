using ClientAccounts.Models;

namespace ClientAccounts.ViewModels
{
    /// <summary>
    /// Класс для создания метода расширения для Account-класса
    /// т.е. можно использовать так: Account.GetAccountInfo() с получ. string информации о счете
    /// </summary>
    static class AccountExtention
	{
		public static string GetAccountInfo(this Account account) =>
			$"{ConvertAccountTypeToString(account.Type)} № {account.AccountID} с годовой ставкой {account.Rate} % сроком {account.AccountPeriod} мес.";
			
		static string ConvertAccountTypeToString(AccountType accountType)
		{
			if (accountType == AccountType.SavingAccount) return "накопительный счет";
			else return "классический депозит";
		}		
	}
}
