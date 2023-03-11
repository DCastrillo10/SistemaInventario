using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repository.IRepository;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MarcaController : Controller
    {
        private readonly IUnitWork _unitWork;

        public MarcaController(IUnitWork unitWork)
        {
            _unitWork = unitWork;
        }
        public IActionResult Index()
        {
            return View();
        }





        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unitWork.Marca.ObtenerTodos();
            return Json(new { data = todos });
        }

        #endregion
    }
}
