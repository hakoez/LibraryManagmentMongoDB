using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace LibraryManagment
{
    public class User:Entity
    {
        public string Password {  get; set; }

        public string Mail { get; set; }

        public string Name { get; set; }

        public string Id {  get; set; }

        private static IMongoCollection<User> _userCollection;//siniftan olusturulan nesnelerden bagimsiz tek bir degisken
                                                              //böylelikle koleksiyon baglantisi birkere tutulur ve tum nesneler kullanir.

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
        public static void SetMongoCollection(IMongoCollection<User> collection)//Koleksiyonu user sinifina bagli statik degiskene atiyor
        {                                                                       //static oldugu icin nesne olusturmadan direk erisilebilir
            _userCollection = collection;                                       //ortak metot
        }
        //Kullanici bilgileri icin metot
        public override void DisplayInfo()
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, this.Id);//Builder sorgulama icin arac
            var userFromDb = _userCollection.Find(filter).FirstOrDefault();//databasede usercollectiona uyan ilk veriyi getirme yoksa null

            if (userFromDb == null)
            {
                Console.WriteLine("User not found in database.");
                return;
            }

            Console.WriteLine($"User Id: {userFromDb.Id}");
            Console.WriteLine($"User Name: {userFromDb.Name}");
            Console.WriteLine($"User Password: {userFromDb.Password}");
            Console.WriteLine($"User Mail: {userFromDb.Mail}");
        }
        //Kullanici bilgilerini güncellemek icin metot
        public void UpdateInfo()
        {
            Console.WriteLine("Update your Name: ");
            string newName = Console.ReadLine();

            Console.WriteLine("Update your Password: ");
            string newPass = Console.ReadLine();

            Console.WriteLine("Update your Mail: ");
            string newMail = Console.ReadLine();

            var filter = Builders<User>.Filter.Eq(u => u.Id, this.Id);
            var update = Builders<User>.Update//MongoDB guncelleme komutu
                .Set(u => u.Name, newName)//koleksiyondaki veri
                .Set(u => u.Password, newPass)
                .Set(u => u.Mail, newMail);

            _userCollection.UpdateOne(filter, update);

            // Nesne üzerindeki veriler
            this.Name = newName;//programdaki anlik kullanici nesnesi 
            this.Password = newPass;
            this.Mail = newMail;

            Console.WriteLine("User updated successfully in database.");

        }

    }
}
