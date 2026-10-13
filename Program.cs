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
        public List<Book> BorrowedBooks { get; set; } = new List<Book>();


        public User(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
    class LibraryManager
    {
        private List<Book> books = new List<Book>();
        private string filePath = "books_data.json";
        public void SaveData()
        {
            string jsonString = JsonSerializer.Serialize(books, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(filePath, jsonString);
        }
        public void LoadData()
        {
            if (File.Exists(filePath))
            {

                string jsonString = File.ReadAllText(filePath);

                books = JsonSerializer.Deserialize<List<Book>>(jsonString);
            }
        }
        private List<User> users = new List<User>();
        public void AddBook() // добавить книгу, в целом изи
        {
            Console.WriteLine("Введите id книги:");
            int pID;
            if (int.TryParse(Console.ReadLine(), out pID))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Произошла ошибка: ...");
                Console.ResetColor();
                Console.Clear();
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
        public void RegisterUser()// регистрация, хз нах нужна, в будущем сделаю что книгу может брать ток зареганный юзер
        {
            Console.WriteLine("Введите ваш ID:");
            int sID = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите ваше имя:");
            string sname = Console.ReadLine();
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
            Console.WriteLine("Введите ID книги, которую хотите почитать:");
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
            while (true)
            {
                Console.WriteLine("-------- Вас приветствует электронная библиотека --------");
                Console.WriteLine("Выберите что хотите сделать");
                Console.WriteLine("1 - Добавить книгу");
                Console.WriteLine("2 - Посмотреть список всех книг");
                Console.WriteLine("3 - Взять книгу");
                Console.WriteLine("4 - Зарегестрироваться");
                int choice = Convert.ToInt16(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        manager.AddBook();
                        manager.SaveData();
                        break;
                    case 2:
                        Console.Clear();
                        manager.ShowAllBooks();
                        break;
                    case 3:
                        Console.Clear();
                        manager.ReadBook();
                        manager.SaveData();
                        break;
                    case 4:
                        Console.Clear();
                        manager.RegisterUser();
                        break;
                }
            }
        }
    }
}