const axios = require('axios');

async function fetchUnoDeck() {
    try {
        const response = await axios.get('mongodb+srv://user:vp2nWVbo6rfCqxNe@cluster.cy1bc.mongodb.net/');
        return response.data;
    } catch (error) {
        console.error('Fehler beim Laden des Uno-Decks:', error);
        return [];
    }
}
function shuffle(deck) {
    for (let i = deck.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [deck[i], deck[j]] = [deck[j], deck[i]];
    }
}
function drawUnoCard(deck) {
    if (deck.length === 0) {
        return null;
    }
    return deck.pop();
}
function isUnoCardPlayable(card, currentCard, currentColor) {

    if (card.typ === "wild" || card.typ === "wildDrawFour") return true;
    if (currentCard.farbe === "Schwarz" && currentColor) {
        return card.farbe === currentColor;
    }
    if (card.farbe === currentCard.farbe) return true;
    if (card.typ === currentCard.typ) {
        if (card.typ === "number") {
            return card.wert === currentCard.wert;
        }
        return true;
    }
    
    return false;
}

function applyUnoCardEffect(card, room) {
    if (card.typ === "skip") {
        room.turnIndex = (room.turnIndex + 1) % room.players.length;
    } else if (card.typ === "reverse") {
        room.players.reverse();
        room.turnIndex = room.players.length - 1 - room.turnIndex;
    } else if (card.typ === "drawTwo") {
        let nextIndex = (room.turnIndex + 1) % room.players.length;
        for (let i = 0; i < 2; i++) {
            const drawn = drawUnoCard(room.deck);
            if (drawn) room.players[nextIndex].cards.push(drawn);
        }
        room.turnIndex = (room.turnIndex + 1) % room.players.length;
    } else if (card.typ === "wildDrawFour") {
        let nextIndex = (room.turnIndex + 1) % room.players.length;
        for (let i = 0; i < 4; i++) {
            const drawn = drawUnoCard(room.deck);
            if (drawn) room.players[nextIndex].cards.push(drawn);
        }
        room.turnIndex = (room.turnIndex + 1) % room.players.length;
    }
}

module.exports = {
    fetchUnoDeck,
    shuffle,
    drawUnoCard,
    isUnoCardPlayable,
    applyUnoCardEffect
};
