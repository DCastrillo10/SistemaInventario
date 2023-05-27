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
    public class CompaniaRepository : Repository<Compania>, ICompaniaRepository
    {
        private readonly ApplicationDbContext _db;

        public CompaniaRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        public void Actualizar(Compania compania)
        {
            var companiaBD = _db.Companias.FirstOrDefault(b => b.Id == compania.Id);
            if (companiaBD != null)
            {
                companiaBD.Nombre = compania.Nombre;
                companiaBD.Descripcion = compania.Descripcion;
                companiaBD.Pais=compania.Pais;
                companiaBD.Ciudad = compania.Ciudad;
                companiaBD.Direccion = compania.Direccion;
                companiaBD.Telefono = compania.Telefono;
                companiaBD.BodegaVentaId = compania.BodegaVentaId;
                companiaBD.ActualizadoPorId = compania.ActualizadoPorId;
                companiaBD.FechaActualizacion = compania.FechaActualizacion;
                _db.SaveChanges();
            }
        }
    }
}
