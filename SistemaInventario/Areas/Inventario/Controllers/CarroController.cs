using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repository.IRepository;
using SistemaInventario.Modelos.ViewModels;
using SistemaInventario.Utilidades;
using System.Security.Claims;

namespace SistemaInventario.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class CarroController : Controller
    {
        private readonly IUnitWork _unitWork;

        [BindProperty]
        public CarroCompraVM carroCompraVM { get; set; }

        public CarroController(IUnitWork unitWork)
        {
            _unitWork = unitWork;
        }
        
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var claimIdentity =(ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            carroCompraVM = new CarroCompraVM();
            carroCompraVM.Orden = new Modelos.Orden();
            carroCompraVM.CarroCompraLista = await _unitWork.CarroCompra.ObtenerTodos(u => u.UsuarioAplicacionId == claim.Value,
                                                                                      incluirPropiedades: "Producto");
            carroCompraVM.Orden.TotalOrden = 0;
            carroCompraVM.Orden.UsuarioAplicacionId=claim.Value;

            foreach (var item in carroCompraVM.CarroCompraLista)
            {
                item.Precio = item.Producto.Precio; //Siempre mostrar el precio actual del producto
                carroCompraVM.Orden.TotalOrden += (item.Precio * item.Cantidad);
            }

            return View(carroCompraVM);
        }

        public async Task<IActionResult> Mas(int carroId)
        {
            var carroCompras = await _unitWork.CarroCompra.ObtenerPrimero(c=>c.Id == carroId);
            carroCompras.Cantidad += 1;
            await _unitWork.Guardar();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Menos(int carroId)
        {
            var carroCompras = await _unitWork.CarroCompra.ObtenerPrimero(c => c.Id == carroId);
            if (carroCompras.Cantidad == 1)
            {
                //Remover el item del carrocompra y actualizar el carrito en la session
                var carroLista = await _unitWork.CarroCompra.ObtenerTodos(c => c.UsuarioAplicacionId == carroCompras.UsuarioAplicacionId);
                var numeroProductos = carroLista.Count();
                _unitWork.CarroCompra.Remover(carroCompras);
                await _unitWork.Guardar();
                HttpContext.Session.SetInt32(DS.ssCarroCompras, numeroProductos - 1);
            }
            else
            {
                carroCompras.Cantidad -= 1;
                await _unitWork.Guardar();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remover(int carroId)
        {
            var carroCompras = await _unitWork.CarroCompra.ObtenerPrimero(c => c.Id == carroId);
            //Remover el item del carrocompra y actualizar el carrito en la session
            var carroLista = await _unitWork.CarroCompra.ObtenerTodos(c => c.UsuarioAplicacionId == carroCompras.UsuarioAplicacionId);
            var numeroProductos = carroLista.Count();
            _unitWork.CarroCompra.Remover(carroCompras);
            await _unitWork.Guardar();
            HttpContext.Session.SetInt32(DS.ssCarroCompras, numeroProductos - 1);

            return RedirectToAction("Index");
        }
    }
}
