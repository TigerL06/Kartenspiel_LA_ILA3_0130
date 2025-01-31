const socket = io();

// Wenn das Spiel startet
document.getElementById('start-game').addEventListener('click', () => {
  socket.emit('startGame');
});

// Wenn eine neue Karte ausgegeben wird
socket.on('dealCards', (cards) => {
  displayCards(cards);
});

// Wenn sich der aktuelle Spieler ändert
socket.on('currentPlayer', (player) => {
  document.getElementById('current-player').innerText = `Aktueller Spieler: ${player}`;
});

function displayCards(cards) {
  const playerCardsDiv = document.getElementById('player-cards');
  playerCardsDiv.innerHTML = ''; // Vorherige Karten entfernen

  cards.forEach(card => {
    const cardElement = document.createElement('div');
    cardElement.classList.add('card');
    cardElement.innerText = `${card.color} ${card.value}`;
    playerCardsDiv.appendChild(cardElement);
  });
}
