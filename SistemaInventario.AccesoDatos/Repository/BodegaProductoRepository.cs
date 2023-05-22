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
    public class BodegaProductoRepository : Repository<BodegaProducto>, IBodegaProductoRepository
    {
        private readonly ApplicationDbContext _db;

        public BodegaProductoRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        public void Actualizar(BodegaProducto bodegaProducto)
        {
            var bodegaProductoBD = _db.BodegaProductos.FirstOrDefault(m=>m.Id== bodegaProducto.Id);
            if (bodegaProductoBD != null)
            {
                bodegaProductoBD.Cantidad = bodegaProducto.Cantidad;
                _db.SaveChanges();
            }
        }

    }
}
