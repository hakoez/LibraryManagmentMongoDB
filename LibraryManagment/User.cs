using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace LibraryManagment
{
    class User:Entity
    {
        public string Password {  get; set; }

        public string Mail { get; set; }

        public string Name { get; set; }

        public string Id {  get; set; }

        public User(string id, string name, string mail, string password)
        {
            Id = id;
            Name = name;
            Mail = mail;
            Password = password;
        }
        public User()
        {
        }
       
        //Kullanici bilgileri icin metot
        public override void DisplayInfo()
        {
            Console.WriteLine($"User Id: {Id}" );
            Console.WriteLine($"User Name: {Name}");
            Console.WriteLine($"User Password: {Password}");
            Console.WriteLine($"User Mail: {Mail}");
        }
        //Kullanici bilgilerini güncellemek icin metot
        public void UpdateInfo()
        {
            Console.WriteLine("Update your ID: ");
            string newID=Console.ReadLine();
            Console.WriteLine("Update your Name: ");
            string newName=Console.ReadLine();
            Console.WriteLine("Update your Password: ");
            string newPass=Console.ReadLine();
            Console.WriteLine("Update your Mail: ");
            string newMail=Console.ReadLine();
            Id = newID;
            Name = newName;
            Password = newPass;
            Mail = newMail;

        }

    }
}
