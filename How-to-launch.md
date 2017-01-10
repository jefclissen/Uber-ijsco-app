#How to launch uberijsco-app  

##Server
We hebben gebruik gemaakt van node en mongodb deze moet je dus op je computer geinstalleerd hebben om onze applicatie te kunnen runnen.  
Als je onze repository van github pulled, staat daar een mapje 'node' en 'mongo'.  
Om te onze mongodb op te zetten ga je naar de map '...\Uber-ijsco-app\mongo\bin', daar open je je cmd-venster en run je het commando "mongod --dbpath '...\Uber-ijsco-app\node\nodetest1\data'". Nu staat de database aan.
Om de node-express server aan te zetten. Ga je naar de map '...\Uber-ijsco-app\node\nodetest1', daar open je je cmd-venster en run je het commando npm start. (Je moet wel bepaalde package's installeren, namelijk express, request, polyline, validator).

Het bovenstaande is allemaal gebeurt op onze online Amazone EC2 server. Hier lopen mongodb en node op. Als je deze lokaal wilt launchen dan ga je naar de website van 'Amazone Services'. Daar kun je inloggen met de volgende credentials, email:"pim.eggermont@hotmail.com", ww:"uberijsco". Ga daarna naar 'ec2', vervolgens naar 'instances', daar zal je onze uberijsco zien staan en als je op connect druk, moet het ww nog decrypte, dit bestand zal ik jullie via mail doorsturen.

##Deliver App
Code van de werkende Deliverer app bevind zich sourcecode/Deliverer App. (deliverer-app is een mislukte versie)

Om de app te gebruiken/testen hebben we het volgende nodig:
__Een driver account__
Dit kan je tijdelijk aanmaken door naar *35.165.103.236/adddriver* het volgende data te sturen:
{
	"drivername":"robbe",
	"driveremail":"robbe@delie.be",
	"driverpassword":"ijs"
}

__Klanten__
Klanten kan je toevoegen via de User App of voor een test kan je naar *35.165.103.236/icecreamrequest* het volgende te sturen:
{
    "username":"mark",
    "email":"mark@hotmail.com",
    "userLat":"51.227192",
    "userLong":"4.415938"
}

Deze stappen moet je volgen na het opstarten van de app:

1. log in met email adres en paswoord
2. ga naar zie aanvragen
3. selecteer de klaten die je wil behandelen
4. de klanten zullen toegevoegd worden aan de kaart alsook een voordelige route
5. om een klanten te behandelen druk je op de marker, nu zal er meer informatie over de klant tevoorschijn komen. Druk daarna op de label en een activity zal openen waar meer informatie over de klant tevoorschijn zal komen. Hier is een knop voorzien om een klant te behandelen. Na het drukken van deze knop zal je terug naar de map gaan.
6. om een nieuwe klant te behandelen druk je op de marker, nu zal er meer informatie over de klant tevoorschijn komen. Druk daarna op de label en de accepteer activity zal openen. hier kan je de klant(en) aanduiden die je wil behandelen.

