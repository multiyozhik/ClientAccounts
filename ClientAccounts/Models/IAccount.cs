
namespace ClientAccounts.Models
{
	interface IAccount
	{		
		double GetCurrentSum(double summa);
		void Put(int summa);
		void Take(int Summa);
	}
}
