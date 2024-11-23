namespace Entities.Models
{
    public class Message : BaseEntity
    {
        public int UserTokenId { get; set; }
        public string Content { get; set; }
    }
}
