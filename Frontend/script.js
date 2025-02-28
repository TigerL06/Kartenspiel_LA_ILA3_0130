const socket = io();
let nameButton = document.querySelector("#joinRoom")
let parent = document.querySelector("#app")
let section = document.querySelector("#name-section")
let username;
let groupename;
let room;
let test;
let statusId;

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
        fetchStatusByName(room);
        if(test = false){
            createStatus();
        }else {
            addPlayerName();
        }

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

async function createStatus() {
    const name = document.getElementById("groupe").value;
    groupename = document.getElementById("groupe").value;
    const messageElement = document.getElementById("responseMessage");

    if (!name || !groupName) {
        messageElement.style.color = "red";
        messageElement.innerText = "Bitte Name und Gruppenname eingeben.";
        return;
    }

    try {
        // 1. Karten mischen durch GET-Anfrage
        const shuffleResponse = await fetch(`https://localhost:7079/api/Database/Cards/Shuffle/${groupName}`, {
            method: "GET",
            headers: { "Content-Type": "application/json" }
        });

        // 2. POST-Anfrage mit gemischten Karten senden
        const requestData = {
            Name: name,
            Number: 1,
            PlayerName: [username],
            CurrentUserNumber: 0,
            Status: "going",
            MixedCards: [],
            LaidCards: []
        };

        const response = await fetch("https://localhost:7079/api/Database/Status", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(requestData)
        });

        if (response.ok) {
            const data = await response.text();
            messageElement.style.color = "green";
            messageElement.innerText = `Erfolg: ${data}`;
        } else {
            const errorMessage = await response.text();
            messageElement.style.color = "red";
            messageElement.innerText = `Fehler: ${errorMessage}`;
        }
    } catch (error) {
        console.error("Netzwerkfehler:", error);
        messageElement.style.color = "red";
        messageElement.innerText = "Netzwerkfehler. API nicht erreichbar?";
    }
}

async function addPlayerName() {
    try {
        const response = await fetch("https://localhost:7079/api/Database/Status/AddPlayerName", {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                statusId: statusId,
                playerName: username
            })
        });
        
        const data = await response.text();
        
        if (response.ok) {
            document.getElementById("responseMessage").innerText = `Erfolg: ${data}`;
        } else {
            document.getElementById("responseMessage").innerText = `Fehler: ${data}`;
        }
    } catch (error) {
        console.error("Netzwerkfehler:", error);
        document.getElementById("responseMessage").innerText = "Netzwerkfehler. API nicht erreichbar?";
    }
}


async function getNextPlayer(groupName) {
    try {
        const response = await fetch(`https://localhost:7079/api/Database/Group/NextPlayer/${groupName}`);
        if (!response.ok) {
            throw new Error(`Fehler beim Abrufen des nächsten Spielers: ${response.statusText}`);
        }
        const data = await response.json();
        console.log(data.message, data.currentUserNumber);
        return data;
    } catch (error) {
        console.error('Error in getNextPlayer:', error);
        return null;
    }
}
async function shuffleCards(groupName) {
    try {
        const response = await fetch(`https://localhost:7079/api/Database/Cards/Shuffle/${groupName}`);
        if (!response.ok) {
            throw new Error(`Fehler beim Mischen der Karten: ${response.statusText}`);
        }
        const data = await response.json();
        console.log(data.message, data.mixedCards);
        return data;
    } catch (error) {
     console.error('Error in shuffleCards:', error);
        return null;
    }
}
(async () => {
    const groupName = 'MeineGruppe';
    const nextPlayer = await getNextPlayer(groupName);
    const shuffled = await shuffleCards(groupName);
})();

async function fetchSevenCards(playerName) {
    try {
        const response = await fetch(`https://localhost:7079/api/Database/Status/GetSevenCards?name=${encodeURIComponent(playerName)}`);
        if (!response.ok) {
            throw new Error('Keine Karten verfügbar');
        }
        const data = await response.json();
        console.log('Sieben Karten:', data);
        // Hier kannst du die Karten im UI anzeigen
    } catch (error) {
        console.error('Fehler:', error);
    }
}

async function getStatusById() {
    const statusId = document.getElementById("statusId").value;
    const url = new URL(`https://localhost:7079/api/Database/Status/${statusId}`);
    const messageElement = document.getElementById("responseMessage");

    // Falls kein statusId eingegeben wurde, abbrechen
    if (!statusId) {
        messageElement.innerText = "Bitte Status-ID eingeben.";
        return;
    }

    try {
        const response = await fetch(url, {
            method: "GET",
            headers: {
                "Content-Type": "application/json"
            }
        });

        if (response.ok) {
            const data = await response.text();
            messageElement.style.color = "green";
            messageElement.innerText = `Erfolg: ${data}`;
        } else {
            const errorMessage = await response.text();
            messageElement.style.color = "red";
            messageElement.innerText = `Fehler: ${errorMessage}`;
        }
    } catch (error) {
        console.error("Netzwerkfehler:", error);
        messageElement.innerText = "Netzwerkfehler. API nicht erreichbar?";
    }
}

async function fetchStatusByName(playerName) {
    const response = await fetch(`https://localhost:7079/api/Database/Status/GetByName?name=${encodeURIComponent(playerName)}`);
    if (!response.ok) {
        throw new Error('Kein Status verfügbar');
    }
    const data = await response.text(); // Liest den Antwort-Text aus
    if (data === "No Status") {
        console.log("Kein Status vorhanden");
        test = false;
        // Weitere Logik, wenn kein Status existiert
    } else {
        // Falls du erwartest, dass die Antwort ein JSON-Objekt ist, kannst du versuchen, es zu parsen:
        try {
            const jsonData = JSON.parse(data);
            console.log("Spieler Status:", jsonData);
            statusId = jsonData.id;
            console.log(statusId);
        } catch (error) {
            console.log("Spieler Status (als Text):", data);
        }
    }
}
