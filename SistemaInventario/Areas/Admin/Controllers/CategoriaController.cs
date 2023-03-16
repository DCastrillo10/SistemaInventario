using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repository.IRepository;
using SistemaInventario.Modelos;

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

        public async Task<IActionResult> Upsert(int? id)
        {
            Categoria categoria = new Categoria();
            if (id == null)
            {
                return View(categoria);
            }

            categoria = await _unitWork.Categoria.Obtener(id.GetValueOrDefault());
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                if (categoria.Id == 0)
                {
                    await _unitWork.Categoria.Agregar(categoria);
                }
                else
                {
                    _unitWork.Categoria.Actualizar(categoria);
                }

                await _unitWork.Guardar();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unitWork.Categoria.ObtenerTodos();
            return Json(new {data = todos});
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var categoriaBD = await _unitWork.Categoria.Obtener(id);
            if (categoriaBD == null)
            {
                return Json(new { success = false, message = "Error al eliminar" });
            }
            else
            {
                _unitWork.Categoria.Remover(categoriaBD);
                await _unitWork.Guardar();
                return Json(new { success = true, message = "Categoria eliminada exitosamente" });
            }
        }

        #endregion
    }
}
