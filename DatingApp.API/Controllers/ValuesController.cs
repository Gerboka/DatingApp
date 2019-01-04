using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{   /* Ezzel az authorize külcsszóval meglehet adni, hogy azonosítás kell ehhez a controllerhez
     Ehhez kell viszont egy authentication middleware session 3 lecture 34   */
    [Authorize] 
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context;
        //A ValuesController konstruktor úgy készítettük el, hogy az a DataContext által adatbázis kapcsolat átadással történik meg
        public ValuesController(DataContext context)
        {
            _context = context;

        }
        // GET api/values 
        [HttpGet]
        /* Az eredeti alap Action Result. Ezt átalakítottuk, hogy az adatbázisbló nyerjen ki adatokat.
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Fruszi", "Geri" };
        }
        */

        //A mi ActionResult metódusunk vagyis IAnctionResult metódus kinyeri a Values
        //adatokat egy values változóba áttesszük a konstruktorban példányosított _concext
        //adatbázis EntityFramework által pedig az adatbázis Values értékeit Listába töltjük
        //A metódus értéke OK-val vissza tér és a values változóval ami már tartalmazza
        //az adatbázis Values táblájának értékeis listába rendezve.
        //Aztán ezzel az a baj, hogy szinkron kódolású, tehát nem enged
        //Több utasítást lefutni amíg ez le nem fut. Ez webappoknál használhatatlan
        /*
        public IActionResult GetValues()
        {
            var values= _context.Values.ToList();
            return Ok(values);
            
        }
        */
        /*
        Elősször is az async Task<>-ba rakjuk a metódusunkat ezzel aszinkroná tesszük
        A művelet elé az await parancsot írjuk és a ToList() metódus aszinkron változatát
        írjuk bele a ToListAsync()-et ehhez a using Microsoft.EntityFrameworkCore-t referenciába
        kell tennünk ebben az osztályban.
         */
        public async Task<IActionResult> GetValues()
        {
            var values= await _context.Values.ToListAsync();
            return Ok(values);
            
        }



        // GET api/values/5
        //Az AllowAnonymus parancsar bárki lekérheti ezt a parancsot [Authorize] -kel az osztály elejére ehhez a funkcióhoz
        [AllowAnonymous]
        [HttpGet("{id}")]
        /*Az eredeti specifikusan egy value értéket kinyerő metódust átszerkesztjük,
        hogy az adatbázisból nyerje ki az adatokat

        public ActionResult<string> Get(int id)
        {
            return "value";
        }
        */
        //A metódusnak átadjuk a böngészőbe beírt /5-öt például integer id változóbal
        //Ennek nem hagyom meg az eredeti szinkron kódját. Aszinkronná tesszük úgy mint a felette lévő metódust.
        public async Task <IActionResult> GetValues(int id)
        {   
            //A value változóva a _context meghívása segítségével amiáltal az
            //EntityFramework-nek hála tudja kezelni az adatbázist
            //A Values.First lehet FirstOrDefault. A First-nél ha nem talál olyat hibát dob ki
            //A FirstOrDefault nál pedig ha nem találja akkor egy alap értéket ad vissza hiba nélkül
            //A keresett szám x ami legyen nagyobb egyenlő mint az adatvázis Values Id-ja ami egyenlő 
            //A mi átadott id-nkal
            var value= await _context.Values.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(value);
        }
        
        
        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
