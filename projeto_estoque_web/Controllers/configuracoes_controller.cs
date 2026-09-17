using Microsoft.AspNetCore.Mvc;

namespace projeto_estoque_web.Controllers
{
    public class ConfiguracoesController : Controller
    {
        public IActionResult Index()
            => View();
    }
}