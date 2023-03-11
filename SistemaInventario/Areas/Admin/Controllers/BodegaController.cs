using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repository.IRepository;
using SistemaInventario.Modelos;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BodegaController : Controller
    {
        private readonly IUnitWork _unitWork;

        public BodegaController(IUnitWork unitWork)
        {
            _unitWork = unitWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Bodega bodega = new Bodega();
            if (id == null)
            {
                //Enviamos la vista para crear
                return  View(bodega);
            }
            //Mostramos la bodega mediante el id
            bodega = await _unitWork.Bodega.Obtener(id.GetValueOrDefault());
            if(bodega == null)
            {
                return NotFound();
            }

            return View(bodega);
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unitWork.Bodega.ObtenerTodos();
            return Json(new { data = todos });
        }

        #endregion
    }
}
