using Backend.Models;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DatabaseController : ControllerBase
{
    private readonly DatabaseRepository _databaseRepository;

    public DatabaseController(DatabaseRepository databaseRepository)
    {
        _databaseRepository = databaseRepository;
    }

    [HttpGet("cards")]
    public IActionResult GetAllCards()
    {
        var cards = _databaseRepository.GetAllCards();
        return Ok(cards);
    }

    [HttpGet("Players/Random")]
    public IActionResult GetRandomPlayerName()
    {
        var player = _databaseRepository.GetRandomPlayerName();

        if (player == null)
        {
            return NotFound(new { message = "Kein Spieler gefunden" });
        }

        return Ok(player);
    }


    [HttpGet("Players/{id}")]
    public IActionResult GetPlayerById(string id)
    {
        var player = _databaseRepository.GetPlayerById(id);

        if (player == null)
        {
            return NotFound(new { message = "Player not found" });
        }

        return Ok(player);
    }

    [HttpGet("cards/{id}")]
    public IActionResult GetCardsById(string id)
    {
        var player = _databaseRepository.GetCardById(id);

        if (player == null)
        {
            return NotFound(new { message = "Player not found" });
        }

        return Ok(player);
    }

    [HttpGet("Status/{id}")]
    public IActionResult GetStatusById(string id)
    {
        var status = _databaseRepository.GetStatusById(id);

        if (status == null)
        {
            return NotFound(new { message = "Status nicht gefunden" });
        }

        return Ok(status);
    }


    [HttpPut("Status/Update")]
    public IActionResult UpdateStatus([FromQuery] string name, [FromQuery] string newStatus)
    {
        var result = _databaseRepository.UpdateStatusByName(name, newStatus);

        if (result)
        {
            return Ok($"Der Status für '{name}' wurde erfolgreich auf '{newStatus}' geändert.");
        }
        else
        {
            return NotFound($"Kein Status mit dem Namen '{name}' gefunden.");
        }
    }



    [HttpPut("Status/Update/{id}")]
    public IActionResult UpdateStatusById(string id, [FromQuery] string newStatus)
    {
        var result = _databaseRepository.UpdateStatusById(id, newStatus);

        if (result)
        {
            return Ok($"Der Status für die ID '{id}' wurde erfolgreich auf '{newStatus}' geändert.");
        }
        else
        {
            return NotFound($"Kein Status mit der ID '{id}' gefunden.");
        }
    }

    [HttpPost("Status")]
    public IActionResult CreateStatus([FromBody] StatusRequestDto request)
    {
        try
        {
            _databaseRepository.AddStatus(
                request.Name,
                request.Number,
                request.PlayerName,
                request.CurrentUserNumber,
                request.Status,
                request.MixedCards,
                request.LaidCards
            );

            return Ok($"Status mit Name '{request.Name}', Anzahl Spielern '{request.Number}', aktueller Spieler '{request.CurrentUserNumber}', " +
                      $"Status '{request.Status}', gemischten Karten und gelegten Karten erfolgreich hinzugefügt.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Fehler beim Erstellen des Status: {ex.Message}");
        }
    }

    [HttpPut("Status/AddPlayerName")]
    public IActionResult AddPlayerName([FromQuery] string statusId, [FromQuery] string playerName)
    {
        var result = _databaseRepository.AddPlayerNameToStatus(statusId, playerName);

        if (result)
        {
            return Ok($"Spielername '{playerName}' wurde erfolgreich zum Status mit der ID '{statusId}' hinzugefügt.");
        }
        else
        {
            return NotFound($"Kein Status mit der ID '{statusId}' gefunden.");
        }
    }

    [HttpGet("Group/NextPlayer/{groupName}")]
    public IActionResult GetNextPlayerNumber(string groupName)
    {
        var updatedNumber = _databaseRepository.IncrementCurrentUserNumber(groupName);

        if (updatedNumber == -1)
        {
            return NotFound($"Keine Gruppe mit dem Namen '{groupName}' gefunden.");
        }

        return Ok(new { message = $"Nächster Spieler gesetzt auf {updatedNumber}", currentUserNumber = updatedNumber });
    }

    [HttpGet("Cards/Shuffle/{groupName}")]
    public IActionResult ShuffleCards(string groupName)
    {
        var shuffledDeck = _databaseRepository.ShuffleAndStoreDeck(groupName);

        if (shuffledDeck == null || shuffledDeck.Length == 0)
        {
            return BadRequest($"Fehler beim Mischen der Karten für die Gruppe '{groupName}'.");
        }

        return Ok(new { message = "Karten erfolgreich gemischt", mixedCards = shuffledDeck });
    }


    [HttpGet("Status/GetSevenCards")]
    public IActionResult GetSevenCards([FromQuery] string name)
    {
        var status = _databaseRepository.GetStatusByName(name);
        if (status == null || status.mixedCards.Length == 0)
            return NotFound("Keine Karten verfügbar.");

        var cardsToReturn = status.mixedCards.Take(7).ToArray();
        status.mixedCards = status.mixedCards.Skip(7).ToArray();
        _databaseRepository.UpdateStatus(status.Name, status.Number, status);

        return Ok(cardsToReturn);
    }

    [HttpGet("Status/GetOneCard")]
    public IActionResult GetOneCard([FromQuery] string name)
    {
        var status = _databaseRepository.GetStatusByName(name);
        if (status == null || status.mixedCards.Length == 0)
            return NotFound("Keine Karten verfügbar.");

        var card = status.mixedCards.First();
        status.mixedCards = status.mixedCards.Skip(1).ToArray();
        _databaseRepository.UpdateStatus(status.Name, status.Number, status);

        return Ok(card);
    }

    [HttpGet("Status/RefillMixedCards")]
    public IActionResult RefillMixedCards([FromQuery] string groupName)
    {
        var status = _databaseRepository.GetStatusByName(groupName);
        if (status == null || status.laidCards.Length == 0)
            return BadRequest("Keine gelegten Karten zum Hinzufügen.");

        status.mixedCards = status.mixedCards.Concat(status.laidCards).ToArray();
        _databaseRepository.UpdateStatus(status.Name, status.Number, status);

        return Ok(status.mixedCards);
    }

    [HttpPost("Status/AddLaidCard")]
    public IActionResult AddLaidCard([FromBody] AddLaidCardDto request)
    {
        var status = _databaseRepository.GetStatusByName(request.Name);
        if (status == null) return NotFound($"Status '{request.Name}' nicht gefunden.");

        status.laidCards = status.laidCards.Append(request.CardId).ToArray();
        _databaseRepository.UpdateStatus(status.Name, status.Number, status);

        return Ok($"Karte '{request.CardId}' zu laidCards hinzugefügt.");
    }

}
