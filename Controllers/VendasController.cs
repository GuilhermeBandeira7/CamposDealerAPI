using CamposDealerApp.Helpers;
using CamposDealerApp.Model;
using CamposDealerApp.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CamposDealerApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VendasController : Controller
    {
        VendaService _service;

        public VendasController(VendaService service)
        {
            _service = service;   
        }

        [HttpGet("Init")]
        public async Task<Response> CarregarDadosIniciais()
        {
            string endpointUrl = "https://camposdealer.dev/Sites/TesteAPI/venda";

            // Create an instance of HttpClient
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(endpointUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();

                        var deserializar = JsonConvert.DeserializeObject<string>(json);

                        List<DeserializeModel>? dadosDeserializados = JsonConvert.DeserializeObject<List<DeserializeModel>>(deserializar);

                        if (dadosDeserializados == null || !dadosDeserializados.Any())
                            return new Response(false, "Falha ao carregar dados.");

                        foreach (DeserializeModel venda in dadosDeserializados)
                        {
                            await _service.AdicionaVendaDeserializada(venda);
                        }

                        return new Response(true, "Dados deserializados e carregados com sucesso.");
                    }
                    else
                    {
                        return new Response(false, $"Falha ao carregar dados. Status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    return new Response(false, ex.Message);
                }
            }
        }

        //[HttpGet("Popula")]
        //public async Task<Response> PopulaBaseDeDados()
        //{

        //}

        [HttpGet]
        public async Task<List<Venda>> RetornaTodasAsVendas()
        {
            return await _service.BuscaTodasVendas();
        }

        [HttpGet("{id}")]
        public async Task<Venda> RetornaVendaPorDsc(long vendaId,[FromBody] string dsc)
        {
            return await _service.BuscaVendaPorDescricao(vendaId, dsc);
        }

        [HttpGet("{id}")]
        public async Task<Venda> RetornaVendaPorCliente(long vendaId, [FromBody]string nomeCliente)
        {
            return await _service.BuscaVendaPorNomeDeCliente(vendaId, nomeCliente);
        }

        [HttpPost]
        public async Task<Response> AdicionaVenda([FromBody]Venda venda)
        {
            return await _service.AdicionaVenda(venda);
        }

        [HttpPut("{id}")]
        public async Task<Response> EditaVenda(long id, [FromBody]Venda venda)
        {
            return await _service.EditaVenda(id, venda);
        }

        [HttpDelete("{id}")]
        public async Task<Response> ExluiVenda(long id)
        {
            return await _service.ExcluiVenda(id);
        }
    }
}
