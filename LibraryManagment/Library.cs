using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace LibraryManagment
{
    class Library
    {
      
        private MongoService _mongoService;
        private List<Loan> loans = new List<Loan>();
        public Library(MongoService mongoService)
        {
            _mongoService = mongoService;//database erisimi
        }

        //Kullanici kayidi - Degistirildi MONGO DB
        public void RegisterUser(User newUser)
        {
            var collection = _mongoService.GetUserCollection();//user collectiona erisim saglamak

            var existingUser = collection.Find(u => u.Id == newUser.Id).FirstOrDefault();//Eger yeni kullanici db de mevcutsa !=null
            if (existingUser != null)
            {
                Console.WriteLine("This ID is already taken.");
                return;
            }

            collection.InsertOne(newUser);
            Console.WriteLine("User registered to MongoDB.");
        }

        //Login kontrolü yapan metot
        public User Login(string username, string password)
        {
            var collection = _mongoService.GetUserCollection();
            var filter = Builders<User>.Filter.Eq(u => u.Id, username) & Builders<User>.Filter.Eq(u => u.Password, password);//Builders sorgu olusturmak icin arac
            var user = collection.Find(filter).FirstOrDefault();//Ilk eleman veya null dönmesi FirstOrDefault

            return user;
        }


        public User GetUserById(string id)
        {
            var collection = _mongoService.GetUserCollection();
            var user = collection.Find(u => u.Id == id).FirstOrDefault();
            return user;
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
                    Entity entity = user;
                    entity.DisplayInfo();
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

            var collection = _mongoService.GetBookCollection();//book koleksiyonuna erisim

            collection.InsertOne(newBook);//koleksiyona ekleme
            Console.WriteLine("Book successfully added to MongoDB.");
        }
        //Kitap güncellemek
        public void UpdateBook()
        {
            var collection = _mongoService.GetBookCollection(); 

            Console.WriteLine("Enter the ISBN for updating the book: ");
            string updateBookISBN = Console.ReadLine();

            // MongoDB ISBN e göre arama
            var filter = Builders<Book>.Filter.Eq(b => b.ISBN, updateBookISBN);//kitabi bulmak icin filtre
            var book = collection.Find(filter).FirstOrDefault();

            if (book == null)
            {
                Console.WriteLine("Given ISBN not found.");
                return;
            }

            //yeni bilgiler
            Console.WriteLine($"ISBN of the book: {updateBookISBN} ");
            Console.WriteLine("Title of the book: ");
            string updateBookTitle = Console.ReadLine();
            Console.WriteLine("Author of the book: ");
            string updateBookAuthor = Console.ReadLine();
            Console.WriteLine("Genre of the book: ");
            string updateBookGenre = Console.ReadLine();
            Console.WriteLine("Stock of the book: ");
            int updateBookStock = int.Parse(Console.ReadLine());

            // MongoDB update için yeni değerler
            var update = Builders<Book>.Update //database guncellemesi
                .Set(b => b.Title, updateBookTitle)
                .Set(b => b.Author, updateBookAuthor)
                .Set(b => b.Genre, updateBookGenre)
                .Set(b => b.Stock, updateBookStock);

            //koleksiyon update
            collection.UpdateOne(filter, update);

            Console.WriteLine("Book updated successfully");
        }
        //Kitap silmek icin metot
        public void DeleteBook()
        {
            var collection = _mongoService.GetBookCollection(); 

            Console.WriteLine("Enter the ISBN of the book to delete: ");
            string bookToDelete = Console.ReadLine();

            var filter = Builders<Book>.Filter.Eq(b => b.ISBN, bookToDelete);//Filtre olusturma

            var result = collection.DeleteOne(filter);//filtreye göre arama ve silme

            if (result.DeletedCount > 0)
            {
                Console.WriteLine("Book successfully deleted.");
            }
            else
            {
                Console.WriteLine("ISBN not found.");
            }
        }


        //Kitaplari listelemek 
        public void ShowBooks()
        {
            var collection = _mongoService.GetBookCollection();

            var books = collection.Find(_ => true).ToList();  // Tüm kitapları getir

            Console.WriteLine("=====BOOKS=====");
            foreach (var book in books)
            {
                Entity entity = book;
                entity.DisplayInfo();
            }
        }


        //Kitap ödünc alma
        public void BorrowBook(User user)
        {
            Console.WriteLine("Enter the ISBN of the book to borrow: ");
            string borrowIsbn = Console.ReadLine();

            var bookCollection = _mongoService.GetBookCollection();
            var loanCollection = _mongoService.GetLoanCollection();

            // Kitabı bul
            var bookToBorrow = bookCollection.Find(b => b.ISBN == borrowIsbn).FirstOrDefault();

            if (bookToBorrow == null)
            {
                Console.WriteLine("Book not found.");
                return;
            }

            if (bookToBorrow.Stock <= 0)
            {
                Console.WriteLine("Book is out of stock.");
                return;
            }

            // book koleksiyonunda Stock'u 1 azalt
            var update = Builders<Book>.Update.Inc(b => b.Stock, -1);
            bookCollection.UpdateOne(b => b.ISBN == borrowIsbn, update);

            // Yeni loan kaydıni koleksiyona ekle
            Loan newLoan = new Loan(user.Id, borrowIsbn);
            loanCollection.InsertOne(newLoan);

            Console.WriteLine("Book borrowed successfully.");
        }

        //Ödünc alinmis kitaplar
        public void BorrowedBooks(User user)
        {
            var loanCollection = _mongoService.GetLoanCollection();
            var bookCollection = _mongoService.GetBookCollection();

            var userLoans = loanCollection.Find(loan => loan.UserId == user.Id).ToList();

            if (userLoans.Count == 0)
            {
                Console.WriteLine("You have not borrowed any books.");
                return;
            }

            foreach (var loan in userLoans)
            {
                var book = bookCollection.Find(b => b.ISBN == loan.ISBN).FirstOrDefault();
                if (book != null)
                {
                    Console.WriteLine($"Title: {book.Title}, Author: {book.Author}, ISBN: {book.ISBN}");
                }
            }
        }

        //Kitap iade etmek
        public void ReturnBook(User user)
        {
            var loanCollection = _mongoService.GetLoanCollection();
            var bookCollection = _mongoService.GetBookCollection();

            Console.WriteLine("Enter the ISBN of the book to return: ");
            string returnIsbn = Console.ReadLine();

            // Loan kaydını bul
            var filter = Builders<Loan>.Filter.Eq(l => l.UserId, user.Id) &
                         Builders<Loan>.Filter.Eq(l => l.ISBN, returnIsbn);

            var loanToRemove = loanCollection.Find(filter).FirstOrDefault();

            if (loanToRemove == null)
            {
                Console.WriteLine("You haven't borrowed this book.");
                return;
            }

            // Loan kaydını sil
            loanCollection.DeleteOne(filter);

            // Kitabın stok değerini artır
            var bookFilter = Builders<Book>.Filter.Eq(b => b.ISBN, returnIsbn);
            var update = Builders<Book>.Update.Inc(b => b.Stock, 1); // Stock++ gibi
            bookCollection.UpdateOne(bookFilter, update);

            Console.WriteLine("Book returned successfully.");
        }

        //Kitap arama (yazar,tür,kitap ismi)
        public void SearchBook()
        {
            var bookCollection = _mongoService.GetBookCollection();

            Console.WriteLine("Search by:\n1) Title\n2) Author\n3) Genre");
            string choice = Console.ReadLine();

            Console.WriteLine("Search keyword: ");
            string keyword = Console.ReadLine();

            FilterDefinition<Book> filter = Builders<Book>.Filter.Empty;//baslangicta filtre bos

            if (choice == "1")
            {
                filter = Builders<Book>.Filter.Regex(b => b.Title, new MongoDB.Bson.BsonRegularExpression(keyword, "i"));
            }
            else if (choice == "2")
            {
                filter = Builders<Book>.Filter.Regex(b => b.Author, new MongoDB.Bson.BsonRegularExpression(keyword, "i"));
            }
            else if (choice == "3")
            {
                filter = Builders<Book>.Filter.Regex(b => b.Genre, new MongoDB.Bson.BsonRegularExpression(keyword, "i"));
            }
            else
            {
                Console.WriteLine("Invalid choice.");
                return;
            }

            var results = bookCollection.Find(filter).ToList();

            if (results.Count == 0)
            {
                Console.WriteLine("No books found matching your search.");
            }
            else
            {
                Console.WriteLine("Search Result:");
                foreach (var book in results)
                {
                    Console.WriteLine($"Title: {book.Title}, Author: {book.Author}, ISBN: {book.ISBN}, Genre: {book.Genre}");
                }
            }
        }


        //Kullanici bilgilerini guncelle ADMIN
        public void UpdateUserInfoByAdmin()
        {
            var userCollection = _mongoService.GetUserCollection();

            Console.WriteLine("Enter the ID of the user to update: ");
            string userId = Console.ReadLine();

            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var userToUpdate = userCollection.Find(filter).FirstOrDefault();

            if (userToUpdate == null)
            {
                Console.WriteLine("User not found");
                return;
            }

            Console.WriteLine($"Current Name: {userToUpdate.Name}");
            Console.Write("New Name: ");
            string newName = Console.ReadLine();

            Console.WriteLine($"Current Email: {userToUpdate.Mail}");
            Console.Write("New Mail: ");
            string newMail = Console.ReadLine();

            Console.WriteLine($"Current Password: {userToUpdate.Password}");
            Console.Write("New Password: ");
            string newPassword = Console.ReadLine();

            var update = Builders<User>.Update //useri databasede guncelleme
                .Set(u => u.Name, newName)
                .Set(u => u.Mail, newMail)
                .Set(u => u.Password, newPassword);

            userCollection.UpdateOne(filter, update);

            Console.WriteLine("User information updated successfully");
        }

        //Kullaniciyi sil ADMIN
        public void DeleteUserByAdmin()
        {
            var userCollection = _mongoService.GetUserCollection();

            Console.WriteLine("Enter the ID of the user to delete: ");
            string userId = Console.ReadLine();

            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var userToDelete = userCollection.Find(filter).FirstOrDefault();

            if (userToDelete == null)
            {
                Console.WriteLine("User not found.");
                return;
            }

            userCollection.DeleteOne(filter);
            Console.WriteLine("User deleted successfully.");
        }

        //Tüm kullanicilari göster
        public void ShowAllUsers()
        {
            var userCollection = _mongoService.GetUserCollection();
            var users = userCollection.Find(_ => true).ToList();//_ tümünü göster

            if (users.Count == 0)
            {
                Console.WriteLine("No registered users.");
                return;
            }

            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.Id}, Name: {user.Name}, Email: {user.Mail}");
            }
        }


    }
}
  