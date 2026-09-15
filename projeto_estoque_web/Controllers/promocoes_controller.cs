using Microsoft.AspNetCore.Mvc;

namespace projeto_estoque_web.Controllers
{
    public class PromocoesController : Controller
    {
        public IActionResult Index()
            => View();
    }
}