using CamposDealerApp.Helpers;
using CamposDealerApp.Model;
using CamposDealerApp.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CamposDealerApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientesController : Controller
    {
        private readonly ClienteService _service;

        public ClientesController(ClienteService service)
        {
            _service = service;
        }

        [HttpGet("Init")]
        public async Task<Response> CarregarDadosIniciais()
        {
            string endpointUrl = "https://camposdealer.dev/Sites/TesteAPI/cliente";

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

                        Cliente[]? dadosDeserializados = JsonConvert.DeserializeObject<Cliente[]>(deserializar);              

                        if (dadosDeserializados == null || !dadosDeserializados.Any())
                            return new Response(false, "Falha ao carregar dados.");

                        foreach (Cliente cliente in dadosDeserializados)
                        {
                            cliente.IdCliente = null;
                            await AdicionaCliente(cliente);
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
        public async Task<List<Cliente>> RetornaTodosOsClientes()
        {
            return await _service.BuscaTodosClientes();
        }

        [HttpGet("{id}/{nome}")]
        public async Task<Cliente> RetornaCliente(long id, string nome)
        {
            return await _service.BuscaCliente(id, nome);
        }

        [HttpPost]
        public async Task<Response> AdicionaCliente([FromBody]Cliente cliente)
        {
            return await _service.AdicionaCliente(cliente);
        }

        [HttpPut("{id}")]
        public async Task<Response> EditaCliente(long id, [FromBody]Cliente cliente)
        {
            return await _service.EditaCliente(id, cliente);    
        }

        [HttpDelete("{id}")]
        public async Task<Response> ExcluiCliente(long id)
        {
            return await _service.ExcluiCliente(id);
        }
    }
}
