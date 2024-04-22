using CamposDealerApp.Helpers;
using CamposDealerApp.Model;
using CamposDealerApp.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CamposDealerApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutosController : Controller
    {
        private readonly ProdutoService _service;

        public ProdutosController(ProdutoService service)
        {
            _service = service;
        }

        [HttpGet("Init")]
        public async Task<Response> CarregarDadosIniciais()
        {
            string endpointUrl = "https://camposdealer.dev/Sites/TesteAPI/produto";

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

                        List<Produto>? dadosDeserializados = JsonConvert.DeserializeObject<List<Produto>>(deserializar);

                        if (dadosDeserializados == null || !dadosDeserializados.Any())
                            return new Response(false, "Falha ao carregar dados.");

                        foreach (Produto produto in dadosDeserializados)
                        {
                            produto.IdProduto = null;
                            await AdicionaProduto(produto);
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

        [HttpGet]
        public async Task<List<Produto>> RetornaTodosOsProdutos() 
        {
            return await _service.BuscaTodosProdutos();
        }

        [HttpGet("{id}/{nome}")]
        public async Task<Produto> RetornaProduto(long id, string nome)
        {
            return await _service.BuscaProduto(id, nome);
        }

        [HttpPost]
        public async Task<Response> AdicionaProduto([FromBody]Produto produto)
        {
            return await _service.AdicionaProduto(produto);
        }

        [HttpPut("{id}")]
        public async Task<Response> EditaProduto(long id, [FromBody]Produto produto)
        {
            return await _service.EditaProduto(id, produto);
        }

        [HttpDelete("{id}")]
        public async Task<Response> ExcluiProduto(long id)
        {
            return await _service.ExcluiProduto(id);
        }
    }
}
