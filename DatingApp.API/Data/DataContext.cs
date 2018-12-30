using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{   // Ez az osztály felelős az adatbázis kapcsolatért. Az osztály az EntityFramework segítségétvel küld utasítést, és adatokat kap vissza
    //az EntytiFramework által!

    //A DataContext osztályunkat a DbContext gyári osztályból származtatjuk, így
    //egyből hozzáférünk annak metódusaihoz, és fel is használhatjuk őket, módosíthatjuk őket!
    public class DataContext : DbContext
    {
        //Az alap DBContext Construktort felülírtuk, hogy a DataContext-ből legyen példányosítás a DbContext alap konrtruktora alapján.
        public DataContext(DbContextOptions<DataContext> options) : base (options)
        {
            
        }
        //Ezzel az eljárással az adatbázisban lévő Value értékét átadja a táblázat Values táblázatába
        //Ehhez referenciába be kellet írni using DatingApp.API.Models; , hogy lássa a Value osztályt.
        public DbSet<Value> Values { get; set; }
    }
}