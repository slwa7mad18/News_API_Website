using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class AuthorPutDTO
    {
        public required string Id { get; set; }
        [Length(3, 20)]
        public required string Name { get; set; }
        public required string Bio { get; set; }
        public required string Image { get; set; }
    }
}
