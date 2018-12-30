// Létrehozuk a value komponenst osztály ami több fájlból áll az angular-ban
// ezzel nyerjük ki az .NET API-ból a values értékeket
import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

// Itt az osztály komponensei láthatóak
@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
// Ez az osztály maga amiben vannak a változók, metódusok ect
export class ValueComponent implements OnInit {
  // Deklaráltunk egy változót values néven aminek megadtuk, hogy bármilyen típus lehet
  values: any;
  // A konstruktornak átadtuk a HttpClient modult http változóval ezzel tud Http parancsokat fogadni
  // a felhassználótól
  constructor(private http: HttpClient) { }
  // Ez az ngONinit indítja ez példányosításkor azokat a metóduokat amiket beleírunk
  ngOnInit() {
    this.getValues();
  }
  // Készítettünk egy metódust ami a Values értéket kinyeri
  getValues () {
    // A http get metódusával megadtuk milyen URL parancsra/parancsot aktiváljon, Ezaz .NET API része
    // Majd feliratkozva ha van válasz akkor a az osztály values értéke egyenlő lesz a válaszban kapott
    // értékkel más esetben a konzolon jelenjen meg az error log
    this.http.get('http://localhost:5000/api/values').subscribe(response => {
      this.values = response;
    }, error => {
      console.log(error);
    });
  }
}
