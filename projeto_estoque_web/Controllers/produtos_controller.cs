using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using projeto_estoque_web.Models;
using projeto_estoque_web.Models.view_models;

namespace projeto_estoque_web.Controllers
{
    public class ProdutosController : Controller
    {
        private const int ItensPorPagina = 8;

        private static readonly string[] CategoriasDisponiveis =
        {
            "Alimentos", "Bebidas", "Higiene", "Limpeza", "Padaria"
        };

        private static readonly List<Produto> _produtos = new()
        {
            new Produto { Id = 1, Nome = "Arroz 5kg", Categoria = "Alimentos", Preco = 24.90m, Quantidade = 42 },
            new Produto { Id = 2, Nome = "Feijão 1kg", Categoria = "Alimentos", Preco = 9.80m, Quantidade = 30 },
            new Produto { Id = 3, Nome = "Óleo de cozinha 900ml", Categoria = "Alimentos", Preco = 8.50m, Quantidade = 12 },
            new Produto { Id = 4, Nome = "Açúcar 1kg", Categoria = "Alimentos", Preco = 5.20m, Quantidade = 25 },
            new Produto { Id = 5, Nome = "Café torrado 500g", Categoria = "Alimentos", Preco = 14.90m, Quantidade = 6 },
            new Produto { Id = 6, Nome = "Leite integral 1L", Categoria = "Bebidas", Preco = 4.50m, Quantidade = 3 },
            new Produto { Id = 7, Nome = "Suco de laranja 1L", Categoria = "Bebidas", Preco = 7.90m, Quantidade = 18 },
            new Produto { Id = 8, Nome = "Refrigerante 2L", Categoria = "Bebidas", Preco = 9.90m, Quantidade = 0 },
            new Produto { Id = 9, Nome = "Água mineral 500ml", Categoria = "Bebidas", Preco = 1.80m, Quantidade = 60 },
            new Produto { Id = 10, Nome = "Sabonete", Categoria = "Higiene", Preco = 2.90m, Quantidade = 100 },
            new Produto { Id = 11, Nome = "Shampoo 400ml", Categoria = "Higiene", Preco = 18.90m, Quantidade = 22 },
            new Produto { Id = 12, Nome = "Detergente 500ml", Categoria = "Limpeza", Preco = 3.40m, Quantidade = 5 },
            new Produto { Id = 13, Nome = "Álcool 1L", Categoria = "Limpeza", Preco = 6.90m, Quantidade = 4 },
            new Produto { Id = 14, Nome = "Pão de forma", Categoria = "Padaria", Preco = 8.90m, Quantidade = 15 }
        };

        private static int _proximoId = _produtos.Count + 1;

        internal static List<Produto> TodosOsProdutos => _produtos;

        public IActionResult Index(string busca, int pagina = 1)
        {
            var lista = _produtos.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(busca))
            {
                var termo = busca.Trim().ToLower();
                lista = lista.Where(p =>
                    p.Nome.ToLower().Contains(termo) ||
                    p.Categoria.ToLower().Contains(termo));
            }

            var ordenados = lista.OrderBy(p => p.Nome).ToList();

            var totalPaginas = Math.Max(1, (int)Math.Ceiling(ordenados.Count / (double)ItensPorPagina));
            pagina = Math.Clamp(pagina, 1, totalPaginas);

            var paginaItens = ordenados
                .Skip((pagina - 1) * ItensPorPagina)
                .Take(ItensPorPagina)
                .ToList();

            var model = new ProdutosViewModel
            {
                TotalProdutos = _produtos.Count,
                EstoqueBaixo = _produtos.Count(p => p.Quantidade <= 5),
                CategoriasCadastradas = _produtos.Select(p => p.Categoria).Distinct().Count(),
                ValorTotalEstoque = _produtos.Sum(p => p.Preco * p.Quantidade),
                Produtos = paginaItens,
                Busca = busca ?? string.Empty,
                PaginaAtual = pagina,
                TotalPaginas = totalPaginas
            };

            return View(model);
        }

        public IActionResult Criar()
        {
            ViewBag.Categorias = ListaCategorias();
            return View(new Produto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Criar(Produto produto)
        {
            ViewBag.Categorias = ListaCategorias(produto.Categoria);

            if (!ModelState.IsValid)
                return View(produto);

            produto.Id = _proximoId++;
            _produtos.Add(produto);

            TempData["Mensagem"] = "Produto cadastrado com sucesso.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Editar(int id)
        {
            var produto = _produtos.FirstOrDefault(p => p.Id == id);

            if (produto is null)
                return NotFound();

            ViewBag.Categorias = ListaCategorias(produto.Categoria);

            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(int id, Produto produto)
        {
            if (id != produto.Id)
                return NotFound();

            var existente = _produtos.FirstOrDefault(p => p.Id == id);

            if (existente is null)
                return NotFound();

            ViewBag.Categorias = ListaCategorias(produto.Categoria);

            if (!ModelState.IsValid)
                return View(produto);

            existente.Nome = produto.Nome;
            existente.Categoria = produto.Categoria;
            existente.Preco = produto.Preco;
            existente.Quantidade = produto.Quantidade;

            TempData["Mensagem"] = "Produto atualizado com sucesso.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Excluir(int id)
        {
            var produto = _produtos.FirstOrDefault(p => p.Id == id);

            if (produto is not null)
            {
                _produtos.Remove(produto);
                TempData["Mensagem"] = "Produto excluído com sucesso.";
            }

            return RedirectToAction(nameof(Index));
        }

        private static SelectList ListaCategorias(string? selecionada = null)
            => new SelectList(CategoriasDisponiveis, selecionada);
    }
}