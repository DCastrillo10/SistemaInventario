using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SistemaInventario.AccesoDatos.Repository.IRepository;
using SistemaInventario.Modelos;
using SistemaInventario.Modelos.ViewModels;
using SistemaInventario.Utilidades;

namespace SistemaInventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductoController : Controller
    {
        private readonly IUnitWork _unitWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductoController(IUnitWork unitWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitWork = unitWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        //HttpGet
        public async Task<IActionResult> Upsert(int? id)
        {
            ProductoVM productoVM = new ProductoVM()
            {
                Producto= new Producto(),
                CategoriaLista = _unitWork.Producto.ObtenerTodosDropDownList("Categoria"),
                MarcaLista = _unitWork.Producto.ObtenerTodosDropDownList("Marca"),
                PadreLista = _unitWork.Producto.ObtenerTodosDropDownList("Producto")
            };

            if (id == null)
            {
                //Mostramos la vista para crear un producto
                productoVM.Producto.Estado = true;
                return View(productoVM);
            }
            else
            {
                //Mostramos la vista con el producto seleccioando
                productoVM.Producto = await _unitWork.Producto.Obtener(id.GetValueOrDefault());
                if(productoVM.Producto == null)
                {
                    return NotFound();
                }
                return View(productoVM);
            }
            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductoVM productoVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productoVM.Producto.Id == 0)
                {
                    //Crear Producto
                    string upload = webRootPath + DS.ImagenRuta;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using(var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productoVM.Producto.ImgUrl = fileName + extension;
                    await _unitWork.Producto.Agregar(productoVM.Producto);
                }
                else
                {
                    //Actualizar Producto
                    var objProducto = await _unitWork.Producto.ObtenerPrimero(p=>p.Id == productoVM.Producto.Id, isTracking: false);
                    
                    if (files.Count > 0) //Si se carga una nueva imagen para el producto existente
                    {
                        string upload = webRootPath + DS.ImagenRuta;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        //Borrar la imagen anterior
                        var anteriorFile = Path.Combine(upload, objProducto.ImgUrl);
                        if (System.IO.File.Exists(anteriorFile))
                        {
                            System.IO.File.Delete(anteriorFile);
                        }

                        using(var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        productoVM.Producto.ImgUrl = fileName + extension;
                    }
                    else
                    {
                        productoVM.Producto.ImgUrl = objProducto.ImgUrl;
                    }
                    _unitWork.Producto.Actualizar(productoVM.Producto);
                }
                TempData[DS.Exitosa] = "Transaccion Exitosa..!";
                await _unitWork.Guardar();
                return View("Index");

            }// If ModelState IsNotValid
            productoVM.CategoriaLista = _unitWork.Producto.ObtenerTodosDropDownList("Categoria");
            productoVM.CategoriaLista = _unitWork.Producto.ObtenerTodosDropDownList("Marca");
            productoVM.PadreLista = _unitWork.Producto.ObtenerTodosDropDownList("Producto");
            return View(productoVM);
        }

       
        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unitWork.Producto.ObtenerTodos(incluirPropiedades:"Categoria,Marca");
            return Json(new { data = todos });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var productoBD = await _unitWork.Producto.Obtener(id);
            if (productoBD == null)
            {
                return Json(new { success = false, message = "Error al eliminar" });
            }

            //Eliminamos la imagen
            string upload = _webHostEnvironment.WebRootPath + DS.ImagenRuta;
            var anteriorFile = Path.Combine(upload, productoBD.ImgUrl);
            if (System.IO.File.Exists(anteriorFile))
            {
                System.IO.File.Delete(anteriorFile);
            }

            _unitWork.Producto.Remover(productoBD);
            await _unitWork.Guardar();
            return Json(new { success = true, message = "Producto eliminada exitosamente" });
            
        }

        [ActionName("ValidarSerie")]
        public async Task<IActionResult> ValidarSerie(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unitWork.Producto.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(b => b.NumeroSerie.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b => b.NumeroSerie.ToLower().Trim() == nombre.ToLower().Trim() && b.Id != id);
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
