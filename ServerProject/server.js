var app = require('express')();
var server = require('http').Server(app);
var io = require('socket.io')(server);

server.listen(3000);


var ConnectedClients =[];
var PlayersOnline =[];
var DiscconectTime = 30;
var Sockets="";
var InCommingConnetions = "FALSE";


var Update = function()
{
	


  for (var i = 0; i< ConnectedClients.length; i++) 
   {
   	if(ConnectedClients[i] != null)
   	{
   		//console.log("ConnectedClients[i].UsersState " +ConnectedClients[i].UsersState);
   		if(ConnectedClients[i].UsersState === "2" || ConnectedClients[i].UsersState === "1" )
   		{
 	     ConnectedClients[i].IdelTime++;
 	     //console.log("USER ID "+ConnectedClients[i].UserID+" IDEL TIME: "+ ConnectedClients[i].IdelTime);
 	    }
 	  if(ConnectedClients[i].IdelTime > DiscconectTime)
 	  {
 	  	//console.log("DISCONNECT NOTICE " +ConnectedClients[i].UserID);
 	  	ConnectedClients[i].UsersState = 0;
 	  	Sockets.emit('OnPlayerDisconnect',ConnectedClients[i]);
	    Sockets.broadcast.emit('OnPlayerDisconnect',ConnectedClients[i]);
 	  	ConnectedClients.splice(ConnectedClients.indexOf(i),1);
 	  	PlayersOnline.splice(PlayersOnline.indexOf(i),1);
 	  	InCommingConnetions="FALSE";
 	  }

 	}


 	}
//console.log("InCommingConnetions " +InCommingConnetions);
//console.log("Connected Clients: "+ConnectedClients.length);
//console.log("Players Online: "+PlayersOnline.length);
}
setInterval(function(){Update()},500);



io.on('connection',function(socket)
{

Sockets =socket; 
InCommingConnetions = "FALSE"



socket.on('OnConnection',function(data)
{
	//console.log("Data: "+ JSON.stringify(data));
 
 
      if(data.UsersState === "2")
        {


        	for (var i = 0; i< ConnectedClients.length; i++) 
                 {

                 	

                 	if(ConnectedClients[i].UserID === data.UserID)
 	                	{
 	                		ConnectedClients[i].UsersState = data.UsersState;
 	                		socket.emit('OnPlayerConnected', data);
 	                		socket.broadcast.emit('OnPlayerConnected', data);
 	                		if(InCommingConnetions ==="TRUE")
 	                		{
 	                		 // console.log("WE SEE THIS WHEN? 2 ");
 	                		 InCommingConnetions = "FALSE";
 	                		 PlayersOnline.push(data);
 	                		}
 	                		
 	                		return;
 	                	}


                 }
                 if(InCommingConnetions ==="FALSE")
                 {
	             ConnectedClients.push(data);
	             InCommingConnetions="TRUE";
	             //console.log("WE SEE THIS WHEN? ");
	             }
        }
        
	
   


     
if(data.UsersState === "1")
{


                        for (var j = 0; j< PlayersOnline.length; j++) 
 	                    {
        
                         if(PlayersOnline[j].UserID === data.UserID)
                         {
                         	// we are as our self and we are already online
                         	PlayersOnline[j].UsersState = 0;
                         	Sockets.emit('OnPlayerDisconnect',PlayersOnline[j]);
	                        Sockets.broadcast.emit('OnPlayerDisconnect',PlayersOnline[j]);
 	  	                    ConnectedClients.splice(ConnectedClients.indexOf(i),1);
 	  	                    PlayersOnline.splice(PlayersOnline.indexOf(i),1);
                         	//console.log("WE ARE LOGING IN TO A NEW DEVICE? 2 ");
                         	InCommingConnetions="FALSE";
                         	return;
                         }
 	  
 	                    }
if(InCommingConnetions ==="FALSE")
{
ConnectedClients.push(data);
socket.emit('OnPlayerConnected', data);
InCommingConnetions="TRUE";
}
}


ConnectedClients.forEach(function(data)
{
 socket.broadcast.emit('OnPlayerConnected', data);
});
 console.log("Connected Clients: "+ConnectedClients.length);
});



socket.on('OnPlayerActions',function(data)
{
 for (var i = 0; i< ConnectedClients.length; i++) 
      {
 	   if(ConnectedClients[i] != null )
 	   {
 	   	if(ConnectedClients[i].UserID === data.UserID)
 	   	{
 	   	ConnectedClients[i].IdelTime = 0;
 	    }
 	   }
     }
//socket.emit('OnPlayerActions', data);
socket.broadcast.emit('OnPlayerActions', data);
});

socket.on('OnPlayerDisconnect',function(data)
{
       for(var i =0; i<ConnectedClients.length; i++)
       {
       	if(ConnectedClients[i] != null )
 	    {
       	   if(ConnectedClients[i].UserID === data.UserID)
       	  {
       	  	ConnectedClients[i].UsersState = 0;
       	  //	console.log("DISCONNECT CLIENT " +ConnectedClients[i].UserID);
           socket.emit('OnPlayerDisconnect',data);
	       socket.broadcast.emit('OnPlayerDisconnect',data);
	       ConnectedClients.splice(ConnectedClients.indexOf(i),1);
	       PlayersOnline.splice(PlayersOnline.indexOf(i),1);
	       InCommingConnetions="FALSE";
       	  }
        }
       }
});

});