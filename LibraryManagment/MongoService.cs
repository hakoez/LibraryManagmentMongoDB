using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagment;
using MongoDB.Driver;

public class MongoService
{
    private IMongoDatabase database;

    public MongoService()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        database = client.GetDatabase("LibraryDB");
    }

    public IMongoCollection<User> GetUserCollection()//user koleksiyonuna erisim
    {
        return database.GetCollection<User>("Users");
    }

    public IMongoCollection<Book> GetBookCollection()//book koleksiyonuna erisim
    {
        return database.GetCollection<Book>("Books");
    }

    public IMongoCollection<Loan> GetLoanCollection()//loan koleksiyonuna erisim
    {
        return database.GetCollection<Loan>("Loans");
    }
}