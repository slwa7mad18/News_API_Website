using Sieve.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Article
    {
        public Article()
        {
            Id = Guid.NewGuid().ToString();
            CreationDate = DateOnly.FromDateTime(DateTime.Now);
        }

        [Sieve(CanSort = true, CanFilter = true)]
        public required string Id { get; set; }
        [Sieve(CanSort = true, CanFilter = true)]
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required byte[] Image { get; set; }

        // Dates
        [Sieve(CanSort = true, CanFilter = true)]
        public required DateOnly CreationDate { get; init; }

        [Sieve(CanSort = true, CanFilter = true)]
        public required DateOnly PublicationDate { get; set; }

        // Navigational Properties
        [ForeignKey(nameof(Author))]
        [Sieve(CanSort = true, CanFilter = true)]
        public required string AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}
