using System.Threading.Tasks;
using DatingApp.API.Models;
/*Ez za interfész gyűjti az autentikációhoz szükséges metódusokat */
namespace DatingApp.API.Data
{
    public interface IAuthRepository
    {   /*Egy asszinkron task ami User-t ad vissza használja Register nevű task
        És egy User típusu user-t adunk át egy string password-el
         */
          Task<User> Register(User user, string password);
         /*Egy asszinkron task ami a Usert ad vissza Login nevű task
         Egy string username-t és egy string password-öt adunk át a tasnknak */  
          Task<User> Login(string username, string password); 
        /*Egy aszinkronk task ami egy bool- értékkel tér vissza és egy string -et adunk át neki */
          Task<bool> UserExist(string username);
    }
}