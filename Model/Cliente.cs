using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CamposDealerApp.Model
{
    public class Cliente
    {
        [Key]
        [Required]
        [JsonIgnore]
        public long? IdCliente { get; set; }
        public string NmCliente { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
    }
}
