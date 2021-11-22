# ColorfulClock
Sending messages using Background Processes and SignalR in .NET 6

This app is for educational purposes.
It displays a real time clock, with random different colors.

Since signalR hubs do have a transient lifespan, background processes / periodic tasks can not be implemented inside them.
But background processes can get the signalR hub instance via DI and call its methods.
So I came up with 2 of them. One sends the current time and the other one sends random colors.

There is also a vue.js client which listens to those events and acts accordingly.
