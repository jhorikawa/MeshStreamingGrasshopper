var receivedData

// 1.Initialize Module
var fs = require("fs");
var server = require("http").createServer(function(req, res) {
     res.writeHead(200, {"Content-Type":"text/html"});
     var output = fs.readFileSync("./index.html", "utf-8");
     res.end(output);
}).listen(8080);
var io = require("socket.io").listen(server);

// User Hash
var userHash = {};

// 2.Event Definition
io.sockets.on("connection", function (socket) {

  // On Connected Custom Event (Save connected user and notify others)
  socket.on("connected", function (name) {
    var msg = name + " is connected.";
    userHash[socket.id] = name;
    io.sockets.emit("publish", {value: msg});
    console.log(msg);
  });

  // Custom Event to Send Message
  socket.on("publish", function (data) {
    console.log("published");
    io.sockets.emit("publish", {value:data.value});
  });

  // On Disconnected Event(Delete connected user and notify others)
  socket.on("disconnect", function () {
    if (userHash[socket.id]) {
      var msg = userHash[socket.id] + " is disconnected.";
      delete userHash[socket.id];
      io.sockets.emit("publish", {value: msg});
      console.log(msg);

      //io.sockets.emit("unity",receivedData);
    }
  });

  socket.on("gh", function (data) {
    console.log(data.length);
    //console.log("data received: " + data.mesh.length);//data.mesh);
    //receivedData = data;
    //io.sockets.emit("unity",{"mesh":data.mesh.toString("base64")});

  });

});
