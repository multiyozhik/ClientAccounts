using System;

namespace ClientAccounts.ViewModels
{
	[Serializable]
	internal class ClientValidationException : Exception
	{	
		public ClientValidationException(string? message) : base(message)
		{
		}	
	}
}