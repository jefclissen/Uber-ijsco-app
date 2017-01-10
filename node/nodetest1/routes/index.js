var express = require('express');
var request = require("request");
var polyline = require("polyline");
var validator = require('validator');
var router = express.Router();
var debug = true;
// Als ik een *+bericht stuur wordt de client treads beëindigd

//POSTCOMMANDS
router.post('/addclient', function (req, res) {
    if(debug == true){
        console.log(req.body);
    }
    // Set our internal DB variable
    var db = req.db;

    // Get our form values. These rely on the "name" attributes
    var username = req.body.username;
    var email = req.body.email;
    var password = req.body.password;

    // Set our collection
    var unhandled_collection = db.get('unhandled_users');
    var inprogress_collection = db.get('in_progress');
    var userlist_collection = db.get('userlist');
    var driverlist_collection = db.get('driverlist');

    // Search email in userlist
    userlist_collection.find({
        "Username":username
    },{},function(e, docs){
        if(docs != ""){
            res.send("0This username is allready in use");
        }else{
            userlist_collection.find({
                "Email": email
            }, {}, function (e, docs) {
                // Check if email is valid
                if (validator.isEmail(email) == false) {
                    console.log("Email not valid");
                    res.send("0Email not valid");
                } else // Check if email allready exist
                if (docs != "") {
                    console.log("This email allready exists");
                    res.send("0This email allready exists");
                } else // Submit to the DB
                {
                    userlist_collection.insert({
                        "Username": username,
                        "Email": email,
                        "Password": password,
                        "Phone": "",
                        "CardHolder": "",
                        "Creditcard": "",
                        "CVC": "",
                        "ExpirationDate": "",
                        "Zipcode": ""
                    }, function (err, doc) {
                        if (err) {
                            res.send("There was a problem adding the information to the database.");
                        }
                    });
                    res.send("1Account created");
                }
            });
        }
    });    
});
router.post('/updatecredentials', function (req, res) {
    if(debug == true){
        console.log(req.body);
    }
    var db = req.db;
    var oldpassword = req.body.oldpassword;
    var newpassword = req.body.newpassword;
    var useremail = req.body.useremail;
    var phone = req.body.phone;
    var cardholder = req.body.cardholder;
    var creditcard = req.body.cardnr;
    var cvc = req.body.cvc;
    var expirationdate = req.body.expirationdate;
    var zipcode = req.body.zipcode;

    // Set our collection
    var userlist_collection = db.get('userlist');
    // Update userdata
    userlist_collection.update({
        "Email": useremail,
        "Password": oldpassword
    }, {
        $set: {
            "Password": newpassword,
            "Phone": phone,
            "CardHolder": cardholder,
            "Creditcard": creditcard,
            "CVC": cvc,
            "ExpirationDate": expirationdate,
            "Zipcode": zipcode
        }
    });
    res.send("1Credentials aangepast");
});
router.post('/adddriver', function (req, res) {
    if(debug == true){
        console.log(req.body);
    }
    // Set our internal DB variable
    var db = req.db;

    // Set variables
    var username = req.body.drivername;
    var email = req.body.driveremail;
    var password = req.body.driverpassword;


    // Set our collection
    var driverlist_collection = db.get('driverlist');

    // Submit to the DB
    
    driverlist_collection.find({
        "DriverName": username,
        "DriverEmail": email,
    }, function (err, docs) {
        if (validator.isEmail(email) == false) {
            console.log("Email not valid");
            res.send("0Email not valid");
        } else // Check if email allready exist
        if (docs != "") {
            console.log("This email already exists");
            res.send("0This email already exists");
        } else // Submit to the DB
        {
            driverlist_collection.insert({
                "DriverName": username,
                "DriverEmail": email,
                "DriverPassword": password
            }, function (err, doc) {
                if (err) {
                    res.send("0There was a problem adding the information to the database.");
                }
            });
            res.send("1Account created");
        }
    });


});
router.post('/icecreamrequest', function (req, res) {
    if(debug == true){
        console.log(req.body);
    }
    // Set our internal DB variable
    var db = req.db;

    // Set variables
    var email = req.body.email;
    var userlong = req.body.userLong;
    var userlat = req.body.userLat;
    var username = req.body.username;

    // Set our collection
    var unhandled_collection = db.get('unhandled_users');
    var userlist_collection = db.get('userlist');

    userlist_collection.find({
        "Email": email
    }, {}, function (e, docs) {
        if(docs != ""){
            username = docs[0].Username;
        }
    });

    //Find user by email
    unhandled_collection.find({
        "Email": email
    }, {}, function (e, docs) {
        if (docs != "") {
            console.log("You allready send a request");
            res.send("You allready send a request");
        } else // Submit to the DB
        {
            res.send("Your request is being handled!");
            unhandled_collection.insert({
                "Username": username,
                "Email": email,
                "Longitude": userlong,
                "Latitude": userlat
            }, function (err, doc) {
                if (err) {
                    res.send("There was a problem adding the information to the database.");
                }
            });
        }
    });
});
router.post('/helpclient', function (req, res) {
    if(debug == true){
        console.log(req.body);
    }
    // Set our internal DB variable
    var db = req.db;
    
    // Set variables
    var driveremail = req.body.driveremail;
    var driverLoc = req.body.driverLat + "," + req.body.driverLong;

    var useremail = [];
    var userLong = [];
    var userLat = [];
    var eta = [];
    var user_volgorde = [];
    var volgorde;
    var totaal = 0;

    // Set our collections
    var unhandled_collection = db.get('unhandled_users');
    var inprogress_collection = db.get('in_progress');

    // Insert all the users in the collection in_progress and delete them from unhandled_users
    for (i = 0; i < req.body.users.length; i++) {
        inprogress_collection.insert({
            "Email": req.body.users[i].useremail,
            "Longitude": req.body.users[i].userLong,
            "Latitude": req.body.users[i].userLat,
            "DriverEmail": driveremail,
            "Eta": "0"
        }, function (err, doc) {
            if (err) {
                res.send("There was a problem adding the information to the database.");
            }
        });

        unhandled_collection.remove({
            "Email": req.body.users[i].useremail
        });
    }

    // Get all users that the driver needs to serve, put them in arrays & calculate route with google directions API
    function myFunction() {
        inprogress_collection.find({
            "DriverEmail": driveremail
        }, {}, function (e, docs) {
            var DestIndex;
            var locs = [];
            var locClients = [];
            for (i = 0; i < docs.length; i++) {
                useremail.push(docs[i].Email);
                userLong.push(docs[i].Longitude);
                userLat.push(docs[i].Latitude);

                var string = docs[i].Latitude + "," + docs[i].Longitude; //long en lat in string met komma zette zodat ik ze kan pushe in de google url
                locs.push(string);
                locClients.push({ //een andere array maken met long en lat apart zodat ik die kan gebruiken in mijn functie determinateFarthest()
                    "Latitude": docs[i].Latitude,
                    "Longitude": docs[i].Longitude
                })
            }
            DestIndex = determineFarthest(locClients, req.body.driverLong, req.body.driverLat);
            var destination = locs[DestIndex];
            locs.splice(DestIndex, 1); //destination verwijderen zodat die niet mee in de url wordt gepush later
            var url = "https://maps.googleapis.com/maps/api/directions/json?origin=" + driverLoc + "&destination=" + destination + "&waypoints=optimize:true|";
            for (i = 0; i < locs.length; i++) { //push the waypoints in the url

                url = url + "" + locs[i] + "|";
            }

            url = url + "&key=AIzaSyB-2-VU9vdv6a2ACDSxpbuzlUDgu8HQjDE";

            request(url, function (error, responds, body) { //http request naar google me de url die ik eerder heb gemaakt
                if (error) console.log(error);
                else {
                    var json = [];
                    var obj = JSON.parse(body);
                     //body die ik binnenkrijg omzetten naar JSON
                    //if(obj.routes[0].waypoint_order != ""){
                       volgorde = obj.routes[0].waypoint_order; //in de google responds zit een waypoint_order, deze geeft weer welke klant op welke plek in het rijtje staat. 
                    //}
                    legs = obj.routes[0].legs;
                    legs_length = obj.routes[0].legs.length;
                    for (i = 0; i < legs_length; i++) {
                        var data = new Object();
                        //belangrijke data in een object data steken en terugsture
                        data.distance = legs[i].distance.text;
                        data.duration = legs[i].duration.text;
                        data.end_address = legs[i].end_address;
                        data.end_address_lat = legs[i].end_location.lat;
                        data.end_address_long = legs[i].end_location.lng;
                        data.start_address = legs[i].start_address;
                        data.start_address_lat = legs[i].start_location.lat;
                        data.start_address_long = legs[i].start_location.lng;
                        data.steps = [];
                        //polylines omvormen en in array steken
                        for (j = 0; j < legs[i].steps.length; j++) {
                            var poly = [];
                            poly[j] = polyline.decode(legs[i].steps[j].polyline.points);
                            for (m = 0; m < poly[j].length; m++) {
                                data.steps.push({
                                    "Latitude": poly[j][m][0],
                                    "Longitude": poly[j][m][1]
                                });
                            }
                        }
                        eta.push(legs[i].duration.value);
                        json.push(data);
                    }
                    res.send(json);
                }

                for (i = 0; i < volgorde.length; i++) { //de locaties in de juiste volgorde zette
                    var index = volgorde[i];
                    user_volgorde.push(locs[index]);
                }

                user_volgorde.push(destination); //destination toevoegen
                locs.splice(DestIndex, 0, destination); //destination toevoegen op plaats 0

                var email_volgorde = [];
                for (i = 0; i < user_volgorde.length; i++) { //user_volgorde en locs vergelijken en dan de email van de klant in de juiste volgorde zetten
                    for (j = 0; j < locs.length; j++) {
                        if (user_volgorde[i] == locs[j]) {
                            email_volgorde.push(useremail[j]);
                        }
                    }
                }
                for (i = 0; i < email_volgorde.length; i++) { //eta[] bij de juiste gebruiker in de collection in_progress zetten
                    totaal = totaal + eta[i];
                    inprogress_collection.update({
                        "Email": email_volgorde[i]
                    }, {
                        $set: {
                            "Eta": totaal
                        }
                    });
                }
            });

        });
    }
    setTimeout(myFunction, 2000); //myFunction met een delay aanroepen omdat problemen ontstaan met de database 
    function determineFarthest(locClients, longD, latD) { //functie die berekent welke geaccepteerde gebruiker er het verste van de driver is
        var farthest = 0;
        var index = 0;
        var d = 0;
        for (var i = 0; i < locClients.length; i++) {
            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(locClients[i].Latitude - latD);
            var dLon = deg2rad(locClients[i].Longitude - longD);
            var a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
                Math.cos(deg2rad(latD)) * Math.cos(deg2rad(locClients[i].Latitude)) *
                Math.sin(dLon / 2) * Math.sin(dLon / 2);
            var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
            var d = R * c;
            if (d > farthest) {
                farthest = d;
                index = i;
            }

        }
        return index;
    }

    function deg2rad(deg) { //Function to go from degrees to radians
        return deg * (Math.PI / 180)
    }
});
router.post('/doneclient', function (req, res) {
    console.log(req.body);
    if(debug == true){
        console.log(req.body);
    }
    // Set our internal DB variable
    var db = req.db;
    // Set variables
    var email = req.body.useremail;
    var driveremail = req.body.driveremail;
    var driverLoc = req.body.driverLat + "," + req.body.driverLong;
    var useremail = [];
    var userLong = [];
    var userLat = [];
    var eta = [];
    var user_volgorde = []; 
    var totaal=0;

    // Set our collections
    var inprogress_collection = db.get('in_progress');

    inprogress_collection.remove({
        "Email": email
    });

    // Get all users that the driver needs to serve, put them in arrays & calculate route with google directions API
    // Nieuwe route doorsturen als er een klant verwijderd is, zodat de vorige route niet op de kaart blijft staan
    function myFunction() {
        inprogress_collection.find({
            "DriverEmail": driveremail
        }, {}, function (e, docs) {
            if(docs == ""){
                res.send("Er zijn geen klanten meer")
            }else{
                var DestIndex;
                var locs = [];
                var locClients = [];
                
                for (i = 0; i < docs.length; i++) {
                    useremail.push(docs[i].Email);
                    userLong.push(docs[i].Longitude);
                    userLat.push(docs[i].Latitude);

                    var string = docs[i].Latitude + "," + docs[i].Longitude; //long en lat in string met komma zette zodat ik ze kan pushe in de google url
                    locs.push(string);
                    locClients.push({ //een andere array maken me long en lat appart zodat ik die kan gebruiken in mijn functie determinateFarthest()
                        "Latitude": docs[i].Latitude,
                        "Longitude": docs[i].Longitude
                    })
                }
                DestIndex = determineFarthest(locClients, req.body.driverLong, req.body.driverLat);
                var destination = locs[DestIndex];
                locs.splice(DestIndex, 1); //destination verwijderen zodat die niet mee in de url word gepush later
                var url = "https://maps.googleapis.com/maps/api/directions/json?origin=" + driverLoc + "&destination=" + destination + "&waypoints=optimize:true|";
                for (i = 0; i < locs.length; i++) { //push the waypoints in the url

                    url = url + "" + locs[i] + "|";
                }

                url = url + "&key=AIzaSyB-2-VU9vdv6a2ACDSxpbuzlUDgu8HQjDE";

                request(url, function (error, responds, body) { //hhtp request naar google me de url die ik ervoor heb gemaakt
                    if (error) console.log(error);
                    else {
                        var json = [];
                        var obj = JSON.parse(body); //body die ik binnenkrijg omzetten naar JSON
                        volgorde = obj.routes[0].waypoint_order; //in de google responds zit een waypoint_order, deze geeft weer welke klant op welke plek in het rijtje staat.
                        legs = obj.routes[0].legs;
                        legs_length = obj.routes[0].legs.length;
                        for (i = 0; i < legs_length; i++) {
                            var data = new Object();
                            //belangrijke data in een object data steken en terugsture
                            data.distance = legs[i].distance.text;
                            data.duration = legs[i].duration.text;
                            data.end_address = legs[i].end_address;
                            data.end_address_lat = legs[i].end_location.lat;
                            data.end_address_long = legs[i].end_location.lng;
                            data.start_address = legs[i].start_address;
                            data.start_address_lat = legs[i].start_location.lat;
                            data.start_address_long = legs[i].start_location.lng;
                            data.steps = [];
                            //polylines omvormen en in array steken
                            for (j = 0; j < legs[i].steps.length; j++) {
                                var poly = [];
                                poly[j] = polyline.decode(legs[i].steps[j].polyline.points);
                                for (m = 0; m < poly[j].length; m++) {
                                    data.steps.push({
                                        "Latitude": poly[j][m][0],
                                        "Longitude": poly[j][m][1]
                                    });
                                }
                            }
                            eta.push(legs[i].duration.value);
                            json.push(data);
                        }
                        res.send(json);
                    }

                    for (i = 0; i < volgorde.length; i++) { //de locaties in de juiste volgorde zette
                        var index = volgorde[i];
                        user_volgorde.push(locs[index]);
                    }

                    user_volgorde.push(destination); //destination toevoegen
                    locs.splice(DestIndex, 0, destination); //destination toevoegen op plaats 0

                    var email_volgorde = [];
                    for (i = 0; i < user_volgorde.length; i++) { //user_volgorde en locs vergelijken en dan de email van de klant in de juiste volgorde zetten
                        for (j = 0; j < locs.length; j++) {
                            if (user_volgorde[i] == locs[j]) {
                                email_volgorde.push(useremail[j]);
                            }
                        }
                    }
                    for (i = 0; i < email_volgorde.length; i++) { //eta[] bij de juiste gebruiker in de collection in_progress zetten
                        totaal = totaal + eta[i];
                        inprogress_collection.update({
                            "Email": email_volgorde[i]
                        }, {
                            $set: {
                                "Eta": totaal
                            }
                        });
                    }
                });
            }
        });
    }
    
    setTimeout(myFunction, 2000); //myFunction met een delay aanroepen omdat problemen ontstaan met de database 
    
    function determineFarthest(locClients, longD, latD) { //functie die berekent welke geaccepteerde gebruiker er het verste van de driver is
        var farthest = 0;
        var index = 0;
        var d = 0;
        for (var i = 0; i < locClients.length; i++) {
            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad(locClients[i].Latitude - latD);
            var dLon = deg2rad(locClients[i].Longitude - longD);
            var a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
                Math.cos(deg2rad(latD)) * Math.cos(deg2rad(locClients[i].Latitude)) *
                Math.sin(dLon / 2) * Math.sin(dLon / 2);
            var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
            var d = R * c;
            if (d > farthest) {
                farthest = d;
                index = i;
            }

        }
        return index;
    }

    function deg2rad(deg) { //Function to go from degrees to radians
        return deg * (Math.PI / 180)
    }
});
router.post('/checkpassword', function (req, res) {
    if(debug == true){
        console.log(req.body);
    }
    // Set our internal DB variable
    var db = req.db;

    // Set variables
    var email = req.body.email;
    var password = req.body.password;

    // Set our collections
    var userlist_collection = db.get('userlist');

    userlist_collection.find({
        "Email": email,
        "Password": password
    }, {}, function (e, docs) {
        if (docs != "") {
            res.send("1Valid");
        } else {
            res.send("0Invalid");
        }
    });
});
router.post('/geteta', function (req, res) {
    if(debug == true){
        console.log(req.body);
    }
    // Set our internal DB variable
    var db = req.db;

    // Set variables
    var useremail = req.body.useremail;

    // Set our collections
    var inprogress_collection = db.get('in_progress');
    var unhandled_collection = db.get('unhandled_users');

    // Select data that you want to send back
    inprogress_collection.find({
        "Email": useremail
    }, {}, function (e, docs) {
        if (docs != "") {
            res.send((docs[0].Eta).toString());
        } else {
            unhandled_collection.find({
                "Email": useremail
            }, {}, function (e, docs) {
                if (docs != "") {
                    res.send("0In progress ...")
                } else {
                    res.send("*Uw verzoek is afgehandeld")
                }
            });
        }
    });
});
router.post('/getcredentials', function (req, res) {
    if(debug == true){
        console.log(req.body);
    }
    // Set our internal DB variable
    var db = req.db;

    // Set variables
    var useremail = req.body.useremail;
    var password = req.body.password;
    // Set our collections
    var unhandled_collection = db.get('unhandled_users');
    var inprogress_collection = db.get('in_progress');
    var userlist_collection = db.get('userlist');
    var driverlist_collection = db.get('driverlist');

    // Select data that you want to send back
    userlist_collection.find({
        "Email": useremail
    }, {}, function (e, docs) {
        if (docs != "") {
            res.send(docs);
        } else {
            res.send("not valid");

        }
    });
});
router.post('/driverlogin',function(req,res){
    // Set our internal DB variable
    var db = req.db;

    // Set variables
    var email = req.body.driveremail;
    var password = req.body.driverpassword;

    // Set our collections
    var driverlist_collection = db.get('driverlist');

    driverlist_collection.find({
        "Email": email,
        "Password": password
    }, {}, function (e, docs) {
        if (docs != "") {
            res.send("1Valid");
        } else {
            res.send("0Invalid");
        }
    });
})

// GETCOMMANDS
router.get('/userlist', function (req, res) {
    // Set our internal DB variable
    var db = req.db;

    // Set our collection
    var userlist_collection = db.get('userlist');

    userlist_collection.find({}, {}, function (e, docs) {
        res.json(docs);
    });
});
router.get('/driverlist', function (req, res) {
    // Set our internal DB variable
    var db = req.db;

    // Set our collection
    var driverlist_collection = db.get('driverlist');

    driverlist_collection.find({}, {}, function (e, docs) {
        res.json(docs);
    });
});
router.get('/unhandledclients', function (req, res) {
    // Set our internal DB variable
    var db = req.db;

    // Set our collection
    var unhandled_collection = db.get('unhandled_users');

    unhandled_collection.find({}, {}, function (e, docs) {
        res.json(docs);
    });
});
router.get('/emptycollections', function (req, res) {
    // Set our internal DB variable
    var db = req.db;

    // Set our collection
    var unhandled_collection = db.get('unhandled_users');
    var inprogress_collection = db.get('in_progress');
    var userlist_collection = db.get('userlist');
    var driverlist_collection = db.get('driverlist');

    unhandled_collection.remove({});
    inprogress_collection.remove({});
    userlist_collection.remove({});
    driverlist_collection.remove({});

    userlist_collection.insert({
        "Username": "user",
        "Email": "user@user.be",
        "Password": "123",
        "Phone": "",
        "CardHolder": "",
        "Creditcard": "",
        "CVC": "",
        "ExpirationDate": "",
        "Zipcode": ""
    }, function (err, doc) {
        if (err) {
            res.send("There was a problem adding the information to the database.");
        }
    });
    res.send("COLLECTIONS CLEARED!")
});
router.get('/inprogress', function (req, res) {
    // Set our internal DB variable
    var db = req.db;

    // Set our collection
    var inprogress_collection = db.get('in_progress');

    inprogress_collection.find({}, {}, function (e, docs) {
        res.json(docs);
    });
});
router.get('/showall', function (req, res) {

    // Set our internal DB variable
    var db = req.db;
    // Get our form values. These rely on the "name" attributes
    var unhandled_collection = db.get('unhandled_users');
    var inprogress_collection = db.get('in_progress');
    var userlist_collection = db.get('userlist');
    var driverlist_collection = db.get('driverlist');

    var data = [];
    unhandled_collection.find({}, {}, function (e, docs) {
        data.push(docs);
    });
    inprogress_collection.find({}, {}, function (e, docs) {
        data.push(docs);
    });
    userlist_collection.find({}, {}, function (e, docs) {
        data.push(docs);
    });
    driverlist_collection.find({}, {}, function (e, docs) {
        data.push(docs);
    });
    res.send(data);

});

module.exports = router;