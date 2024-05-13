using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Author
    {
        public Author()
        {
            Id = Guid.NewGuid().ToString();
            Articles = [];
        }

        [Sieve(CanSort = true, CanFilter = true)]
        public required string Id { get; set; }
        [Length(3, 20)]
        [Sieve(CanSort = true, CanFilter = true)]
        public required string Name { get; set; }
        public required string Bio { get; set; }
        public required byte[] Image { get; set; }

        // Navigational Properties
        public List<Article> Articles { get; set; }
    }
}
