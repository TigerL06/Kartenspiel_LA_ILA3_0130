using MongoDB.Driver;
using Backend.Models;
using System.Collections.Generic;

namespace Backend.Repositories
{
    public class DatabaseRepository
    {
        private readonly IMongoCollection<Card> _cardCollection;
        private readonly IMongoCollection<Player> _playerCollection;

        public DatabaseRepository(IConfiguration configuration)
        {
            var mongoClient = new MongoClient(configuration.GetSection("MongoDB:ConnectionString").Value);
            var database = mongoClient.GetDatabase("Kartenspiel");

            _cardCollection = database.GetCollection<Card>("Karten");
            _playerCollection = database.GetCollection<Player>("Spielernamen");
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
    }
}
