namespace ClientsRepositoryLib
{
	public class ClientsRepository : IClientsRepository
	{
		List<Client> ClientsList { get; set; } = GetClients(); // закрытое с-во инициализируем сгенерированной базой клиентов
		static string[] lastNames = new string[] { "Иванов", "Сидоров", "Ли", "Хренов" };
		static string[] firstNames = new string[] { "Нео", "Евгений", "Матвей", "Мустафа" };
		static string[] middleNames = new string[] { "Сергеевич", "Данилович", "Борисович" };

		static List<Client> GetClients()
		{
			var listClients = new List<Client>();
			for (int i = 0; i < 10; i++)                // 10 клиентов
				listClients.Add(
					 new Client
					 (Guid.NewGuid(),
					 lastNames[new Random().Next(lastNames.Length)],
					 firstNames[new Random().Next(firstNames.Length)],
					 middleNames[new Random().Next(middleNames.Length)],
					 new Random().Next(1000000, 9000000).ToString(),
					 new Random().Next(100000, 900000).ToString(),
					 DateTime.Now, "", "", ""));
			return listClients;
		}
		public ICollection<Client> GetClientsList() => ClientsList;
		public void Save(ICollection<Client> clients) => ClientsList = clients.ToList();

		// получаем список Id клиентов для привязки к таблице счетов клиентов (доступ по Id)		
		public List<Guid> GetClientsIDList() => ClientsList.Select(Client => Client.Id).ToList();
	}
}