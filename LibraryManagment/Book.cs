using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagment
{
    class Book:Entity
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Genre { get; set; }
        public int Stock { get; set; }

        public Book(string title,string author,string isbn,string genre,int stock)
        {
            Title = title;
            Author = author;
            ISBN = isbn;
            Genre = genre;
            Stock = stock;
        }
        //kitap bilgileri icin metot
        public override void DisplayInfo()
        {
            Console.WriteLine($"Title:{Title}, Author:{Author}, ISBN:{ISBN}, Genre:{Genre}, Stock:{Stock}");
        }
    }
}
