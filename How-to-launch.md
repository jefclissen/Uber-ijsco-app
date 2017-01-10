#How to launch uberijsco-app  

We hebben gebruik gemaakt van node en mongodb deze moet je dus op je computer geinstalleerd hebben om onze applicatie te kunnen runnen.  
Als je onze repository van github pulled, staat daar een mapje 'node' en 'mongo'.  
Om te onze mongodb op te zetten ga je naar de map '...\Uber-ijsco-app\mongo\bin', daar open je je cmd-venster en run je het commando "mongod --dbpath '...\Uber-ijsco-app\node\nodetest1\data'". Nu staat de database aan.
Om de node-express server aan te zetten. Ga je naar de map '...\Uber-ijsco-app\node\nodetest1', daar open je je cmd-venster en run je het commando npm start. (Je moet wel bepaalde package's installeren, namelijk express, request, polyline, validator).

Het bovenstaande is allemaal gebeurt op onze online Amazone EC2 server. Hier lopen mongodb en node op. Als je deze lokaal wilt launchen dan ga je naar de website van 'Amazone Services'. Daar kun je inloggen met de volgende credentials, email:"pim.eggermont@hotmail.com", ww:"uberijsco". Ga daarna naar 'ec2', vervolgens naar 'instances', daar zal je onze uberijsco zien staan en als je op connect druk, moet het ww nog decrypte, dit bestand zal ik jullie via mail doorsturen.