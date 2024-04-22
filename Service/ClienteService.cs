using CamposDealerApp.Context;
using CamposDealerApp.Helpers;
using CamposDealerApp.Model;
using Microsoft.EntityFrameworkCore;

namespace CamposDealerApp.Service
{
    public class ClienteService
    {
        public CampDContext _context;

        public ClienteService(CampDContext context)
        {
            _context = context; 
        }

        public async Task<List<Cliente>> BuscaTodosClientes()
        {
            try
            {
                List<Cliente>? clientes = await _context.Cliente.ToListAsync();
                if(clientes.Any()) 
                    return clientes;

                return new();
            }
            catch(Exception ex) 
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return new();
            }
        }

        public async Task<Cliente> BuscaCliente(long clienteId, string nome)
        {
            try
            {
                Cliente? cliente = await _context.Cliente.Where(c => c.IdCliente == clienteId 
                && c.NmCliente.ToLower() == nome.ToLower()).FirstOrDefaultAsync();

                if(cliente != null)
                    return cliente;

                throw new Exception("Cliente não encontrado.");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return new();
            }
        }

        public async Task<Response> AdicionaCliente(Cliente cliente)
        {
            try
            {
                if (cliente.Cidade == string.Empty || cliente.NmCliente == string.Empty)
                    return new Response(false, "Nenhum campo de cliente deve estar vazio.");

                await _context.Cliente.AddAsync(cliente);
                await _context.SaveChangesAsync();
                return new Response(true, "Cliente adicionado com sucesso.");
            }
            catch(Exception ex)
            {
                return new Response(false, ex.Message);
            }
        }

        public async Task<Response> EditaCliente(long clienteId, Cliente clienteEditado)
        {
            try
            {
                Cliente? cliente = await _context.Cliente.Where(c => c.IdCliente == clienteId).FirstOrDefaultAsync();
                if( cliente != null)
                {
                    _context.Entry(clienteEditado).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return new Response(true, "Cliente editado com sucesso.");
                }
                return new Response(false, "Cliente não encontrado.");
            }
            catch(Exception ex)
            {
                return new Response(false, ex.Message);
            }
        }

        public async Task<Response> ExcluiCliente(long clienteId)
        {
            try
            {
                await _context.Cliente.Where(c => c.IdCliente == clienteId).ExecuteDeleteAsync();
                await _context.SaveChangesAsync();
                return new Response(true, "Cliente excluído com sucesso.");
            }
            catch (Exception ex)
            {
                return new Response(false, ex.Message);
            }
        }
    }
}
