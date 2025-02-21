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

    [HttpGet("Players")]
    public IActionResult GetAllPlayers()
    {
        var player = _databaseRepository.GetAllPlayers();
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
}
