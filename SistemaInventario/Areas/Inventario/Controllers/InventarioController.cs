using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using SistemaInventario.AccesoDatos.Repository.IRepository;
using SistemaInventario.Modelos;
using SistemaInventario.Modelos.ViewModels;
using SistemaInventario.Utilidades;
using System.Security.Claims;

namespace SistemaInventario.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Inventario)]
    public class InventarioController : Controller
    {
        private readonly IUnitWork _unitWork;
        
        [BindProperty]
        public InventarioVM inventarioVM { get; set; }

        public InventarioController(IUnitWork unitWork)
        {
            _unitWork = unitWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult NuevoInventario()
        {
            inventarioVM = new InventarioVM()
            {
                Inventario = new Modelos.Inventario(),
                BodegaLista = _unitWork.Inventario.ObtenerTodosDropDownLista("Bodega")
            };
            inventarioVM.Inventario.Estado = false;
            //Obtener el Id del usuario de la sesion
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            inventarioVM.Inventario.UsuarioAplicacionId = claim.Value;
            inventarioVM.Inventario.FechaInicial = DateTime.Now;
            inventarioVM.Inventario.FechaFinal = DateTime.Now;

            return View(inventarioVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NuevoInventario(InventarioVM inventarioVM)
        {
            if (ModelState.IsValid)
            {
                inventarioVM.Inventario.FechaInicial = DateTime.Now;
                inventarioVM.Inventario.FechaFinal = DateTime.Now;
                await _unitWork.Inventario.Agregar(inventarioVM.Inventario);
                await _unitWork.Guardar();
                return RedirectToAction("DetalleInventario", new { id = inventarioVM.Inventario.Id });
            }
            inventarioVM.BodegaLista = _unitWork.Inventario.ObtenerTodosDropDownLista("Bodega");
            return View(inventarioVM);
        }

        public async Task<IActionResult> DetalleInventario(int id)
        {
            inventarioVM = new InventarioVM();
            inventarioVM.Inventario = await _unitWork.Inventario.ObtenerPrimero(i => i.Id == id,incluirPropiedades:"Bodega");
            inventarioVM.InventarioDetalles = await _unitWork.InventarioDetalle.ObtenerTodos(d => d.InventarioId == id,
                                              incluirPropiedades: "Producto,Producto.Marca");
            return View(inventarioVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetalleInventario(int inventarioId, int productoId, int cantidadId)
        {
            inventarioVM = new InventarioVM();
            inventarioVM.Inventario = await _unitWork.Inventario.ObtenerPrimero(i => i.Id == inventarioId);
            var bodegaProducto = await _unitWork.BodegaProducto.ObtenerPrimero(p => p.ProductoId == productoId &&
                                                                                    p.BodegaId == inventarioVM.Inventario.BodegaId);
            var detalle = await _unitWork.InventarioDetalle.ObtenerPrimero(d => d.InventarioId == inventarioId &&
                                                                                d.ProductoId == productoId);
            if(detalle == null)
            {
                inventarioVM.InventarioDetalle = new Modelos.InventarioDetalle();
                inventarioVM.InventarioDetalle.InventarioId= inventarioId;
                inventarioVM.InventarioDetalle.ProductoId = productoId;
                if (bodegaProducto != null)
                {
                    inventarioVM.InventarioDetalle.StockAnterior = bodegaProducto.Cantidad;
                }
                else
                {
                    inventarioVM.InventarioDetalle.StockAnterior = 0;
                }
                inventarioVM.InventarioDetalle.Cantidad = cantidadId;
                await _unitWork.InventarioDetalle.Agregar(inventarioVM.InventarioDetalle); //Insert into
                await _unitWork.Guardar();
            }
            else
            {
                detalle.Cantidad += cantidadId;
                await _unitWork.Guardar();
            }
            return RedirectToAction("DetalleInventario", new { id = inventarioId });
        }

        public async Task<IActionResult> Mas(int id) //recibe el id del detalle
        {
            inventarioVM = new InventarioVM();
            var detalle = await _unitWork.InventarioDetalle.Obtener(id);
            //inventarioVM.Inventario = await _unitWork.Inventario.Obtener(detalle.InventarioId);

            detalle.Cantidad += 1;
            await _unitWork.Guardar();
            return RedirectToAction("DetalleInventario", new { id = detalle.InventarioId });
        }

        public async Task<IActionResult> Menos(int id) //recibe el id del detalle
        {
            inventarioVM = new InventarioVM();
            var detalle = await _unitWork.InventarioDetalle.Obtener(id);
            //inventarioVM.Inventario = await _unitWork.Inventario.Obtener(detalle.InventarioId);
            if (detalle.Cantidad==1)
            {
                _unitWork.InventarioDetalle.Remover(detalle);
                await _unitWork.Guardar();
            }
            else
            {
                detalle.Cantidad -= 1;
                await _unitWork.Guardar();
            }
            return RedirectToAction("DetalleInventario", new { id = detalle.InventarioId });
        }

        public async Task<IActionResult> GenerarStock(int id) //Recibe el Id del inventario
        {
            var inventario = await _unitWork.Inventario.Obtener(id);
            var detallelista = await _unitWork.InventarioDetalle.ObtenerTodos(d => d.InventarioId == id);
            //Obtener el Id del usuario de la sesion
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            foreach (var item in detallelista)
            {
                var bodegaProducto = new BodegaProducto();
                bodegaProducto = await _unitWork.BodegaProducto.ObtenerPrimero(p => p.ProductoId == item.ProductoId &&
                                                                                    p.BodegaId == inventario.BodegaId);

                if (bodegaProducto != null) //Actualizo la cantidad en bodegaproducto
                {
                    await _unitWork.KardexInventario.RegistrarKardex(bodegaProducto.Id, "Entrada", "Registro de Inventario",
                                                                     bodegaProducto.Cantidad, item.Cantidad, claim.Value);
                    bodegaProducto.Cantidad += item.Cantidad;
                    await _unitWork.Guardar();
                }
                else //Inserto el registro en bodegaproducto
                {
                    bodegaProducto = new BodegaProducto();
                    bodegaProducto.BodegaId = inventario.BodegaId;
                    bodegaProducto.ProductoId=item.ProductoId;
                    bodegaProducto.Cantidad = item.Cantidad;
                    await _unitWork.BodegaProducto.Agregar(bodegaProducto);
                    await _unitWork.Guardar();
                    await _unitWork.KardexInventario.RegistrarKardex(bodegaProducto.Id, "Entrada", "Inventario Inicial",
                                                                     0, item.Cantidad, claim.Value);
                }
            }
            //Cambiamos el estado del inventario y actualizamos fechas
            inventario.Estado = true;
            inventario.FechaFinal = DateTime.Now;
            await _unitWork.Guardar();
            TempData[DS.Exitosa] = "Stock Generado con exito";
            return RedirectToAction("Index");
        }

        public IActionResult KardexProducto()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult KardexProducto(string fechaInicioId, string fechaFinalId, int productoId)
        {
            return RedirectToAction("KardexProductoResultado", new { fechaInicioId, fechaFinalId, productoId });
        }

        public async Task<IActionResult> KardexProductoResultado(string fechaInicioId, string fechaFinalId, int productoId)
        {
            KardexInventarioVM kardexInventarioVM = new KardexInventarioVM();
            kardexInventarioVM.Producto = new Producto();
            kardexInventarioVM.Producto = await _unitWork.Producto.Obtener(productoId);
            kardexInventarioVM.FechaInicio = DateTime.Parse(fechaInicioId);
            kardexInventarioVM.FechaFin = DateTime.Parse(fechaFinalId).AddHours(23).AddMinutes(59);

            kardexInventarioVM.KardexInventarioLista = await _unitWork.KardexInventario.ObtenerTodos(
                                                              k => k.BodegaProducto.ProductoId == productoId &&
                                                                  (k.FechaRegistro >= kardexInventarioVM.FechaInicio &&
                                                                   k.FechaRegistro <= kardexInventarioVM.FechaFin),
                                                       incluirPropiedades: "BodegaProducto,BodegaProducto.Producto,BodegaProducto.Bodega",
                                                       orderBy: o=>o.OrderBy(o=>o.FechaRegistro)
                                                       );
            return View(kardexInventarioVM);
        }

        public async Task<IActionResult> ImprimirKardex(string fechaInicio, string fechaFinal, int productoId)
        {
            KardexInventarioVM kardexInventarioVM = new KardexInventarioVM();
            kardexInventarioVM.Producto = new Producto();
            kardexInventarioVM.Producto = await _unitWork.Producto.Obtener(productoId);
            
            kardexInventarioVM.FechaInicio = DateTime.Parse(fechaInicio);
            kardexInventarioVM.FechaFin = DateTime.Parse(fechaFinal);

            kardexInventarioVM.KardexInventarioLista = await _unitWork.KardexInventario.ObtenerTodos(
                                                              k => k.BodegaProducto.ProductoId == productoId &&
                                                                  (k.FechaRegistro >= kardexInventarioVM.FechaInicio &&
                                                                   k.FechaRegistro <= kardexInventarioVM.FechaFin),
                                                       incluirPropiedades: "BodegaProducto,BodegaProducto.Producto,BodegaProducto.Bodega",
                                                       orderBy: o => o.OrderBy(o => o.FechaRegistro)
                                                       );
            
            return new ViewAsPdf("ImprimirKardex", kardexInventarioVM)
            {
                FileName="KardexInventario.pdf",
                PageOrientation=Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize=Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches="--page-offset 0 --footer-center [page] --footer-font-size 12"
            };
        }





        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unitWork.BodegaProducto.ObtenerTodos(incluirPropiedades:"Bodega,Producto");
            return Json(new { data = todos });
        }

        [HttpGet]
        public async Task<IActionResult> BuscarProducto(string term)
        {
            if (!string.IsNullOrEmpty(term))
            {
                var listaProductos = await _unitWork.Producto.ObtenerTodos(p => p.Estado == true);
                var data = listaProductos.Where(x => x.NumeroSerie.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                                                     x.Descripcion.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList();
                return Ok(data);
            }
            return Ok();
        }
        #endregion
    }
}
