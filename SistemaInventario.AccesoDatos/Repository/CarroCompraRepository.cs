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
    public class CarroCompraRepository : Repository<CarroCompra>, ICarroCompraRepository
    {
        private readonly ApplicationDbContext _db;

        public CarroCompraRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        public void Actualizar(CarroCompra carroCompra)
        {
            _db.Update(carroCompra);
        }
    }
}
