using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repository.IRepository;
using SistemaInventario.Modelos;
using SistemaInventario.Modelos.ViewModels;
using System.Diagnostics;

namespace SistemaInventario.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitWork _unitWork;

        public HomeController(ILogger<HomeController> logger, IUnitWork unitWork)
        {
            _logger = logger;
            _unitWork = unitWork;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Producto> productoLista = await _unitWork.Producto.ObtenerTodos();
            return View(productoLista);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}