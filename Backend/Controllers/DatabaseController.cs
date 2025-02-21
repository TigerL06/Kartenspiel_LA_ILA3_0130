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


    // 1. Get-Request: Sieben Karten abholen und aus mixedCards entfernen
    // URL: api/Database/Status/GetSevenCards?name=StatusName
    [HttpGet("Status/GetSevenCards")]
    public IActionResult GetSevenCards([FromQuery] string name)
    {
        var status = _databaseRepository.GetStatusByName(name);
        if (status == null)
        {
            return NotFound($"Status mit dem Namen '{name}' wurde nicht gefunden.");
        }

        if (status.mixedCards == null || !status.mixedCards.Any())
        {
            return NotFound("Keine Karten im gemischten Karten-Array vorhanden.");
        }

        int count = Math.Min(7, status.mixedCards.Length);
        var cardsToReturn = status.mixedCards.Take(count).ToArray();

        // Entferne die ausgegebenen Karten aus dem mixedCards-Array
        status.mixedCards = status.mixedCards.Skip(count).ToArray();

        _databaseRepository.UpdateStatus(status);

        return Ok(cardsToReturn);
    }

    // 2. Get-Request: Eine Karte abholen und aus mixedCards entfernen
    // URL: api/Database/Status/GetOneCard?name=StatusName
    [HttpGet("Status/GetOneCard")]
    public IActionResult GetOneCard([FromQuery] string name)
    {
        var status = _databaseRepository.GetStatusByName(name);
        if (status == null)
        {
            return NotFound($"Status mit dem Namen '{name}' wurde nicht gefunden.");
        }

        if (status.mixedCards == null || !status.mixedCards.Any())
        {
            return NotFound("Keine Karten im gemischten Karten-Array vorhanden.");
        }

        var card = status.mixedCards.First();
        status.mixedCards = status.mixedCards.Skip(1).ToArray();

        _databaseRepository.UpdateStatus(status);

        return Ok(card);
    }

    // 3. Get-Request: Alle laidCards in das mixedCards-Array hinzufügen
    // URL: api/Database/Status/RefillMixedCards?groupName=StatusName
    [HttpGet("Status/RefillMixedCards")]
    public IActionResult RefillMixedCards([FromQuery] string groupName)
    {
        var status = _databaseRepository.GetStatusByName(groupName);
        if (status == null)
        {
            return NotFound($"Status mit dem Namen '{groupName}' wurde nicht gefunden.");
        }

        if (status.laidCards == null || !status.laidCards.Any())
        {
            return BadRequest("Keine gelegten Karten vorhanden, um sie hinzuzufügen.");
        }

        status.mixedCards = status.mixedCards.Concat(status.laidCards).ToArray();
        // Optional: status.laidCards = new string[0]; // falls man die gelegten Karten "entfernen" will
        _databaseRepository.UpdateStatus(status);

        return Ok(status.mixedCards);
    }

    // 4. Post-Request: Eine Karte zur laidCards-Liste hinzufügen
    // URL: api/Database/Status/AddLaidCard
    // Erwarteter JSON-Body: { "Name": "StatusName", "CardId": "KartenId" }
    [HttpPost("Status/AddLaidCard")]
    public IActionResult AddLaidCard([FromBody] AddLaidCardDto request)
    {
        var status = _databaseRepository.GetStatusByName(request.Name);
        if (status == null)
        {
            return NotFound($"Status mit dem Namen '{request.Name}' wurde nicht gefunden.");
        }

        var laidCardsList = status.laidCards.ToList();
        laidCardsList.Add(request.CardId);
        status.laidCards = laidCardsList.ToArray();

        _databaseRepository.UpdateStatus(status);

        return Ok($"Karte mit ID '{request.CardId}' wurde zu laidCards hinzugefügt.");
    }

}
