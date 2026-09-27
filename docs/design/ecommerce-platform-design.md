# E-Commerce Platform Design

## Översikt

Projektet är en fullstack e-handelsplattform för en svensk streetwear- och sneakerbutik.

Syftet är att bygga en fungerande e-handel med fokus på tydlig arkitektur, säkerhet, testbarhet och möjlighet att utveckla systemet vidare.

Plattformen ska stödja tre typer av användare:

- Besökare
- Kund
- Administratör

Besökare ska kunna se produkter och produktinformation. Kunder ska kunna skapa konto, logga in, använda kundvagn, genomföra köp och se tidigare order. Administratörer ska kunna hantera produkter, lager och order.

## Teknik

### Frontend

- React
- JavaScript

### Backend

- ASP.NET Core
- Entity Framework Core
- REST API

### Databas

- PostgreSQL

### Drift

- Docker
- Railway
- CI

## Arkitektur

Backend byggs som en modulär monolit och delas upp i fyra lager:

- Domain
- Application
- Infrastructure
- API

Domain innehåller domänmodeller och affärsregler.

Application innehåller use cases och applikationslogik.

Infrastructure hanterar databas, externa tjänster och andra tekniska delar.

API ansvarar för kommunikationen mellan frontend och backend.

## Backendmoduler

Backend delas upp i följande moduler:

- Identity
- Catalog
- Inventory
- Cart
- Checkout
- Orders
- Payments

Varje modul har ett tydligt ansvar.

Catalog ansvarar för produkter och produktinformation.

Inventory ansvarar för lager och lagerreservationer.

Cart ansvarar för kundvagnen.

Checkout ansvarar för flödet från kundvagn till färdig order.

Orders ansvarar för orderhantering och orderhistorik.

Payments ansvarar för det simulerade betalningsflödet och betalningsstatus.

Identity ansvarar för autentisering och behörighet.

Moduler ska kommunicera genom tydliga gränssnitt och ska inte ändra andra modulers data direkt.

## Domänmodell

De viktigaste modellerna är:

- Category
- Product
- ProductVariant
- ProductImage
- Cart
- CartItem
- Order
- OrderItem
- Payment
- CustomerProfile

### Product

Product innehåller information som är gemensam för en produkt, till exempel:

- Namn
- Beskrivning
- Varumärke
- Bilder

### ProductVariant

En Product kan ha flera ProductVariants.

Varje variant ska bland annat ha:

- Unik SKU
- Storlek
- Färg
- Pris
- Lagersaldo

Det gör det möjligt att hantera olika storlekar och färger av samma produkt separat.

### Cart och CartItem

Cart representerar kundens kundvagn.

CartItem innehåller vilken ProductVariant kunden har valt och antal.

Kundvagnen ska inte användas som slutlig källa för produktens pris. Priset ska alltid kontrolleras på backend vid checkout.

### Order och OrderItem

Order representerar ett köp.

OrderItem ska spara information om produkten, produktvarianten och priset vid köptillfället.

Det gör att tidigare order inte ändras om produktens namn eller pris ändras senare.

### Payment

Payment representerar betalningsdelen för en order.

Projektet använder inte riktiga betalningar. Betalningen ska i stället simuleras för att kunna testa hela checkout-flödet.

Payment kan till exempel innehålla:

- OrderId
- Belopp
- PaymentStatus
- Betalningsreferens
- Skapad tidpunkt

Exempel på betalningsstatus:

- Pending
- Completed
- Failed

## Säkerhet

Backend ska använda säker autentisering och behörighetskontroll.

Följande delar ingår:

- Säkra cookies
- CSRF-skydd
- Authorization policies
- Admin-policy
- Ägarskapskontroll
- Validering
- Rate limiting

Backend ska identifiera kunden genom den autentiserade sessionen.

Endpoints för kunddata ska inte acceptera ett valfritt `CustomerId` för att välja kundvagn, profil eller order.

En kund ska därför inte kunna komma åt en annan kunds data genom att ändra ett ID i en request.

Administrativa endpoints ska endast vara tillgängliga för användare med rätt behörighet.

Hemliga värden och känslig konfiguration ska inte lagras direkt i koden utan hanteras genom miljövariabler eller annan säker konfiguration.

## Kundvagn

Kundvagnen ska innehålla de produktvarianter som kunden vill köpa.

Kunden ska kunna:

- Lägga till produkter
- Ändra antal
- Ta bort produkter
- Se kundvagnens innehåll

Backend ska kontrollera att produktvarianten fortfarande finns och är aktiv.

Det pris som visas i frontend ska inte användas som slutgiltigt pris vid checkout.

## Checkout

Checkout hanteras på backend.

Ett förenklat flöde:

1. Kundvagnen valideras.
2. Aktuella priser hämtas från databasen.
3. Lagersaldo kontrolleras.
4. Lager reserveras.
5. Order och orderrader skapas.
6. En simulerad betalning genomförs.
7. Ordern bekräftas eller avbryts.

Backend ansvarar alltid för den slutliga prisberäkningen.

Frontend ska därför inte kunna skicka ett eget pris och få backend att lita på det.

En databastransaktion ska skydda de lokala databasändringarna, till exempel:

- Order
- Orderrader
- Lagerreservationer

Om något går fel under dessa steg ska ändringarna kunna återställas.

## Simulerad betalning

Projektet kommer inte att använda Stripe, Klarna eller någon annan riktig betaltjänst.

Betalningen ska i stället simuleras i systemet.

Syftet är att kunna visa hur ett betalningsflöde kan fungera utan att använda riktiga kortuppgifter eller riktiga pengar.

En simulerad betalning ska kunna lyckas eller misslyckas.

Vid lyckad betalning:

- Payment får status `Completed`
- Ordern bekräftas
- Lagerreservationen används för att uppdatera lagret

Vid misslyckad betalning:

- Payment får status `Failed`
- Ordern ska inte slutföras
- Lagerreservationen ska släppas

Det gör det möjligt att testa både lyckade och misslyckade checkout-flöden.

## Frontend

Frontend delas upp efter funktion.

Exempel:

- Authentication
- Products
- Cart
- Checkout
- Orders
- Admin

API-anrop ska gå genom en gemensam API-klient i stället för att ligga direkt i olika komponenter.

Frontend ska hantera olika tillstånd tydligt:

- Loading
- Empty
- Error
- Success

Gränssnittet ska vara responsivt och fungera på både mobil och desktop.

Grundläggande tillgänglighet ska också ingå, till exempel:

- Semantisk HTML
- Tydliga labels
- Tangentbordsnavigation
- Tydliga felmeddelanden

## Admin

Administratören ska kunna hantera de viktigaste delarna av butiken.

Det ska bland annat vara möjligt att:

- Skapa produkter
- Uppdatera produkter
- Hantera produktvarianter
- Hantera lager
- Se order
- Uppdatera relevant orderstatus

Adminfunktionerna ska skyddas genom authorization och särskilda policies.

## Teststrategi

Projektet ska testas på flera nivåer.

### Domain tests

Testar affärsregler och domänlogik.

Exempel:

- Ogiltigt antal produkter ska inte accepteras
- Lager får inte bli negativt
- En order ska ha ett giltigt totalbelopp

### Application tests

Testar use cases och applikationslogik.

Exempel:

- Lägga till produkt i kundvagn
- Skapa order
- Hantera simulerad betalning

### Integration tests

Testar integrationen mellan flera delar av systemet.

Exempel:

- Entity Framework Core och PostgreSQL
- Repository- eller databasoperationer
- Checkout och lagerreservation

### API tests

Testar:

- Endpoints
- Statuskoder
- Autentisering
- Authorization
- Request och response

### React tests

Testar viktiga komponenter och beteenden i frontend.

### End-to-end tests

Testar kompletta användarflöden.

Exempel:

- Registrera konto och logga in
- Visa produkter
- Lägga en produkt i kundvagnen
- Genomföra checkout
- Simulera betalning
- Se orderhistorik

## CI

CI ska automatiskt kontrollera projektet vid exempelvis push eller pull request.

Pipeline ska minst kunna:

- Bygga backend
- Bygga frontend
- Köra backend-tester
- Köra frontend-tester

Det gör det lättare att upptäcka problem innan förändringar läggs till i huvudgrenen.

## Docker och deployment

Projektet ska kunna köras med Docker.

Backend och PostgreSQL ska senare kunna deployas till Railway.

Projektet ska också innehålla:

- Health checks
- Loggning
- Miljöbaserad konfiguration

Syftet är att projektet ska kunna köras både lokalt och i en deployad miljö.

## Demo-data

Projektet ska innehålla demo-data så att applikationen är enkel att starta och testa.

Exempel:

- Produkter
- Produktvarianter
- Produktbilder
- Storlekar
- Färger
- Lagerstatus
- Testanvändare

Demo-data gör det också enklare att visa projektet i portfolio och under intervjuer.

## Första versionen

Första versionen fokuserar på kärnflödet:

**Produkt → Produktvariant → Kundvagn → Checkout → Simulerad betalning → Order**

Följande funktioner ingår inte i första versionen:

- Produktrecensioner
- Önskelista
- Avancerade kampanjer
- Riktig betalningsintegration
- Microservices

Dessa delar kan läggas till senare om kärnfunktionerna är stabila.

## README

Projektets README ska innehålla:

- Kort projektbeskrivning
- Teknikstack
- Arkitektur
- Viktiga funktioner
- Installation
- Hur projektet startas
- Hur tester körs
- Deployment
- Screenshots
- Demo-länk

README ska göra det enkelt för någon som ser projektet på GitHub att förstå vad projektet är, hur det är uppbyggt och hur det kan köras.
