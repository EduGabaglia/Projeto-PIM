using Microsoft.AspNetCore.Mvc;
using projeto_estoque_web.Models.view_models;

namespace projeto_estoque_web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var model = new DashboardViewModel
            {
                TotalProdutos = 42,
                EstoqueBaixo = 5,
                PedidosPendentes = 3,
                VendasMes = 2540.00m,

                UltimosPedidos = PedidosController.TodosOsPedidos
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

                ProdutosMaisVendidos = new List<ProdutoMaisVendidoViewModel>
                {
                    new ProdutoMaisVendidoViewModel
                    {
                        Nome = "Arroz 5kg",
                        QuantidadeVendida = 12
                    },
                    new ProdutoMaisVendidoViewModel
                    {
                        Nome = "Feijão 1kg",
                        QuantidadeVendida = 10
                    },
                    new ProdutoMaisVendidoViewModel
                    {
                        Nome = "Óleo de cozinha",
                        QuantidadeVendida = 8
                    },
                    new ProdutoMaisVendidoViewModel
                    {
                        Nome = "Açúcar 1kg",
                        QuantidadeVendida = 6
                    }
                }
            };

            return View(model);
        }
    }
}