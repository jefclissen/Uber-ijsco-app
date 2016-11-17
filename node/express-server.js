var express = require("express");
var bodyparser = require("body-parser");
var app = express();
app.use(bodyparser.json());

app.use(function(req, res, next) {
  res.header("Access-Control-Allow-Origin", "*");
  res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
  next();
});

var path = require('path');
app.use(express.static(__dirname + '/../'));

app.get("/", function(req, res) {
	var personen = [{
        "naam":"Eggermont",
        "voornaam":"Pim"
    },
    {
        "naam":"Jef",
        "voornaam":"Clissen"
    }];

    res.json(personen); //SEND ARRAY
});

app.get("/data",function(req,res){
        //res.send(200,"toon alle gebruikers");
    var clients = 
        [{"name":"Jef",
                    "currentLocation":
                        {"Latitude":"156498652",
                         "Longitude":"641564145"
                        }
         },
        {"name":"Pim",
                    "currentLocation":
                        {"Latitude":"645324",
                         "Longitude":"34345345"
                        }
         },
         {"name":"Jan",
                    "currentLocation":
                        {"Latitude":"111111",
                         "Longitude":"22"
                        }
         },
         {"name":"Lobbe",
                    "currentLocation":
                        {"Latitude":"456",
                         "Longitude":"456"
                        }
         }
        ];

    res.json(clients); //SEND ARRAY
    
});

app.listen(3000);