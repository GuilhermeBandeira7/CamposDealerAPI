using CamposDealerApp.Context;
using CamposDealerApp.Helpers;
using CamposDealerApp.Model;
using Microsoft.EntityFrameworkCore;

namespace CamposDealerApp.Service
{
    public class ProdutoService
    {
        CampDContext _context;

        public ProdutoService(CampDContext context)
        {
            _context = context;
        }

        public async Task<List<Produto>> BuscaTodosProdutos()
        {
            try
            {
                List<Produto>? produtos = await _context.Produtos.ToListAsync();
                if (produtos.Any())
                    return produtos;

                return new();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return new();
            }
        }

        public async Task<Produto> BuscaProduto(long produtoId, string descricao)
        {
            try
            {
                Produto? produto = await _context.Produtos
                    .Where(p => p.IdProduto == produtoId && 
                    p.DscProduto.ToLower().Trim() == descricao.ToLower().Trim())
                    .FirstOrDefaultAsync();

                if (produto != null)
                    return produto;

                throw new Exception("Cliente não encontrado.");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return new();
            }
        }

        public async Task<Response> AdicionaProduto(Produto produto)
        {
            try
            {
                if (produto.VlrUnitario <= 0 || produto.DscProduto == string.Empty)
                    return new Response(false, "Campos do produtos não podem estar vazios.");

                await _context.Produtos.AddAsync(produto);
                await _context.SaveChangesAsync();
                return new Response(true, "Produto adicionado com sucesso.");               
            }
            catch (Exception ex)
            {
                return new Response(false, ex.Message);
            }
        }

        public async Task<Response> EditaProduto(long produtoId, Produto produtoEditado)
        {
            try
            {
                Produto? produto = await _context.Produtos.Where(p => p.IdProduto == produtoId).FirstOrDefaultAsync();
                if (produto != null)
                {
                    _context.Entry(produtoEditado).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return new Response(true, "Produto editado com sucesso.");
                }
                return new Response(false, "Produto não encontrado.");
            }
            catch (Exception ex)
            {
                return new Response(false, ex.Message);
            }
        }

        public async Task<Response> ExcluiProduto(long produtoId)
        {
            try
            {
                await _context.Produtos.Where(p => p.IdProduto == produtoId).ExecuteDeleteAsync();
                await _context.SaveChangesAsync();
                return new Response(true, "Produto excluído com sucesso.");
            }
            catch (Exception ex)
            {
                return new Response(false, ex.Message);
            }
        }
    }
}
