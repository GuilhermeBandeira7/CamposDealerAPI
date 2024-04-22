using CamposDealerApp.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace CamposDealerApp.Helpers
{
    public class DeserializeModel
    {
        public long IdVenda { get; set; }
        public long IdCliente { get; set; }
        public long IdProduto { get; set; }
        public int QtdVenda { get; set; }
        public int VlrUnitarioVenda { get; set; }
        public DateTime DthVenda { get; set; }
        public float VlrTotalVenda { get; set; }
    }
}
