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
        var player = _databaseRepository.GetStatusById(id);

        if (player == null)
        {
            return NotFound(new { message = "Player not found" });
        }

        return Ok(player);
    }

    [HttpPut]
    [Route("Status/Update")]
    public IActionResult UpdateStatus([FromQuery] string name, [FromQuery] string newStatus)
    {
        var result = _databaseRepository.UpdateStatusByName(name, newStatus);

        if (result)
        {
            return Ok($"Der Status für '{name}' wurde erfolgreich geändert zu '{newStatus}'.");
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
            return Ok($"Der Status für die ID '{id}' wurde erfolgreich geändert zu '{newStatus}'.");
        }
        else
        {
            return NotFound($"Kein Status mit der ID '{id}' gefunden.");
        }
    }

    [HttpPost]
    [Route("Status")]
    public IActionResult CreateStatus([FromQuery] string name, [FromQuery] int anzahl, [FromQuery] string status)
    {
        try
        {
            _databaseRepository.AddStatus(name, anzahl, status);
            return Ok($"Status mit Name '{name}', Anzahl Spielern '{anzahl}' und Status '{status}' erfolgreich hinzugefügt.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Fehler beim Erstellen des Status: {ex.Message}");
        }
    }





}
