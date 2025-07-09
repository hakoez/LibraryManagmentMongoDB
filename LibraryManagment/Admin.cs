using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagment
{
    class Admin : User
    {
        private Library library;

        public Admin(Library library)
        {
            this.library = library;
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
                        library.ShowBooks();
                        break;
                    case "2":
                        library.AddBook();
                        break;
                    case "3":
                        library.UpdateBook();
                        break;
                    case "4":
                        library.DeleteBook();
                        break;
                    case "5":
                        library.ShowAllUsers();
                        break;
                    case "6":
                        library.UpdateUserInfoByAdmin();
                        break;
                    case "7":
                        library.DeleteUserByAdmin();
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

    }
}
