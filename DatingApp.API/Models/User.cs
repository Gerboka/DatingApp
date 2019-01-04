/*Ez az osztály a felhassználó-hoz tartozó osztály */
namespace DatingApp.API.Models
{
    public class User
    {   /*A User Id értéke integer-ben ami az adatbázis Id értéke */
        public int Id { get; set; }
        /*A User Username-e vagyis felhasználó neve string-ben */
        public string Username { get; set; }
        /*A felhasszánló jelszava elősször hash-elve kódolva */
        public byte[] PasswordHash { get; set; }
        /*A felhasszánló jelszavának SALT-ja*/
        public byte[] PasswordSalt { get; set; }
    }
}