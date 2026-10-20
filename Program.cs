using System.Text.Json;
namespace Library1
{
    class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public int ViewsCount { get; set; }

        // это для классов чтоб юзать их, есть вроде ещё способ но мне пох)
        public Book(int id, string title, string author, int year)
        {
            Id = id;
            Title = title;
            Author = author;
            Year = year;
        }
    }

    class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public User(int id, string name, string password)
        {
            Id = id;
            Name = name;
            Password = password;
        }
    }
    class LibraryManager
    {
        private List<Book> books = new List<Book>();
        private List<User> users = new List<User>();
        private string filePath = "books_data.json";
        private string UfilePath = "users_data.json";
        public void SaveData()
        {
            string jsonString = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(filePath, jsonString);
        }
        public void USaveData()
        {
            string UjsongString = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(UfilePath, UjsongString);
        }
        public void LoadData()
        {
            if (File.Exists(filePath))
            {

                string jsonString = File.ReadAllText(filePath);

                books = JsonSerializer.Deserialize<List<Book>>(jsonString);
            }
        }
        public void ULoadDota()
        {
            if (File.Exists(UfilePath))
            {
                string UjsongString = File.ReadAllText(UfilePath);
                users = JsonSerializer.Deserialize<List<User>>(UjsongString);
            }
        }

        public void AddBook() // добавить книгу, в целом изи
        {
            Console.WriteLine("Введите id книги:");
            int pID;
            if (!int.TryParse(Console.ReadLine(), out pID))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Произошла ошибка: ...");
                Console.ResetColor();
                Console.Clear();
                return;
            }
            bool idExists = books.Any(b => b.Id == pID);
            if (idExists)
            {
                Console.WriteLine("Ошибка: книга с таким ID уже существует в библиотеке");
                return;
            }
            Console.WriteLine("Введите название книги:");
            string Ptitle = Console.ReadLine();
            Console.WriteLine("Введите автора книги:");
            string pAuthor = Console.ReadLine();
            Console.WriteLine("Введите год выпуска:");
            int pYear = Convert.ToInt32(Console.ReadLine());
            Book book2 = new Book(pID, Ptitle, pAuthor, pYear);
            books.Add(book2);
            Console.WriteLine("Книга успешно добавлена");
        }
        public void RegisterUser()
        {
            int Id = 0;
            Console.Write("Введите ваше имя: ");
            string name = Console.ReadLine();
            bool NameExist = users.Any(b => b.Name == name);
            if (NameExist)
            {
                Console.WriteLine("Пользователь уже существует");
                return;
            }
            Console.Write("Введите ваш пароль: ");
            string password = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Пароль не может быть пустым!");
                return;
            }

            Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;// неслыханная имба
            User user = new User(Id, name, Convert.ToString(password));
            users.Add(user);
            Console.WriteLine("Регистрация успешна!");
        }
        public bool LoginUser()
        {
            Console.Write("Введите ваше имя: ");
            string name = Console.ReadLine();
            Console.Write("Введите ваш пароль: ");
            string password = Console.ReadLine();
            User element = users.FirstOrDefault(u => u.Name == name);
            if (element == null)
            {
                Console.WriteLine("Пользователь с таким именем не найден.");
                return false;
            }
            if (password != element.Password)
            {
                Console.WriteLine("Пароль неверный!");
                return false;
            }
            Console.WriteLine("Вход успешно выполнен!");
            return true;

        }
        public void ShowAllBooks() // показать все книги, изи вобще 
        {
            foreach (Book element in books)
            {
                Console.WriteLine($"Id книги: {element.Id}");
                Console.WriteLine($"Название: {element.Title}");
                Console.WriteLine($"Автор: {element.Author}");
                Console.WriteLine($"Год: {element.Year}");
                Console.WriteLine($"Популярность: {element.ViewsCount}");
                Console.WriteLine();
            }
        }
        public void ReadBook()// взять книгу, в будущем добавлю пару фич
        {
            Console.Write("Введите ID книги, которую хотите почитать: ");
            int bookID = Convert.ToInt32(Console.ReadLine());

            Book foundbook = books.FirstOrDefault(b => b.Id == bookID);

            if (foundbook == null)
            {
                Console.WriteLine("Такой книги не найдено.");
            }
            else
            {
                foundbook.ViewsCount++;
                Console.WriteLine($"Вы открыли книгу '{foundbook.Title}'. Приятного чтения!");
            }
        }
        public void ReturnBook()
        {

        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            LibraryManager manager = new LibraryManager();
            manager.LoadData();
            manager.ULoadDota();
            int choice = -1;
            bool isAuthenticated = false;
            while (!isAuthenticated)
            {
                Console.WriteLine("-------- Вас приветствует электронная библиотека --------");
                Console.WriteLine("Вам необходимо войти или зарегестрироваться");
                Console.WriteLine("1 - Войти");
                Console.WriteLine("2 - Зарегестрироваться");
                Console.WriteLine("0 - Выход");
                choice = Convert.ToInt32(Console.ReadLine());
                Console.Clear();
                switch (choice)
                {
                    case 1:
                        isAuthenticated = manager.LoginUser();
                        break;
                    case 2:
                        manager.RegisterUser();
                        manager.USaveData();
                        break;
                }
            }
               if (isAuthenticated)
               {
                while (choice != 0)
                  {
                    {
                        Console.WriteLine("-------- Вас приветствует электронная библиотека --------");
                        Console.WriteLine("Выберите что хотите сделать");
                        Console.WriteLine("1 - Добавить книгу");
                        Console.WriteLine("2 - Посмотреть список всех книг");
                        Console.WriteLine("3 - Взять книгу");
                        Console.WriteLine("0 - Выход");
                        choice = Convert.ToInt32(Console.ReadLine());
                        Console.Clear();
                        switch (choice)
                        {
                            case 1:
                                manager.AddBook();
                                manager.SaveData();
                                break;
                            case 2:
                                manager.ShowAllBooks();
                                break;
                            case 3:
                                manager.ShowAllBooks();
                                manager.ReadBook();
                                manager.SaveData();
                                break;
                            case 0:
                                Environment.Exit(choice);
                                break;
                        }
                    }
                }
            }
               }
        }
    }