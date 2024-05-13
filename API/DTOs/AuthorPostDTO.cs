using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class AuthorPostDTO
    {
        [Length(3, 20)]
        public required string Name { get; set; }
        public required string Bio { get; set; }
        public required string Image { get; set; }
    }
}
