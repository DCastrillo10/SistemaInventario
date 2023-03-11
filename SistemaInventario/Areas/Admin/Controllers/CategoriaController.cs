using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repository.IRepository;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriaController : Controller
    {
        private readonly IUnitWork _unitWork;

        public CategoriaController(IUnitWork unitWork)
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
            var todos = await _unitWork.Categoria.ObtenerTodos();
            return Json(new {data = todos});
        }
        #endregion
    }
}
