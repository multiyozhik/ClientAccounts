using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientsRepositoryLib
{	
	public interface IClientsRepository
	{
		ICollection<Client> GetClientsList();
		void Save(ICollection<Client> clients);
	}
}
