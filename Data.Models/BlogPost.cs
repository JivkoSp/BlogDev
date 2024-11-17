
namespace Data.Models
{
    internal sealed class BlogPost
    {
        public string? Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime PublishDate { get; set; }

        public Category? Category { get; set; }

        public List<Tag> Tags { get; set; } = new();
    }
}