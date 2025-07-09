using System;
using System.Collections.Generic;

namespace LibraryManagment
{
    class Program
    {
        static void Main(string[] args)
        {
            Library managment = new Library();
            User currentUser = null;

            while (true)
            {
                Console.WriteLine("\n1) Register");
                Console.WriteLine("2) Login");
                Console.WriteLine("3) Quit");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    User newUser = new User();

                    Console.Write("Enter name: ");
                    newUser.Name = Console.ReadLine();

                    Console.Write("Enter ID: ");
                    newUser.Id = Console.ReadLine();

                    Console.Write("Enter password: ");
                    newUser.Password = Console.ReadLine();

                    Console.Write("Enter email: ");
                    newUser.Mail = Console.ReadLine();

                    managment.RegisterUser(newUser);

                }

                else if (choice == "2")
                {
                    Console.WriteLine("Enter ID: ");
                    string userID = Console.ReadLine();
                    Console.WriteLine("Enter Password: ");
                    string userPWD = Console.ReadLine();
                    currentUser = managment.Login(userID, userPWD);

                    if (currentUser != null)
                    {
                        if (currentUser.Id == "admin" && currentUser.Password == "admin")
                        {
                            Console.WriteLine("Entering Admin panel...");
                            Admin admin = new Admin(managment);
                            admin.AdminMenu();
                            continue;
                        }
                        Console.WriteLine("Login Succesfully");
                        managment.UserMenu(currentUser);
                    }
                    else
                    {
                        Console.WriteLine("Login failed.");
                    }
                }
                else
                {
                    Console.WriteLine("Exiting...");
                    break;
                }
            }
        }
    }
}