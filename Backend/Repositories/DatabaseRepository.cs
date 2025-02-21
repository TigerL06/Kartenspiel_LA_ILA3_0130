using MongoDB.Driver;
using Backend.Models;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Backend.Repositories
{
    public class DatabaseRepository
    {
        private readonly IMongoCollection<Card> _cardCollection;
        private readonly IMongoCollection<Player> _playerCollection;
        private readonly IMongoCollection<Status> _statusCollection;

        public DatabaseRepository(IConfiguration configuration)
        {
            var mongoClient = new MongoClient(configuration.GetSection("MongoDB:ConnectionString").Value);
            var database = mongoClient.GetDatabase("Kartenspiel");

            _cardCollection = database.GetCollection<Card>("Karten");
            _playerCollection = database.GetCollection<Player>("Spielernamen");
            _statusCollection = database.GetCollection<Status>("Spielstand");
        }

        /// <summary>
        /// Function to get all cards from the database
        /// </summary>
        /// <returns></returns>
        public List<Card> GetAllCards()
        {
            var bsonCards = _cardCollection.Find(_ => true).ToList();

            var cards = bsonCards.Select(card => new Card
            {
                Id = card.Id,
                Nummer = card.Nummer,
                Farbe = card.Farbe,
                Spezial = card.Spezial
            }).ToList();

            return cards;
        }

        /// <summary>
        /// Funktion to get all players from the database
        /// </summary>
        /// <returns></returns>
        public List<Player> GetAllPlayers()
        {
            var bsonPlayers = _playerCollection.Find(_ => true).ToList();

            var players = bsonPlayers.Select(player => new Player
            {
                Id = player.Id,
                Name = player.Name,

            }).ToList();

            return players;
        }


        /// <summary>
        /// Function to get a player by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Player GetPlayerById(string id)
        {
            var player = _playerCollection.Find(player => player.Id == id).FirstOrDefault();

            return player;
        }

        /// <summary>
        /// Function to get a status by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Status GetStatusById(string id)
        {
            var status = _statusCollection.Find(status => status.Id == id).FirstOrDefault();

            return status;
        }

        /// <summary>
        /// Function to get a card by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Card GetCardById(string id)
        {
            var card = _cardCollection.Find(card => card.Id == id).FirstOrDefault();

            return card;
        }

        /// <summary>
        /// Function to add a new status to the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="anzahl"></param>
        /// <param name="status"></param>
        public void AddStatus(string name, int number, int currentUserNumber, string status, string[] mixedCards, string[] laidCards)
        {
            try
            {
                var newStatus = new Status
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    Name = name,
                    Number = number,
                    CurrentUserNumber = currentUserNumber,
                    status = status,
                    mixedCards = mixedCards,
                    laidCards = laidCards
                };

                _statusCollection.InsertOne(newStatus);
                Console.WriteLine("Status erfolgreich hinzugefügt.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Hinzufügen des Status: {ex.Message}");
            }
        }

        /// <summary>
        /// Function to update the status by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        public bool UpdateStatusByName(string name, string newStatus)
        {
            try
            {
                // Filter: Suche nach dem Dokument mit dem angegebenen Namen
                var filter = Builders<Status>.Filter.Eq(s => s.Name, name);

                // Update: Ändere nur das Feld "status"
                var update = Builders<Status>.Update.Set(s => s.status, newStatus);

                // Aktualisierung durchführen
                var result = _statusCollection.UpdateOne(filter, update);

                if (result.MatchedCount > 0)
                {
                    Console.WriteLine($"Status für '{name}' erfolgreich auf '{newStatus}' geändert.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Kein Status mit dem Namen '{name}' gefunden.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Aktualisieren des Status: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Function to update the status by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        public bool UpdateStatusById(string id, string newStatus)
        {
            try
            {
                // Konvertiere die ID zu ObjectId
                var objectId = ObjectId.Parse(id);

                // Filter: Suche nach dem Dokument mit der angegebenen ID
                var filter = Builders<Status>.Filter.Eq(s => s.Id, objectId.ToString());

                // Update: Ändere nur das Feld "status"
                var update = Builders<Status>.Update.Set(s => s.status, newStatus);

                // Aktualisierung durchführen
                var result = _statusCollection.UpdateOne(filter, update);

                if (result.MatchedCount > 0)
                {
                    Console.WriteLine($"Status für die ID '{id}' erfolgreich auf '{newStatus}' geändert.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Kein Status mit der ID '{id}' gefunden.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Aktualisieren des Status: {ex.Message}");
                return false;
            }
        }

        public int IncrementCurrentUserNumber(string groupName)
        {
            var filter = Builders<Status>.Filter.Eq(s => s.Name, groupName);
            var groupStatus = _statusCollection.Find(filter).FirstOrDefault();

            if (groupStatus == null)
            {
                return -1; // Gruppe nicht gefunden
            }

            // Erhöhe den `CurrentUserNumber` und falls er größer als `Number` ist, setze ihn zurück auf 1
            int newCurrentUserNumber = (groupStatus.CurrentUserNumber % groupStatus.Number) + 1;

            var update = Builders<Status>.Update.Set(s => s.CurrentUserNumber, newCurrentUserNumber);
            _statusCollection.UpdateOne(filter, update);

            return newCurrentUserNumber;
        }

        public string[] ShuffleAndStoreDeck(string groupName)
        {
            var allCards = _cardCollection.Find(_ => true).ToList();

            if (allCards.Count == 0)
            {
                return new string[0]; // Keine Karten vorhanden
            }

            // Mische die Karten
            var shuffledCards = allCards.OrderBy(_ => Guid.NewGuid()).Select(c => c.Id).ToArray();

            // Speichere das gemischte Deck in `mixedCards` des `Status`-Dokuments der Gruppe
            var filter = Builders<Status>.Filter.Eq(s => s.Name, groupName);
            var update = Builders<Status>.Update.Set(s => s.mixedCards, shuffledCards);

            _statusCollection.UpdateOne(filter, update);

            return shuffledCards;
        }

        // Hinzugefügte Methode: Status anhand des Namens abrufen
        public Status GetStatusByName(string name)
        {
            return _statusCollection.Find(s => s.Name == name).FirstOrDefault();
        }

        // Hinzugefügte Methode: Komplettes Status-Dokument aktualisieren
        public void UpdateStatus(Status status)
        {
            _statusCollection.ReplaceOne(s => s.Id == status.Id, status);
        }
    }
}
