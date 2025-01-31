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

        public List<Card> GetAllCards()
        {
            var bsonCards = _cardCollection.Find(_ => true).ToList();

            var cards = bsonCards.Select(card => new Card
            {
                Id = card.Id,
                Nummer = card.Nummer ?? 0,
                Farbe = card.Farbe,
                Spezial = card.Spezial
            }).ToList();

            return cards;
        }

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

        public Player GetPlayerById(string id)
        {
            var player = _playerCollection.Find(player => player.Id == id).FirstOrDefault();

            return player;
        }

        public Status GetStatusById(string id)
        {
            var status = _statusCollection.Find(status => status.Id == id).FirstOrDefault();

            return status;
        }

        public Card GetCardById(string id)
        {
            var card = _cardCollection.Find(card => card.Id == id).FirstOrDefault();

            return card;
        }

        public void AddStatus(string name, int anzahl, string status)
        {
            try
            {
                // Neues Status-Objekt erstellen
                var newStatus = new Status
                {
                    Id = ObjectId.GenerateNewId().ToString(), // Automatische Generierung einer ID
                    Name = name,
                    Number = anzahl, // Anzahl der Spieler
                    status = status
                };

                // Status-Dokument in die Collection einfügen
                _statusCollection.InsertOne(newStatus);

                Console.WriteLine("Status erfolgreich hinzugefügt.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Hinzufügen des Status: {ex.Message}");
            }
        }


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


    }
}
