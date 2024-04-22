using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CamposDealerApp.Model
{
    public class Venda
    {
        [Key]
        [Required]
        [JsonIgnore]
        public long? IdVenda { get; set; }
        [ForeignKey("IdCliente")]
        public Cliente Cliente { get; set; } = new Cliente();
        [ForeignKey("IdProduto")]
        public List<Produto> Produtos { get; set; } = new List<Produto>();
        public int QtdVenda { get; set; }
        public int VlrUnitarioVenda { get; set; }
        public DateTime DthVenda { get; set; }
        public float VlrTotalVenda { get; set; } 
    }
}
