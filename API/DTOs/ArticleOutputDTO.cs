namespace API.DTOs
{
    public class ArticleOutputDTO
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string Image { get; set; }
        public required DateOnly CreationDate { get; set; }
        public required DateOnly PublicationDate { get; set; }
        public required string AuthorId { get; set; }
    }
}
