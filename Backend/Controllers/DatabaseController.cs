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

    [HttpGet("{id}")]
    public IActionResult GetPlayerById(string id)
    {
        var player = _databaseRepository.GetPlayerById(id);

        if (player == null)
        {
            return NotFound(new { message = "Player not found" });
        }

        return Ok(player);
    }
}
