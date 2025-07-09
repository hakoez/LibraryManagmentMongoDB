using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagment
{
    class Library
    {
        private List<User> users = new List<User>(){
            new User("admin", "Administrator", "admin@library.com", "admin")
            };

        private List<Book> books = new List<Book>() {
            new Book("Moby Dick", "Herman Melville", "123456", "Adventure", 10),
            new Book("War and Peace", "Leo Tolstoy", "123123", "Historical", 5),
            new Book("Pride and Prejudice", "Jane Austen", "123321", "Romance", 10),
            new Book("The Hobbit", "Tolkien", "123231", "Fantasy", 5)


        };
        private List<Loan> loans = new List<Loan>();

       //Kullanici kayidi
        public void RegisterUser(User newUser)
        {
            foreach(var user in users)
            {
                if(user.Id == newUser.Id)
                {
                    Console.WriteLine("This ID is already taken.");
                    return;
                }
            }
            users.Add(newUser);
            Console.WriteLine("User registered.");
        }
        //Login kontrolü yapan metot
        public User Login(string username,string password)
        {
            foreach(var user in users)
            {
                if(user.Id == username &&  user.Password == password)
                {
                    return user;
                }
               
            }
            return null;
        }
        //User id yi getiren metot
        public User GetUserById(string id)
        {
            foreach(var user in users)
            {
                if (user.Id == id)
                    return user;
            }
            return null;
        }
         //User menuye gecen metot
        public void UserMenu(User user)
        {
            while (true)
            {
                Console.WriteLine("1)View Account Info");
                Console.WriteLine("2)Update Account Info");
                Console.WriteLine("3)Show Books");
                Console.WriteLine("4)Search Books");
                Console.WriteLine("5)Borrowed Books");
                Console.WriteLine("6)Borrow Book");
                Console.WriteLine("7)Return Book");
                Console.WriteLine("8)Logout");
                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    user.DisplayInfo();
                }
                else if (choice == "2")
                {
                    user.UpdateInfo();
                }
                else if(choice=="3")
                {
                    ShowBooks();
                }
                else if (choice=="4")
                {
                    SearchBook();
                }
                else if (choice=="5")
                {
                    BorrowedBooks(user);
                }
                else if (choice=="6")
                {
                    BorrowBook(user);
                }
                else if (choice=="7")
                {
                    ReturnBook(user);
                }
                else
                {
                    Console.WriteLine("Logging out...");
                    break;
                }
            }
        }
        //Admin Menu
        public void AdminMenu()
        {
            while (true)
            {
                Console.WriteLine("\n==== ADMIN PANEL ====");
                Console.WriteLine("1)Show All books");
                Console.WriteLine("2)Add Book");
                Console.WriteLine("3)Update Book");
                Console.WriteLine("4)Delete Book");
                Console.WriteLine("5)Show All Users");
                Console.WriteLine("6)Update User");
                Console.WriteLine("7)Delete User");
                Console.WriteLine("8)Logout");
                Console.WriteLine("Choose an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowBooks();
                        break;
                    case "2":
                        AddBook();
                        break;
                    case "3":
                        UpdateBook();
                        break;
                    case "4":
                        DeleteBook();
                        break;
                    case "5":
                        ShowAllUsers();
                        break;
                    case "6":
                        UpdateUserInfoByAdmin();
                        break;
                    case "7":
                        DeleteUserByAdmin();
                        break;
                    case "8":
                        Console.WriteLine("Logging out...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice...");
                        break;
                }

            }
        }
        //Kitap Eklemek icin metot
        public void AddBook()
        {
            
            Console.WriteLine("Title of the book: ");
            string newBookTitle = Console.ReadLine();
            Console.WriteLine("Author of the book: ");
            string newBookAuthor = Console.ReadLine();
            Console.WriteLine("ISBN of the book: ");
            string newBookISBN = Console.ReadLine();
            Console.WriteLine("Genre of the book: ");
            string newBookGenre = Console.ReadLine();
            Console.WriteLine("Stok of the book: ");
            int newBookStok= int.Parse(Console.ReadLine());
           
            Book newBook = new Book(newBookTitle, newBookAuthor, newBookISBN, newBookGenre,newBookStok );
            books.Add(newBook);

            Console.WriteLine("Book sucessfully added.");
        }
        //Kitap güncellemek
        public void UpdateBook()
        {
            Console.WriteLine("Enter the ISBN for updating the book: ");
            string updateBook = Console.ReadLine();

            foreach ( var book in books)
            {
                if(updateBook==book.ISBN)
                {
                    Console.WriteLine($"ISBN of the book: {updateBook} ");
                    Console.WriteLine("Title of the book: ");
                    string updateBookTitle = Console.ReadLine();
                    Console.WriteLine("Author of the book: ");
                    string updateBookAuthor = Console.ReadLine();
                    Console.WriteLine("Genre of the book: ");
                    string updateBookGenre = Console.ReadLine();
                    Console.WriteLine("Stok of the book: ");
                    int updateBookStok = int.Parse(Console.ReadLine());

                    book.Title = updateBookTitle;
                    book.Author = updateBookAuthor;
                    book.Genre = updateBookGenre;
                    book.Stock = updateBookStok;

                    Console.WriteLine("Book updated successfully");
                    return;
                }
            }
            Console.WriteLine("Given ISBN not found.");
        }
        //Kitap silmek icin metot
        public void DeleteBook()
        {
            Console.WriteLine("Enter the ISBN of the book to delete: ");
            string bookToDelete= Console.ReadLine();

            for(int i = 0;i<books.Count;i++)
            {
                if (books[i].ISBN == bookToDelete)
                {
                    books.RemoveAt(i);
                    Console.WriteLine("Book successfully deleted.");
                    return;
                }
            }

            Console.WriteLine("ISBN not found.");
        }

        //Kitaplari listelemek 
        public void ShowBooks()
        {
            foreach(var book in books)
            {
                Console.WriteLine($"Title:{book.Title}, Author:{book.Author}, ISBN:{book.ISBN}, Genre:{book.Genre}, Stock:{book.Stock}");
            }
        }


        //Kitap ödünc alma
        public void BorrowBook(User user)
        {
            Console.WriteLine("Enter the ISBN of the book to borrow: ");
            string borrowIsbn = Console.ReadLine();

            Book bookToBorrow = null;

            foreach(var book in books)
            {
                if(book.ISBN == borrowIsbn)
                {
                    bookToBorrow = book;
                    break;
                }
            }
            if(bookToBorrow == null)
            {
                Console.WriteLine("Book not found.");
                return;
            }
            if(bookToBorrow.Stock<=0)
            {
                Console.WriteLine("Book is out of stock.");
                return;
            }

            loans.Add(new Loan(user.Id, borrowIsbn));
            bookToBorrow.Stock--;

            Console.WriteLine("Book borrowed successfully.");
        }
        //Ödünc alinmis kitaplar
        public void BorrowedBooks(User user)
        {
            bool found = false;
            foreach(var loan in loans)
            {
                if(user.Id==loan.UserId)
                {
                  foreach(var book in books)
                    {
                        if(book.ISBN==loan.ISBN)
                        {
                            Console.WriteLine($"Title:{book.Title},Author:{book.Author},ISBN:{book.ISBN}");
                            found = true;
                        }
                    }
                }
            }
            if(!found)
            {
                Console.WriteLine("You have not borrowed any books.");
            }
        }
        //Kitap iade etmek
        public void ReturnBook(User user)
        {
            Console.WriteLine("Enter the ISBN of the book to return: ");
            string returnIsbn = Console.ReadLine();

            Loan loanToRemove = null;

            foreach(var loan in loans)
            {
                if(loan.UserId == user.Id && loan.ISBN == returnIsbn)
                {
                    loanToRemove = loan;
                    break;
                }
            }

            if(loanToRemove == null)
            {
                Console.WriteLine("You havent borrowed this book.");
                return;
            }

            loans.Remove(loanToRemove);

            foreach(var book in books)
            {
                if (book.ISBN == returnIsbn)
                {
                    book.Stock++;
                    break;
                }

            }

            Console.WriteLine("Book returned successfully.");
        }
        //Kitap arama (yazar,tür,kitap ismi)
        public void SearchBook()
        {
            Console.WriteLine("Search by :\n1)Title \n2)Author \n3)Genre");
            string choice = Console.ReadLine();

            Console.WriteLine("Search keyword: ");
            string keyword = Console.ReadLine();

            List<Book> results = new List<Book>();

            foreach(var book in books)
            {
                if((choice=="1" && book.Title.ToLower().Contains(keyword)))
                {
                    results.Add(book);
                }
                else if(choice=="2"&& book.Author.ToLower().Contains(keyword))
                {
                    results.Add(book);
                }
                else if (choice=="3" && book.Genre.ToLower().Contains(keyword))
                {
                    results.Add(book);
                }
            }

            if(results.Count == 0)
            {
                Console.WriteLine("No books found matching your search.");
            }
            else
            {
                Console.WriteLine("Search Result: ");
                foreach(var book in results)
                {
                    Console.WriteLine($"Tittle:{book.Title},Author:{book.Author},ISBN:{book.ISBN},Genre:{book.Genre}");
                }
            }
        }
        //Admin Kullanici görüntüleme
        public void ViewAllUsers()
        {
            if(users.Count == 0)
            {
                Console.WriteLine("No registered users.");
                return;
            }

            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.Id},Name: {user.Name}, Email: {user.Mail}");
            }
           
        }
        //Kullanici bilgilerini guncelle ADMIN
        public void UpdateUserInfoByAdmin()
        {
            Console.WriteLine("Enter the ID of the user to update: ");
            string userId = Console.ReadLine();

            User userToUpdate = GetUserById(userId);
            if(userToUpdate == null)
            {
                Console.WriteLine("User not found");
                return;
            }

            Console.WriteLine($"Current Name: {userToUpdate.Name}");
            Console.Write("New Name: ");
            userToUpdate.Name = Console.ReadLine();

            Console.WriteLine($"Current Email: {userToUpdate.Name}");
            Console.Write("New Mail: ");
            userToUpdate.Mail = Console.ReadLine();

            Console.WriteLine($"Current Password: {userToUpdate.Name}");
            Console.Write("New Password: ");
            userToUpdate.Password = Console.ReadLine();

            Console.WriteLine("User information updated successfully");
        }
        //Kullaniciyi sil ADMIN
        public void DeleteUserByAdmin()
        {
            Console.WriteLine("Enter the ID of the user to delete: ");
            string userId = Console.ReadLine();

            User userToDelete = GetUserById(userId);

            if(userToDelete == null)
            {
                Console.WriteLine("User not found.");
                return;
            }

            users.Remove(userToDelete);
            Console.WriteLine("User deleted successfully.");


        }
        //Tüm kullanicilari göster
        public void ShowAllUsers()
        {
            foreach(var user in users)
            {
                Console.WriteLine($"ID:{user.Id},Name:{user.Name}, Mail:{user.Mail}");
            }
        }
       
        
    }
}
  