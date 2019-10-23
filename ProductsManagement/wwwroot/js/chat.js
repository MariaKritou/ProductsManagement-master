
"use strict";

//var connection = new signalR.HubConnectionBuilder().withUrl(baseUrl+"/chatHub").build();

//Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage",function (user, message) {
  var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
  var encodedMsg = user + " says " + msg;//how the msg will appear
  var li = document.createElement("li");//create a new li item
  li.textContent = encodedMsg;//put the msg in li item as text
  document.getElementById("messagesList").appendChild(li);//append li to ul with id=messagelist
});

//connection.start().then(function () {
//  document.getElementById("sendButton").disabled = false;
//}).catch(function (err) {
//  return console.error(err.toString());
//});

document.getElementById("sendButton").addEventListener("click", function (event) {
  var user = document.getElementById("userInput").value;//get username
  var message = document.getElementById("messageInput").value;//get message
  connection.invoke("SendMessage", user, message).catch(function (err) {
    return console.error(err.toString());
  });
  event.preventDefault();
});