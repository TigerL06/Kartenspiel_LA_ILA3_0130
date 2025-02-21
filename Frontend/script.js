const socket = io();

let username;
let room;

document.getElementById("joinRoom").addEventListener("click", () => {
    username = document.getElementById("username").value;
    room = document.getElementById("room").value;

    if (!username || !room) {
        alert("Bitte Benutzernamen und Raum eingeben!");
        return;
    }

    document.getElementById("name-section").style.display = "none";
    document.getElementById("game").style.display = "block";

    socket.emit("joinRoom", { username, room });
});

socket.on("updatePlayers", (players) => {
    let playersDiv = document.getElementById("players");
    playersDiv.innerHTML = "";
    players.forEach(player => {
        let playerDiv = document.createElement("div");
        playerDiv.classList.add("player");
        playerDiv.innerHTML = `<strong>${player.name}</strong> (Karten: ${player.cards.length})`;
        playersDiv.appendChild(playerDiv);
    });
});

socket.on("updateGame", ({ currentCard, turn }) => {
    document.getElementById("center-card").innerText = currentCard;
    document.getElementById("turnIndicator").innerText = `Am Zug: ${turn}`;
});

socket.on("gameOver", (message) => {
    alert(message);
    location.reload();
});
