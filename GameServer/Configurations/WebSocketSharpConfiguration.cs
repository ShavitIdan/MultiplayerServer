using System.ComponentModel.DataAnnotations;

namespace GameServer.Configurations
{
    public class WebSocketSharpConfiguration
    {
        [Required]
        public int? Port { get; set; }

        [Required]
        public string? Path { get; set; }
    }
}
