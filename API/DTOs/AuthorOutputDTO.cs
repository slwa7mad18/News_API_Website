namespace API.DTOs
{
    public class AuthorOutputDTO
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Bio { get; set; }
        public required string Image { get; set; }
    }
}
