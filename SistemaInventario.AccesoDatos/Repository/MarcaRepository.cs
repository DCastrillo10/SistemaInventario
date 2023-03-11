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
    public class MarcaRepository : Repository<Marca>, IMarcaRepository
    {
        private readonly ApplicationDbContext _db;

        public MarcaRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        public void Actualizar(Marca marca)
        {
            var marcaBD = _db.Marcas.FirstOrDefault(m=>m.Id== marca.Id);
            if (marcaBD != null)
            {
                marcaBD.Nombre = marca.Nombre;
                marcaBD.Estado = marca.Estado;
            }
        }
    }
}
