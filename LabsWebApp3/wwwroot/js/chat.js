"use strict";

var signalR = new signalR.HubConnectionBuilder();
var connection = signalR.withUrl("/chatHub").build();
connection.serverTimeoutInMilliseconds = 1000 * 60 * 60 * 3;

connection.start();

connection.on("Notify", function (message) {
    var li = document.createElement("li");
    li.textContent = message;
    document.getElementById("messagesList").appendChild(li);
});

connection.on("ReceiveMessage", function (message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var li = document.createElement("li");
    li.textContent = msg;
    document.getElementById("messagesList").appendChild(li);
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var blocked = document.getElementById("IsBlocked");
    if (blocked.value) {
        var time = document.getElementById("UpTo").value;
        var now = new Date();
        if (now >= time) {
            blocked.value = false;
        }
        else {
            document.getElementById("labelBlocked").text =
                "Вы заблокированы модератором и не можете посылать сообщений до " + time + "!";
            return;
        }
    }
    document.getElementById("labelBlocked").context = "";
    var message = document.getElementById("messageInput").value;
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");

    connection.invoke("SendMessage", msg, null).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("sendPrivateButton").addEventListener("click", function (event) {
    var recipient = document.getElementById("selectedUser").value;
    var message = document.getElementById("messageInput").value;
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    connection.invoke("SendMessage", msg, recipient).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

connection.on("ReceiveBlocked", function (upto) {
    if (upto <= new Date()) {
        document.location.reload();
        return;
    }
    document.getElementById("IsBlocked").value = true;
    document.getElementById("UpTo").value = upto;
    document.getElementById("labelBlocked").innerText =
        "Вы заблокированы модератором и не можете посылать сообщений до " + upto + "!";
});

document.getElementById("blockButton").addEventListener("click", function (event) {
    var ticks = document.getElementById("ticks");
    var tick = ticks.options[ticks.selectedIndex].value;
    var recipient = document.getElementById("selectedUser").value;
    connection.invoke("SendBlock", recipient, tick.toString()).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});