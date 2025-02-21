namespace Backend.Models
{
    // DTO für den POST-Request (kann in eine separate Datei, z.B. AddLaidCardDto.cs, ausgelagert werden)
    public class AddLaidCardDto
    {
        public required string Name { get; set; }
        public required string CardId { get; set; }
    }
}
