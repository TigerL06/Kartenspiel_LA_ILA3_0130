const socket = io();
let nameButton = document.querySelector("#joinRoom")
let parent = document.querySelector("#app")
let section = document.querySelector("#name-section")
let username;
let groupename;
let room;

nameButton.addEventListener("click", function() {
    console.log("gut")
    let text = document.querySelector("#username");
    if(text.value){
        username = text.value;
    } // Nutze .value anstelle von .textContent

    console.log(username); // Zeigt den eingegebenen Wert in der Konsole an
    section.remove();
    if(text.value){
        game()
    }
    if(!text.value){
        getPlayerName();
    }
})
async function getPlayerName(){
    try {
        const response = await fetch("https://localhost:7079/api/Database/Players/Random", { mode: "cors" });

        if (!response.ok) {
            throw new Error("Netzwerkantwort war nicht ok: " + response.status);
        }
        const data = await response.json();
        // Angenommen, die Response enthält direkt den Spielernamen oder ein Objekt mit der Eigenschaft "name"
        // Falls es ein Objekt ist, z.B. { name: "NeuerName" }, dann:
        username = data.name || data;
        console.log("Neuer Spielername:", username);
        game();
    } catch (error) {
        console.error("Fehler beim Abrufen des Spielernamens:", error);
    }
}


function game() {
    let main = document.createElement("div");
    main.setAttribute("id", "main");

    let top = document.createElement("div");
    top.setAttribute("id", "top");
    let name = document.createElement("h3");
    name.setAttribute("id", "username")
    name.innerHTML = username;
    let groupe = document.createElement("input");
    groupe.setAttribute("id", "groupe");
    let groupeButton = document.createElement("button");
    groupeButton.innerHTML = "Join";
    groupeButton.setAttribute("id", "joinRooms")

    let game = document.createElement("div");
    game.setAttribute("id", "game");

    // Zentrales div für die aktuelle Karte hinzufügen
    let centerCard = document.createElement("div");
    centerCard.setAttribute("id", "center-card");
    centerCard.innerHTML = "Aktuelle Karte"; // Platzhaltertext
    game.appendChild(centerCard);

    let cards = document.createElement("div");
    cards.setAttribute("id", "cards");

    let nameSection = document.createElement("div");
    nameSection.setAttribute("id", "name-section");

    let section = document.createElement("div");
    section.setAttribute("id", "sections");

    top.appendChild(name);
    top.appendChild(groupe);
    top.appendChild(groupeButton);
    main.appendChild(top);
    main.appendChild(game);
    main.appendChild(cards);
    parent.appendChild(main);

    // Spieler hinzufügen bei Klick auf den Button
    groupeButton.addEventListener("click", function () {
        groupename = document.querySelector("#groupe");
        console.log(groupename);
        createPlayers();
        createCards();
    });

    document.getElementById("joinRooms").addEventListener("click", () => {
        room = document.querySelector("#groupe").value;
    
        if (!username || !room) {
            alert("Bitte Benutzernamen und Raum eingeben!");
            return;
        }
        
        socket.emit("joinRoom", { username, room });
    });
}

function createCards(){
    let cardsSection = document.querySelector("#cards");
    let card = document.createElement("button");
    card.setAttribute("class", "card");
    cardsSection.appendChild(card);

}

let players = [
    { name: "Liam", cards: 5 },
    { name: "Emma", cards: 7 },
    { name: "Noah", cards: 6 },
    { name: "Sophia", cards: 4 },
    { name: "Oliver", cards: 8 },
    { name: "Oliver", cards: 8 },
    { name: "Oliver", cards: 8 }
];

function createPlayers() {
    let gameSection = document.querySelector("#game");
    gameSection.innerHTML = ""; // Löscht bestehende Spieler

    // Füge das zentrale div erneut hinzu (falls gelöscht)
    let centerCard = document.createElement("div");
    centerCard.setAttribute("id", "center-card");
    centerCard.innerHTML = "Aktuelle Karte";
    gameSection.appendChild(centerCard);

    players.forEach(playerData => {
        let section = document.createElement("div");
        section.setAttribute("class", "player");

        let name = document.createElement("p");
        name.innerHTML = playerData.name;
        let cardsnumber = document.createElement("p");
        cardsnumber.innerHTML = playerData.cards;

        section.appendChild(name);
        section.appendChild(cardsnumber);
        gameSection.appendChild(section);
    });

    positionPlayers(); // Spieler korrekt anordnen
}

function positionPlayers() {
    let totalPlayers = players.length;
    let gameSection = document.querySelector("#game");
    let playerDivs = document.querySelectorAll(".player");

    gameSection.style.position = "relative"; // Referenzpunkt für Spieler

    const centerX = 50; // Mittelpunkt in Prozent (relativ zum `#game`)
    const centerY = 50; // Mittelpunkt in Prozent (relativ zum `#game`)
    const radius = 40;  // Radius des Kreises in Prozent (relativ zum `#game`)

    playerDivs.forEach((player, index) => {
        let x, y;

        if (totalPlayers === 1) {
            // 1 Spieler -> Mitte
            x = "50%";
            y = "50%";
        } else if (totalPlayers === 2) {
            // 2 Spieler -> Oben und Unten
            x = "50%";
            y = index === 0 ? "25%" : "75%";
        } else if (totalPlayers === 3) {
            // 3 Spieler -> Dreieck
            let positions = [
                { x: "50%", y: "10%" },
                { x: "20%", y: "75%" },
                { x: "80%", y: "75%" }
            ];
            x = positions[index].x;
            y = positions[index].y;
        } else if (totalPlayers === 4) {
            // 4 Spieler -> Vier Ecken
            let positions = [
                { x: "15%", y: "15%" },
                { x: "85%", y: "15%" },
                { x: "15%", y: "85%" },
                { x: "85%", y: "85%" }
            ];
            x = positions[index].x;
            y = positions[index].y;
        } else {
            // Mehr als 4 Spieler -> Kreisförmige Anordnung
            const angle = (index / totalPlayers) * 2 * Math.PI; // Winkel berechnen
            x = centerX + radius * Math.cos(angle) + "%";       // x-Position berechnen
            y = centerY + radius * Math.sin(angle) + "%";       // y-Position berechnen
        }

        // Spieler-Position setzen
        player.style.position = "absolute";
        player.style.left = x;
        player.style.top = y;
        player.style.transform = "translate(-50%, -50%)"; // Zentrierung
    });
}