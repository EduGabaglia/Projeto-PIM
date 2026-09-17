using Microsoft.AspNetCore.Mvc;
using projeto_estoque_web.Models.view_models;

namespace projeto_estoque_web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var produtos = ProdutosController.TodosOsProdutos;
            var pedidos = PedidosController.TodosOsPedidos;

            var agora = DateTime.Now;

            var maisVendidos = pedidos
                .Where(p => p.Status != "Cancelado")
                .SelectMany(p => p.Itens)
                .GroupBy(i => i.NomeProduto)
                .Select(g => new ProdutoMaisVendidoViewModel
                {
                    Nome = g.Key,
                    QuantidadeVendida = g.Sum(i => i.Quantidade)
                })
                .OrderByDescending(x => x.QuantidadeVendida)
                .Take(4)
                .ToList();

            var model = new DashboardViewModel
            {
                TotalProdutos = produtos.Count,
                EstoqueBaixo = produtos.Count(p => p.Quantidade <= 5),
                PedidosPendentes = pedidos.Count(p => p.Status == "Pendente"),
                VendasMes = pedidos
                    .Where(p => p.Status != "Cancelado" &&
                                p.Data.Year == agora.Year &&
                                p.Data.Month == agora.Month)
                    .Sum(p => p.ValorTotal),

                UltimosPedidos = pedidos
                    .OrderByDescending(p => p.Data)
                    .ThenByDescending(p => p.Id)
                    .Take(3)
                    .Select(p => new PedidoResumoViewModel
                    {
                        Id = p.Id,
                        Data = p.Data,
                        Valor = p.ValorTotal,
                        Status = p.Status
                    })
                    .ToList(),

                ProdutosMaisVendidos = maisVendidos
            };

            return View(model);
        }
    }
}