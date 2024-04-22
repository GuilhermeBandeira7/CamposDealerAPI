using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CamposDealerApp.Model
{
    public class Produto
    {
        [Key]
        [Required]
        [JsonIgnore]
        public long? IdProduto { get; set; }
        public string DscProduto { get; set; } = string.Empty;
        public float VlrUnitario { get; set; }
    }
}
