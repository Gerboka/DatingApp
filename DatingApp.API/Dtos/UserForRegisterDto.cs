using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{/*Ez a Data transfer osztály adódik át az autentikálást végző controller osztály-hoz
a Register metódushoz. Ebben ellenőrizzük most hogy kitöltötte-e a Username, password
mezőket ne legyen üres regisztráció pl
 */
    public class UserForRegisterDto
    {   
        /*Ahhoz, hogy valami szükséges legyen ezt a [Required]parancsot használjuk */
        [Required]
        public string Username { get; set; }
        /*További feltételeket adhatunk például a string hosszúsága minimum, maximum és párosítunk hozzá egy
        error üzenetet */
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage ="Egy 4 és 8 karakter közötti jelszót adj meg köszi!")]
        public string Password { get; set; }
    }
}