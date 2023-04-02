using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repository.IRepository;
using SistemaInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repository
{
    public class ProductoRepository : Repository<Producto>, IProductoRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductoRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        public void Actualizar(Producto producto)
        {
            var productoBD = _db.Productos.FirstOrDefault(m=>m.Id== producto.Id);
            if (productoBD != null)
            {
                if(producto.ImgUrl != null)
                {
                    productoBD.ImgUrl = producto.ImgUrl;
                }
                productoBD.NumeroSerie= producto.NumeroSerie;
                productoBD.Descripcion= producto.Descripcion;
                productoBD.Precio= producto.Precio;
                productoBD.Costo= producto.Costo;
                productoBD.CategoriaId= producto.CategoriaId;
                productoBD.MarcaId= producto.MarcaId;
                productoBD.PadreId= producto.PadreId;
                productoBD.Estado= producto.Estado;

                _db.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> ObtenerTodosDropDownList(string obj)
        {
            if (obj == "Categoria")
            {
                return _db.Categorias.Where(c => c.Estado == true).Select(c => new SelectListItem
                {
                    Text= c.Nombre,
                    Value=c.Id.ToString() 
                });
            }

            if (obj == "Marca")
            {
                return _db.Marcas.Where(c => c.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
            }

            if (obj == "Producto")
            {
                return _db.Productos.Where(c => c.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Descripcion,
                    Value = c.Id.ToString()
                });
            }

            return null;
        }
    }
}
