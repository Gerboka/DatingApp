using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;
/*Ebben az osztályban írjuk le pontosan az IAuthRepository interfészben található
metódusokat */
namespace DatingApp.API.Data
{   /*Hassználni kell az IAuthRepository interfész ezért az osztály neve után
    : IAuthRepository-t beírjuk */
    public class AuthRepository : IAuthRepository
    {   
        private readonly DataContext _context;
        
        
        /*A példányosításkor egy DataContext példányt adunk át amivel ebben az osztályban
        található context az átadottal lesz egyenlő */
        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        /* A beloggoláshoz szükséges aszinkron Task ami egy Usert ad vissza 
        string usernamet és string password-öt adunk át a metódusnak
         */
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return null;
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))    
                return null;
            return user;
        }
        // Ez a felhassználó által beír jelszó ellenörző metódus
         private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if(computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        public async Task<User> Register(User user, string password)
        {   
            /*A regisztráláshoz a kapott string password-öt át kell alakítanunk hash-elnünk majd salt-olnunk
            Ehhez kell két byte típus. */
            byte[] passwordHash;
            byte[] passwordSalt;
            //Meghívjuk a CreatePasswordHash metódust out referenciákkal
            CreatePasswordHash(password,out passwordHash,out passwordSalt);
            //Miután megtörtént a hashelés ennek user példánynak a PasswordHash és PasswordSalt
            //Értékeit felülírjuk az itt megkapott passwordHash, és passwordSalt-al.
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }
        /*A password hasheléséhez és saltolásához szükséges metódus 
        Ne kérdezd... működik hassználd!!!!!*/
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {   
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            
        }
        /*Felhassználó létezését ellenörző aszinkron task metódus */
        public async Task<bool> UserExist(string username)
        {
            if(await _context.Users.AnyAsync(x => x.Username == username))
                return true;
            return false;
        }   
    }
}