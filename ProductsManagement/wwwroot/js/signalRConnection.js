"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl(baseUrl + "/chatHub").build();

connection.start();
