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
    public class InventarioDetalleRepository : Repository<InventarioDetalle>, IInventarioDetalleRepository
    {
        private readonly ApplicationDbContext _db;

        public InventarioDetalleRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        public void Actualizar(InventarioDetalle inventarioDetalle)
        {
            var inventarioDetalleBD = _db.InventarioDetalles.FirstOrDefault(m=>m.Id== inventarioDetalle.Id);
            if (inventarioDetalleBD != null)
            {
                
                inventarioDetalleBD.StockAnterior = inventarioDetalle.StockAnterior;
                inventarioDetalleBD.Cantidad = inventarioDetalle.Cantidad;
                
                _db.SaveChanges();
            }
        }

    }
}
