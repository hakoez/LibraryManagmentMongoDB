using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Serializers;

namespace LibraryManagment
{
    class Program
    {
        static void Main(string[] args)
        {

            MongoService mongoService = new MongoService();
            User.SetMongoCollection(mongoService.GetUserCollection());//user koleksiyonunu _userCollectiona atama
            Library managment = new Library(mongoService);
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

                    Console.Write("Enter password(min 6 max 14 characters): ");
                    string pass = Console.ReadLine();
                    if (pass.Length < 6 || pass.Length > 14)
                    {
                        Console.WriteLine("Please enter your password between 6-14 characters.");
                        continue;
                    }
                    else
                    {
                        newUser.Password = pass;
                    }
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