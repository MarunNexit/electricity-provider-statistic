using System.ComponentModel.DataAnnotations;

namespace electricity_provider_server_api.Models
{
    public class LogItem
    {
        [Key]
        public int Id { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [StringLength(20)]
        public string? Level { get; set; }

        [StringLength(100)]
        public string? Action { get; set; }

        [StringLength(100)]
        public string? Entity { get; set; }

        public int? EntityId { get; set; }

        [StringLength(300)]
        public string? Message { get; set; }

        public string? Details { get; set; }

        public int? UserId { get; set; }
    }
}