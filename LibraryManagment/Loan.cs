using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace LibraryManagment
{
    [BsonIgnoreExtraElements]//koleksiyonda olan ancak benim istemedigim _id bilgisi icin
    public class Loan
    {
        public string UserId { get; set; }
        public string ISBN { get; set; }

        public Loan(string userId, string isbn)
        {
            UserId = userId;
            ISBN = isbn;
        }

    }
}
