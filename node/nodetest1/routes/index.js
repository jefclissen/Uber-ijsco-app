var express = require('express');
var router = express.Router();

//POSTCOMMANDS
router.post('/addclient', function(req, res) {
    
    // Set our internal DB variable
    var db = req.db;
    
    // Get our form values. These rely on the "name" attributes
    //var username = req.body.username;
    var email = req.body.usermail;
    var password = req.body.password;
    var name = req.body.name;
    var backname = req.body.backname;
    var phone = req.body.phone;
    var language = req.body.language;
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
    
    // Remove everything from db
    //collection.remove({});
    
    // Submit to the DB
    userlist_collection.insert({
        "Email": email,
        "Paswoord": password,
        "Naam" : name,
        "Achternaam": backname,
        "Phone": phone,
        "language": language,
        "Creditcard": creditcard,
        "CVC": cvc,
        "Vervaldatummaand": expirationMonth,
        "Vervaldatumjaar": expirationYear,
        "Postcode": zipcode,
    }, function (err, doc) {
        if (err) {
            // If it failed, return error
            res.send("There was a problem adding the information to the database.");
        }
        else {
            // And forward to success page
            res.redirect("userlist");
        }
    });
});
router.post('/adddriver', function(req, res) {
    
    // Set our internal DB variable
    var db = req.db;
    
    // Get our form values. These rely on the "name" attributes
    //var username = req.body.username;
    var name = req.body.name;
    var backname = req.body.backname;
    var email = req.body.usermail;
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
    userlist_collection.insert({
        "Naam" : name,
        "Achternaam": backname,
        "Email": email,
        "Phone": phone,
        "Paswoord": password,
        "Stad": city
    }, function (err, doc) {
        if (err) {
            // If it failed, return error
            res.send("There was a problem adding the information to the database.");
        }
        else {
            // And forward to success page
            res.redirect("userlist");
        }
    });
});
router.post('/icecreamrequest', function(req, res) {

    // Set our internal DB variable
    var db = req.db;

    // Get our form values. These rely on the "name" attributes
    //var user_id = req.body.ID;
    var userName = req.body.username;
    var userLong = req.body.userLong;
    var userLat = req.body.userLat;
    console.log(req.body);
    
    // Set our collection
    var unhandled_collection = db.get('unhandled_users');
    var inprogress_collection = db.get('in_progress');
    var userlist_collection = db.get('userlist');
    var driverlist_collection = db.get('driverlist');
        
    // Submit to the DB
    unhandled_collection.insert({
        //"_id" : user_id,
        "Naam": userName,
        "Longitude" : userLong,
        "Latitude" : userLat
        
    }, function (err, doc) {
        if (err) {
            // If it failed, return error
            res.send("There was a problem adding the information to the database.");
        }
        else {
            // And forward to success page
            res.redirect("userlist");
        }
    });
});
router.post('/ikwildezeklantenhelpen', function(req, res) {
    var db = req.db;
    
    var user_ID = req.body.userid;
    var userLong = req.body.userLong;
    var userLat = req.body.userLat;
    
    var unhandled_collection = db.get('unhandled_users');
    var inprogress_collection = db.get('in_progress');
    
    unhandled_collection.remove({_id: user_ID});
    
    inprogress_collection.in_progress.insert({
        "_id": user_ID,
        "Longitude": userLong,
        "Latitude": userLat
    }, function (err, doc) {
        if (err) {
            // If it failed, return error
            res.send("There was a problem adding the information to the database.");
        }
        else {
            // And forward to success page
            res.redirect("userlist");
        }
    });
});

//GETCOMMANDS
router.get('/userlist', function(req, res) {
    
    // Set our internal DB variable
    var db = req.db;
    
    // Get our form values. These rely on the "name" attributes
    var unhandled_collection = db.get('unhandled_users');
    var inprogress_collection = db.get('in_progress');
    var userlist_collection = db.get('userlist');
    var driverlist_collection = db.get('driverlist');
    
    // Select data that you want to send back
    userlist_collection.find({},{},function(e,docs){
        res.json(docs);
    });
});
router.get('/unhandledusers', function(req, res) {
    
    // Set our internal DB variable
    var db = req.db;
    
    // Get our form values. These rely on the "name" attributes
    var unhandled_collection = db.get('unhandled_users');
    var inprogress_collection = db.get('in_progress');
    var userlist_collection = db.get('userlist');
    var driverlist_collection = db.get('driverlist');
    
    // Select data that you want to send back
    unhandled_collection.find({},{},function(e,docs){
        res.json(docs);
    });
});
router.get('/emptycollections', function(req, res) {
    
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
router.get('/showall', function(req, res) {
    
    // Set our internal DB variable
    var db = req.db;
    // Get our form values. These rely on the "name" attributes
    var unhandled_collection = db.get('unhandled_users');
    var inprogress_collection = db.get('in_progress');
    var userlist_collection = db.get('userlist');
    var driverlist_collection = db.get('driverlist');
    
    res.send("Unhandled collection");
    unhandled_collection.find({},{},function(e,docs){
        res.json(docs);
    });
    res.send("Inprogress collection");
    inprogress_collection.find({},{},function(e,docs){
        res.json(docs);
    });
    res.send("Userlist collection");
    userlist_collection.find({},{},function(e,docs){
        res.json(docs);
    });
    res.send("Driverlist collection");
    driverlist_collection.find({},{},function(e,docs){
        res.json(docs);
    });
});


module.exports = router;