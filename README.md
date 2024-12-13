# ClientAccounts

Прототип банковской системы, которая содержит информацию о клиентах и их счетах.
- Предусмотрена возможность управлять счетами (открывать, закрывать счета, осуществлять переводы со счета на счет). 
- Предусмотрена работа в системе разных пользователей: менеджера и консультанта с разными разрешенными операциями.
- Репозиторий для хранения данных клиентов выделен в отдельную библиотеку, подключаемую к проекту. 
- Реализовано логгирование в txt-файл операций со счетами. 

Проект выполнен на платформе .NET и WPF с применением интерфейса INotifyPropertyChanged и шаблона mvvm.
Для логгирования операций со счетами - nuget-пакет NLog, с применением методов объекта Logger, для конфигурирования файл Nlog.config.
https://github.com/NLog/NLog/wiki/Tutorial  


При запуске WPF-приложения открывается окно UserSelectionWindow, выбираем пользователя (менеджер или консультант). Консультант может только просматривать данные клиента и менять номер телефона, менеджер - может изменять остальные данные и добавлять нового клиента.

Открывается окно с таблицей клиентских данных ClientsWindow с кнопками:
- Показать вклады, 
- Сохранить,
- Закрыть окно.

При клике на Показать вклады - AccountsWindow с таблицей счетов для выбранного клиента (Id, тип, сумма), кнопки:
- Открыть новый вклад (OpeningAccountWindow),
- Пополнить вклад (AccountReplenishingWindow), 
- Снять частично сумму с вклада (WithdraingWindow),
- Выполнить перевод между своими счетами (TransactionToOwnAccountWindow),
- Выполнить перевод на счет другого клиента (TransactionToAnotherAccountWindow),
- Закрыть вклад.

Для запуска приложения в App.xaml установлен обработчик для события Startup="OnStartup".
В OnStartup-обработчике App.xaml.cs открывается ПЕРВОЕ ОКНО UserSelectionWindow с DataContext = userSelectionVM.
В App.xaml.cs также инициализируются private поля для репозиториев клиентов, счетов с рандомной генерацией списков.

Реализован механизм Dependency Injection внедрения зависимости.
- В App.xaml.cs-классе выполняется конфигурация сервисов в static ConfigureServices()-методе с помощью AddSingleton-методов (с регистрацией конкретных классов-реализаций интерфейсов и VM-классов подложек для всех основных окон). 
- Метод конфигурации сервисов возвращает объект ServiceCollection.
- Из полученной коллекции сервисов получаем IServiceProvider serviceProvider = ConfigureServices().BuildServiceProvider() - провайдер сервиса, который позволяет получить в любом месте нужный сервис методом GetService<(TService)>(), например, VM-класс - как подложку для соответствующего окна.

Взаимодействие пользователя с приложением осуществляется:
- через события и обработчики событий, 
- через команды, для этого создан RelayCommand-класс, реализующий ICommand (реализует методы bool CanExecute, void Execute и событие EventHandler CanExecuteChanged).

В учебных целях создан метод расширения для Account-класса, т.е. можно использовать так: 
Account.GetAccountInfo() с получ. string информации о счете.

Основные классы:
- App.xaml.cs - для конфигурации и запуска первого окна UserSelectionWindow,
- UserSelectionVM - DataContext окна выбора пользователя (менеджер или консультант),
- ClientsInfoVM - DataContext окна с таблицей клиентов и журналом изменений (кто изменил, дата, тип изменения, измененные данные),
- ClientAccountsVM - DataContext окна счетов (выбранного клиента) с таблицей номер счета - сумма на счете.

Команды для выполнения операций со счетами клиентов работают по принципу:
- открытие соответствующего окна, 
- сбор данных с формы, 
- вызов обновления списка счетов, отображаемых в окне UpdateDisplayedAccountsList(),
- выброс MessageBox с сообщением об операции, 
- изменение репозитория счетов AccountsRepository.ChangeRepository(),
- обновление списка счетов для автоматического отображения изменений в окне UpdateDisplayedAccountsList,
- вызов события изменения счета AccountEvent?.Invoke(),
- логгирование соответствующего сообщения в файл Logger.Info(), имя txt-файла прописываем в Nlog.config.