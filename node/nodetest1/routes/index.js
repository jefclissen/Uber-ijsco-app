var express = require('express');
var request = require("request");
var polyline = require("polyline");
var validator = require('validator');
var router = express.Router();

//POSTCOMMANDS
router.get('/', function (req, res) {
    var db = req.db;
    db.createCollection("unhandled_users");
    db.createCollection("in_progress");
    db.createCollection("userlist");
    db.createCollection("driverlist");
    res.send("ok");
});
router.post('/addclient', function (req, res) {

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
        "Email": email
    }, {}, function (e, docs) {
        // Check if email is valid
        if (validator.isEmail(email) == false) {
            console.log("Email niet geldig");
            res.send("0Email is niet geldig");
        } else //Check if email allready exist
        if (docs != "") {
            console.log("Deze email bestaat al");
            res.send("0Deze email bestaat al");
        } else // Submit to the DB
        {
            userlist_collection.insert({
                "Username": username,
                "Email": email,
                "Password": password,
                "Phone": "",
                "Creditcard": "",
                "CVC": "",
                "ExpirationMonth": "",
                "ExpirationDay": "",
                "Zipcode": ""
            }, function (err, doc) {
                if (err) {
                    res.send("There was a problem adding the information to the database.");
                }
            });
            res.send("1Account aangemaakt");
        }
    });
});
router.post('/addcredentials', function (req, res) {
    var db = req.db;

    var email = req.body.email;
    var phone = req.body.phone;
    var creditcard = req.body.cardnr;
    var cvc = req.body.cvc;
    var expirationMonth = req.body.expirationMonth;
    var expirationYear = req.body.expirationYear;
    var zipcode = req.body.zipcode;

    // Set our collection
    var unhandled_collection = db.get('unhandled_users');
    var inprogress_collection = db.get('in_progress');
    var userlist_collection = db.get('userlist');
    var driverlist_collection = db.get('driverlist');
    userlist_collection.update({
        "Email": email
    }, {
        $set: {
            "Phone": phone,
            "Creditcard": creditcard,
            "CVC": cvc,
            "ExpirationMonth": expirationMonth,
            "ExpirationDay": expirationYear,
            "Zipcode": zipcode
        }
    });
    res.send("ja");
});
router.post('/adddriver', function (req, res) {

    // Set our internal DB variable
    var db = req.db;

    // Get our form values. These rely on the "name" attributes
    //var username = req.body.username;
    var name = req.body.name;
    var lastname = req.body.lastname;
    var email = req.body.email;
    var phone = req.body.phone;
    var password = req.body.password;
    var city = req.body.city;

    // Set our collection
    var unhandled_collection = db.get('unhandled_users');
    var inprogress_collection = db.get('in_progress');
    var userlist_collection = db.get('userlist');
    var driverlist_collection = db.get('driverlist');

    // Remove everything from db
    //collection.remove({});

    // Submit to the DB
    driverlist_collection.insert({
        "Name": name,
        "Lastname": lastname,
        "Email": email,
        "Phone": phone,
        "Password": password,
        "City": city
    }, function (err, doc) {
        if (err) {
            // If it failed, return error
            res.send("There was a problem adding the information to the database.");
        } else {
            // And forward to success page
            res.redirect("userlist");
        }
    });
});
router.post('/icecreamrequest', function (req, res) {
    // Set our internal DB variable
    var db = req.db;

    // Get our form values. These rely on the "name" attributes
    var email = req.body.email;
    var username = req.body.username;
    var userlong = req.body.userLong;
    var userlat = req.body.userLat;

    // Set our collection
    var unhandled_collection = db.get('unhandled_users');
    var inprogress_collection = db.get('in_progress');
    var userlist_collection = db.get('userlist');
    var driverlist_collection = db.get('driverlist');
    //Find user by email
    unhandled_collection.find({
        "Email": email
    }, {}, function (e, docs) {
        if (docs != "") {
            console.log("U hebt al een verzoek ingediend");
            res.send("0U hebt al een verzoek ingediend");
        } else // Submit to the DB
        {
            res.send("1Your request is being handled!");
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
    var useremail = [];
    var userLong = [];
    var userLat = [];
    var eta = [];
    var db = req.db;

    var driveremail = req.body.driveremail;
    var driverLoc = req.body.driverLat + "," + req.body.driverLong;

    var unhandled_collection = db.get('unhandled_users');
    var inprogress_collection = db.get('in_progress');
    var userlist_collection = db.get('userlist');
    var driverlist_collection = db.get('driverlist');

    for (i = 0; i < req.body.users.length; i++) {
        useremail.push(req.body.users[i].useremail);
        userLong.push(req.body.users[i].userLong);
        userLat.push(req.body.users[i].userLat);

        unhandled_collection.remove({
            "Email": useremail[i]
        });

        inprogress_collection.insert({
            "Email": useremail[i],
            "Longitude": userLong[i],
            "Latitude": userLat[i],
            "DriverEmail": driveremail,
        }, function (err, doc) {
            if (err) {
                res.send("There was a problem adding the information to the database.");
            }
        });
    }

    inprogress_collection.find({
        "DriverEmail": driveremail
    }, {}, function (e, docs) {
        var locs = [];
        var locClients = [];
        for (i = 0; i < docs.length; i++) {
            var string = docs[i].Latitude + "," + docs[i].Longitude;
            locs.push(string);
            locClients.push({
                "Latitude": docs[i].Latitude,
                "Longitude": docs[i].Longitude
            })
        }
        index = determineFarthest(locClients, req.body.driverLong, req.body.driverLat);
        var destination = locs[index];
        locs.splice(index, 1);
        var url = "https://maps.googleapis.com/maps/api/directions/json?origin=" + driverLoc + "&destination=" + destination + "&waypoints=optimize:true|";
        for (i = 0; i < locs.length; i++) {

            url = url + "" + locs[i] + "|";
        }

        url = url + "&key=AIzaSyB-2-VU9vdv6a2ACDSxpbuzlUDgu8HQjDE";
        request(url, function (error, responds, body) {
            if (error) console.log(error);
            else {
                var json = [];
                var obj = JSON.parse(body);
                legs = obj.routes[0].legs;
                legs_length = obj.routes[0].legs.length;
                for (i = 0; i < legs_length; i++) {
                    var data = new Object();
                    data.url = url;
                    data.distance = legs[i].distance.text;
                    data.duration = legs[i].duration.text;
                    eta.push(legs[i].duration.value);
                    data.end_address = legs[i].end_address;
                    data.end_address_lat = legs[i].end_location.lat;
                    data.end_address_long = legs[i].end_location.long;
                    data.start_address = legs[i].start_address;
                    data.start_address_lat = legs[i].start_location.lat;
                    data.start_address_long = legs[i].start_location.long;
                    data.steps = [];
                    for (j = 0; j < legs[i].steps.length; j++) {
                        var poly = [];
                        poly[j] = polyline.decode(legs[i].steps[j].polyline.points);
                        for (m = 0; m < poly[j].length; m++) {
                            data.steps.push({
                                "lat": poly[j][m][0],
                                "long": poly[j][m][1]
                            });
                        }
                    }
                    json.push(data);
                }
                res.send(json);
            }
            for (i = 0; i < useremail.length; i++) {
                inprogress_collection.update({
                    "Email": useremail[i]
                }, {
                    $set: {
                        "Eta": eta[i]
                    }
                });
            }
        });

    });



    function determineFarthest(locClients, longD, latD) {

        var farthest = 0;
        var index = 0;
        var d = 0;
        for (var i = 0; i < locClients.length; i++) { //Calculate lenght of every accepted client and check wich one if the farthest
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

    function deg2rad(deg) {
        return deg * (Math.PI / 180)
    }


});
router.post('/doneclient', function (req, res) {
    var db = req.db;

    var email = req.body.email;

    var unhandled_collection = db.get('unhandled_users');
    var inprogress_collection = db.get('in_progress');
    var userlist_collection = db.get('userlist');
    var driverlist_collection = db.get('driverlist');

    inprogress_collection.remove({
        "Email": email
    });
    res.send("Gebruiker afgehandeld");
});
router.post('/clientlogin', function (req, res) {
    var db = req.db;

    var email = req.body.email;
    var password = req.body.password;

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
router.post('/getinfo', function (req, res) {
    var email = req.body.useremail
        // Set our internal DB variable
    var db = req.db;

    // Get our form values. These rely on the "name" attributes
    var unhandled_collection = db.get('unhandled_users');
    var inprogress_collection = db.get('in_progress');
    var userlist_collection = db.get('userlist');
    var driverlist_collection = db.get('driverlist');

    // Select data that you want to send back
    res.send("iets");
});

// GETCOMMANDS
router.get('/userlist', function (req, res) {
    // Set our internal DB variable
    var db = req.db;

    // Get our form values. These rely on the "name" attributes
    var unhandled_collection = db.get('unhandled_users');
    var inprogress_collection = db.get('in_progress');
    var userlist_collection = db.get('userlist');
    var driverlist_collection = db.get('driverlist');

    // Select data that you want to send back
    userlist_collection.find({}, {}, function (e, docs) {
        res.json(docs);
    });
});
router.get('/unhandledclients', function (req, res) {

    // Set our internal DB variable
    var db = req.db;

    // Get our form values. These rely on the "name" attributes
    var unhandled_collection = db.get('unhandled_users');
    var inprogress_collection = db.get('in_progress');
    var userlist_collection = db.get('userlist');
    var driverlist_collection = db.get('driverlist');

    // Select data that you want to send back
    unhandled_collection.find({}, {}, function (e, docs) {
        res.json(docs);
    });
});
router.get('/emptycollections', function (req, res) {

    var db = req.db;

    var unhandled_collection = db.get('unhandled_users');
    var inprogress_collection = db.get('in_progress');
    var userlist_collection = db.get('userlist');
    var driverlist_collection = db.get('driverlist');


    unhandled_collection.remove({});
    inprogress_collection.remove({});
    userlist_collection.remove({});
    driverlist_collection.remove({});

    res.send("CLEARED MOTHGERFUCKERD")
});
router.get('/inprogress', function (req, res) {
    var db = req.db;

    var unhandled_collection = db.get('unhandled_users');
    var inprogress_collection = db.get('in_progress');
    var userlist_collection = db.get('userlist');
    var driverlist_collection = db.get('driverlist');

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

    res.send("Unhandled collection");
    unhandled_collection.find({}, {}, function (e, docs) {
        res.json(docs);
    });
    res.send("Inprogress collection");
    inprogress_collection.find({}, {}, function (e, docs) {
        res.json(docs);
    });
    res.send("Userlist collection");
    userlist_collection.find({}, {}, function (e, docs) {
        res.json(docs);
    });
    res.send("Driverlist collection");
    driverlist_collection.find({}, {}, function (e, docs) {
        res.json(docs);
    });
});

module.exports = router;