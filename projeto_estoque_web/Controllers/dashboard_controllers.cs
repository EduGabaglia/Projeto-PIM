using Microsoft.AspNetCore.Mvc;
using Projeto_estoque.web.Models.ViewModels;

namespace Projeto_estoque.web.Controllers
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

                UltimosPedidos = new List<PedidoResumoViewModel>
                {
                    new PedidoResumoViewModel
                    {
                        Id = 1024,
                        Data = DateTime.Now,
                        Valor = 85.90m,
                        Status = "Pendente"
                    },
                    new PedidoResumoViewModel
                    {
                        Id = 1023,
                        Data = DateTime.Now.AddDays(-1),
                        Valor = 42.50m,
                        Status = "Preparando"
                    },
                    new PedidoResumoViewModel
                    {
                        Id = 1022,
                        Data = DateTime.Now.AddDays(-2),
                        Valor = 120.00m,
                        Status = "Entregue"
                    }
                },

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