using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Spectre.Console;
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
        public static void Error(string ErrorMessage)
        {
            AnsiConsole.MarkupLine("[red]Ошибка![/] ");
            Console.WriteLine(ErrorMessage);
            Console.Write("\nНажмите Enter для продолжения...");
            Console.ReadKey();
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
                try
                {
                    books = JsonSerializer.Deserialize<List<Book>>(File.ReadAllText(filePath)) ?? new List<Book>();
                }
                catch (JsonException)
                {
                    books = new List<Book>();
                }
            }
        }
        public void ULoadDota()
        {
            if (File.Exists(UfilePath))
            {
                try
                {
                    users = JsonSerializer.Deserialize<List<User>>(File.ReadAllText(UfilePath)) ?? new List<User>();
                }
                catch (JsonException)
                {
                    users = new List<User>();
                }
            }
        }
        public static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result))
                    return result;

                Console.WriteLine("Нужно ввести целое число, попробуй ещё раз.");
            }
        }

        private static string HashPassword(string password) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(password)));

        public void AddBook()
        {
            int id = 0;
            Console.Write("Введите название книги: ");
            string Ptitle = Console.ReadLine();
            Console.Write("Введите автора книги: ");
            string pAuthor = Console.ReadLine();
            Console.Write("Введите год выпуска: ");
            if (!int.TryParse(Console.ReadLine(), out int Year) || Year > DateTime.Now.Year)
            {
                Console.WriteLine("Ошибка, используйте цифры и настоящий год");
                return;
            }
            id = books.Count > 0 ? books.Max(u => u.Id) + 1 : 1;
            Book book2 = new Book(id, Ptitle, pAuthor, Year);
            books.Add(book2);
            Console.Write("Книга успешно добавлена. Нажмите Enter для продолжения...");
            Console.ReadKey();
            Console.Clear();
        }
        public void RegisterUser()
        {
            int Id = 0;
            Console.Write("Введите ваше имя: ");
            string name = Console.ReadLine();
            name = name.Trim(Convert.ToChar(" "));
            bool NameExist = users.Any(b => b.Name == name);
            if (NameExist)
            {
                Error("Пользователь уже существует");
                return;
            }
            string password = AnsiConsole.Prompt(new TextPrompt<string>("Введите пароль:").Secret());
            password = password.Trim(Convert.ToChar(" "));
            if (string.IsNullOrWhiteSpace(password))
            {
                Error("Пароль не может быть пустым!");
                return;
            }
            string passwordHash = HashPassword(password);
            Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
            User user = new User(Id, name, passwordHash);
            users.Add(user);
            Console.Write("Регистрация успешна! Нажмите Enter для продолжения...");
            Console.ReadKey();
        }
        public bool LoginUser()
        {
            Console.Write("Введите ваше имя: ");
            string name = Console.ReadLine();
            name = name.Trim(Convert.ToChar(" "));
            string password = AnsiConsole.Prompt(new TextPrompt<string>("Введите пароль:").Secret());
            password = password.Trim(Convert.ToChar(" "));
            string passwordHash = HashPassword(password);
            User element = users.FirstOrDefault(u => u.Name == name);
            if (element == null)
            {
                Error("Пользователь с таким именем не найден.");
                return false;
            }
            if (passwordHash != element.Password)
            {
                Error("Пароль неверный");
                return false;
            }
            Console.Write("Вход успешно выполнен! Нажмите Enter для продолжения...");
            Console.ReadKey();
            Console.Clear();
            return true;

        }
        public void ShowAllBooks()
        {
            var table = new Table();
            table.AddColumn("Id книги");
            table.AddColumn("Название");
            table.AddColumn("Автор");
            table.AddColumn("Год");
            table.AddColumn("Популярность");
            foreach (Book element in books)
            {
                table.AddRow(Convert.ToString(element.Id), 
                    element.Title, 
                    element.Author, 
                    element.Year.ToString(), 
                    element.ViewsCount.ToString());
            }
            AnsiConsole.Write(table);
        }
        public void ReadBook()
        {
            int bookID = ReadInt("Введите ID книги которую хотите почитать");

            Book foundbook = books.FirstOrDefault(b => b.Id == bookID);

            if (foundbook == null)
            {
                Error("Такой книги не найдено");
            }
            else
            {
                foundbook.ViewsCount++;
                Console.WriteLine($"Вы открыли книгу '{foundbook.Title}'. Приятного чтения!");
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            LibraryManager manager = new LibraryManager();
            manager.LoadData();
            manager.ULoadDota();
            var choice = "";
            bool isAuthenticated = false;
            while (!isAuthenticated)
            {
                choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Вам необходимо войти или зарегестрироваться")
                    .AddChoices("Войти", "Зарегестрироваться", "Выйти"));
                Console.Clear();
                switch (choice)
                {
                    case "Войти":
                        isAuthenticated = manager.LoginUser();
                        break;
                    case "Зарегестрироваться":
                        manager.RegisterUser();
                        manager.USaveData();
                        break;
                    case "Выйти":
                        Environment.Exit(0);
                        break;
                }
            }
                while (choice != "Выход")
                {
                    {
                        AnsiConsole.MarkupLine($"[purple]--------Вас приветствует электронная библиотека--------[/]");
                        choice = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                            .AddChoices("Добавить книгу", "Посмотреть список всех книг", "Взять книгу", "Выход"));
                        Console.Clear();
                        switch (choice)
                        {
                            case "Добавить книгу":
                                manager.AddBook();
                                manager.SaveData();
                                break;
                            case "Посмотреть список всех книг":
                                manager.ShowAllBooks();
                                break;
                            case "Взять книгу":
                                manager.ShowAllBooks();
                                manager.ReadBook();
                                manager.SaveData();
                                break;
                            case "Выход":
                                Environment.Exit(0);
                                break;
                        }
                    }
                }
            }
        }
    }