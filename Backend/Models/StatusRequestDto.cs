namespace Backend.Models
{
    public class StatusRequestDto
    {
        public required string Name { get; set; }
        public int Number { get; set; }
        public string[] PlayerName { get; set; }
        public int CurrentUserNumber { get; set; }
        public required string Status { get; set; }
        public required string[] MixedCards { get; set; }
        public required string[] LaidCards { get; set; }
    }
}
