using CamposDealerApp.Context;
using CamposDealerApp.Helpers;
using CamposDealerApp.Model;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace CamposDealerApp.Service
{
    public class VendaService
    {
        CampDContext _context;

        public VendaService(CampDContext context)
        {
            _context = context;
        }

        public async Task<List<Venda>> BuscaTodasVendas()
        {
            try
            {
                List<Venda>? vendas = await _context.Vendas.ToListAsync();
                if (vendas.Any())
                    return vendas;
                return new();

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return new();
            }
        } 

        public async Task<Venda> BuscaVendaPorDescricao(long vendaId, string dscProduto)
        {
            try
            {
                Venda? venda = await _context.Vendas
                    .Where(v => v.IdVenda == vendaId)
                    .Include(p => p.Produtos).FirstOrDefaultAsync();

                if(venda != null)
                {
                    //Verifica se alguma das vendas possui e descrição de produto passada por parâmetro.
                    int cont = 0;
                    bool contemProduto = false;
                    while(cont < venda.Produtos.Count || contemProduto)
                    {
                        if (venda.Produtos[cont].DscProduto.ToLower().Trim() == dscProduto)
                            contemProduto = true;
                        cont++;
                    } 
                
                    if(contemProduto) 
                        return venda;
                    return new();
                }

                throw new Exception("Nenhuma venda foi encontrada.");
            }
            catch(Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return new();
            }
        }

        public async Task<Venda> BuscaVendaPorNomeDeCliente(long vendaId, string nomeCliente)
        {
            try
            {
                Venda? venda = await _context.Vendas
                    .Where(v => v.IdVenda == vendaId)
                    .FirstOrDefaultAsync();

                if (venda != null)
                {
                    if (venda.Cliente.NmCliente == nomeCliente)
                        return venda;
                    return new();
                }    

                throw new Exception("Nenhuma venda foi encontrada.");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return new();
            }
        }

        public async Task<Response> AdicionaVenda(Venda venda)
        {
            try
            {
                if (venda == null)
                    throw new Exception("Venda não pode ser nulo.");

                Cliente? cliente = await _context.Cliente
                    .Where(c => c.IdCliente == venda.Cliente.IdCliente)
                    .FirstOrDefaultAsync();

                if (cliente == null)
                    return new Response(false, "Não foi encontrado um cliente para esta venda.");
                venda.Cliente = cliente;       
                venda.VlrTotalVenda = venda.VlrUnitarioVenda * venda.QtdVenda;

                await _context.Vendas.AddAsync(venda);
                await _context.SaveChangesAsync();

                return new Response(true, "Venda adicionada com sucesso.");
                 
            }
            catch(Exception ex)
            {
                return new Response(false, ex.Message);
            }
        }

        public async Task<Response> AdicionaVendaDeserializada(DeserializeModel venda)
        {
            try
            {
                if (venda == null)
                    throw new Exception("Venda não pode ser nulo.");

                Cliente? cliente = await _context.Cliente
                    .Where(c => c.IdCliente == venda.IdCliente)
                    .FirstOrDefaultAsync();

               Produto? produto = await _context.Produtos.Where(p => p.IdProduto == venda.IdProduto).FirstOrDefaultAsync();

                if (cliente == null || produto == null)
                    return new Response(false, "Não foi encontrado um cliente ou produto para esta venda.");

                Venda vendaDeserializada = new Venda();
                vendaDeserializada.QtdVenda = venda.QtdVenda;
                vendaDeserializada.DthVenda = venda.DthVenda;
                vendaDeserializada.VlrUnitarioVenda = venda.VlrUnitarioVenda;
                vendaDeserializada.Cliente = cliente;
                vendaDeserializada.Produtos.Add(produto);
                vendaDeserializada.QtdVenda = venda.QtdVenda;
                vendaDeserializada.VlrTotalVenda = venda.QtdVenda * venda.VlrUnitarioVenda;


                await _context.Vendas.AddAsync(vendaDeserializada);
                await _context.SaveChangesAsync();

                return new Response(true, "Venda adicionada com sucesso.");

            }
            catch (Exception ex)
            {
                return new Response(false, ex.Message);
            }
        }

        public async Task<Response> EditaVenda(long vendaId, Venda vendaEditada)
        {
            try
            {
                Venda? venda = await _context.Vendas.Where(v => v.IdVenda == vendaId)
                    .FirstOrDefaultAsync();

                if(venda != null)
                {
                    _context.Entry(vendaEditada).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return new Response(true, "Venda editada com sucesso.");
                }
                throw new Exception("Venda não encontrada.");
            }
            catch(Exception ex)
            {
                return new Response(false, ex.Message);
            }
        }

        public async Task<Response> ExcluiVenda(long vendaId)
        {
            try
            {
                await _context.Vendas.Where(v => v.IdVenda == vendaId).ExecuteDeleteAsync();
                await _context.SaveChangesAsync();
                return new Response(true, "Venda excluída com sucesso.");
            }
            catch(Exception ex)
            {
                return new Response(false, ex.Message); 
            }
        }
    }
}
