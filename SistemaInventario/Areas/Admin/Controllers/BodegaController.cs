using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SistemaInventario.AccesoDatos.Repository.IRepository;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;

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

        [HttpPost]
        public async Task<IActionResult> Upsert(Bodega bodega)
        {
            if (ModelState.IsValid)
            {
                if (bodega.Id == 0)
                {
                    await _unitWork.Bodega.Agregar(bodega);
                    TempData[DS.Exitosa] = "Bodega creada exitosamente";
                }
                else
                {
                    _unitWork.Bodega.Actualizar(bodega);
                    TempData[DS.Exitosa] = "Bodega actualizada exitosamente";
                }

                await _unitWork.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error]  = "Error al grabar la Bodega";
            return View(bodega);
        }



        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unitWork.Bodega.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var bodegaDB = await _unitWork.Bodega.Obtener(id);
            if (bodegaDB == null)
            {
                return Json(new { success = false, message="Error al eliminar" });
            }
            _unitWork.Bodega.Remover(bodegaDB);
            await _unitWork.Guardar();
            return Json(new { success = true, message = "Bodega eliminada exitosamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id=0)
        {
            bool valor = false;
            var lista = await _unitWork.Bodega.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim() && b.Id != id);
            }

            if (valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });
        }


        #endregion
    }
}
