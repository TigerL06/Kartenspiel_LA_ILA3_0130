const express = require("express");
const http = require("http");
const { Server } = require("socket.io");
const path = require("path");

const app = express();
const server = http.createServer(app);
const io = new Server(server);

// Statische Dateien bereitstellen (HTML, CSS, JS)
app.use(express.static(__dirname));

// Route für den Root-Pfad (Startseite)
app.get("/", (req, res) => {
    res.sendFile(path.join(__dirname, "public", "index.html"));
});

let rooms = {};

io.on("connection", (socket) => {
    console.log("Ein Spieler hat sich verbunden:", socket.id);

    socket.on("joinRoom", ({ username, room }) => {
        socket.join(room);

        if (!rooms[room]) {
            rooms[room] = {
                players: [],
                turnIndex: 0,
                currentCard: drawCard()
            };
        }

        rooms[room].players.push({ id: socket.id, name: username, cards: generateCards(5) });

        io.to(room).emit("updatePlayers", rooms[room].players);
        io.to(room).emit("updateGame", { currentCard: rooms[room].currentCard, turn: getCurrentTurn(room) });

        console.log(`${username} ist dem Raum ${room} beigetreten.`);
    });

    socket.on("playCard", ({ room, card }) => {
        let game = rooms[room];
        if (!game) return;

        let player = game.players[game.turnIndex];
        if (player.id !== socket.id) {
            socket.emit("errorMessage", "Nicht dein Zug!");
            return;
        }

        game.currentCard = card;
        player.cards = player.cards.filter(c => c !== card);

        if (player.cards.length === 0) {
            io.to(room).emit("gameOver", `${player.name} hat gewonnen!`);
            delete rooms[room];
            return;
        }

        game.turnIndex = (game.turnIndex + 1) % game.players.length;
        io.to(room).emit("updateGame", { currentCard: game.currentCard, turn: getCurrentTurn(room) });
    });

    socket.on("disconnect", () => {
        for (let room in rooms) {
            rooms[room].players = rooms[room].players.filter(player => player.id !== socket.id);
            io.to(room).emit("updatePlayers", rooms[room].players);
        }
    });
});

function drawCard() {
    const colors = ["Rot", "Blau", "Grün", "Gelb"];
    const numbers = ["1", "2", "3", "4", "5", "6", "7", "8", "9"];
    return `${colors[Math.floor(Math.random() * colors.length)]} ${numbers[Math.floor(Math.random() * numbers.length)]}`;
}

function generateCards(count) {
    return Array.from({ length: count }, () => drawCard());
}

function getCurrentTurn(room) {
    return rooms[room]?.players[rooms[room]?.turnIndex]?.name || "";
}

// Server auf Port 3000 starten
server.listen(3002, () => console.log("Server läuft auf Port 3002"));
