using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientsRepositoryLib
{	
	/// <summary>
	/// Интерфейс репозитория для хранения данных клинета 
	/// (метод получения коллекции клиентов и метод для ее сохранения)
	/// </summary>
	public interface IClientsRepository
	{
		ICollection<Client> GetClientsList();
		void Save(ICollection<Client> clients);
	}
}
