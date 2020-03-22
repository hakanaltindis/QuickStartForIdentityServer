namespace WebApplicationClient.Models
{

    public class Post
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsDeleted { get; set; }

        public int BlogId { get; set; }
    }
}
