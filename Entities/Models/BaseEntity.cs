namespace Entities.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow; // Default to current date/time
    }
}
