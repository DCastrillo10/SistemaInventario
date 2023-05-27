using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repository.IRepository;
using SistemaInventario.Modelos.ViewModels;
using SistemaInventario.Utilidades;
using System.Security.Claims;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =DS.Role_Admin)]
    public class CompaniaController : Controller
    {
        private readonly IUnitWork _unitWork;

        public CompaniaController(IUnitWork unitWork)
        {
            _unitWork = unitWork;
        }

        public async Task<IActionResult> Upsert()
        {
            CompaniaVM companiaVM = new CompaniaVM()
            {
                Compania= new Modelos.Compania(),
                BodegaLista = _unitWork.Inventario.ObtenerTodosDropDownLista("Bodega") 
            };

            companiaVM.Compania = await _unitWork.Compania.ObtenerPrimero();
            if (companiaVM.Compania == null)
            {
                companiaVM.Compania= new Modelos.Compania();
            }
            return View(companiaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(CompaniaVM companiaVM)
        {
            if(ModelState.IsValid)
            {
                TempData[DS.Exitosa] = "Compania grabada exitosamente";
                var claimIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

                if (companiaVM.Compania.Id==0) //Crear Compañia
                {
                    companiaVM.Compania.CreadoPorId = claim.Value;
                    companiaVM.Compania.ActualizadoPorId = claim.Value;
                    companiaVM.Compania.FechaCreacion=DateTime.Now;
                    companiaVM.Compania.FechaActualizacion=DateTime.Now;
                    await _unitWork.Compania.Agregar(companiaVM.Compania);
                }
                else //Actualizar Compania
                {
                    companiaVM.Compania.ActualizadoPorId = claim.Value;
                    companiaVM.Compania.FechaActualizacion = DateTime.Now;
                    _unitWork.Compania.Actualizar(companiaVM.Compania);
                }
                await _unitWork.Guardar();
                return RedirectToAction("Index", "Home", new {area="Inventario"});
            }
            TempData[DS.Error] = "Error al grabar compania";
            return View(companiaVM);
        }
    }
}
