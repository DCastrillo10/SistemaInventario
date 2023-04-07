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
    public class UsuarioAplicacionRepository : Repository<UsuarioAplicacion>, IUsuarioAplicacionRepository
    {
        private readonly ApplicationDbContext _db;

        public UsuarioAplicacionRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        
    }
}
