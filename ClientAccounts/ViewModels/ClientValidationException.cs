﻿using System;

namespace ClientAccounts.ViewModels
{
    // Класс исключения при ошибках ввода данных клиента (как пример создания собств. типа искл.)
	// [Serializable] атрибут ничего не делает, но типа метка, ярлык, что класс м.б. сериализ. class Exception : ISerializable

    [Serializable]
	internal class ClientValidationException : Exception
	{	
		public ClientValidationException(string? message) : base(message)
		{
		}	
	}
}