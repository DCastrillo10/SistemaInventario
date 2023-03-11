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
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoriaRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        public void Actualizar(Categoria categoria)
        {
            var categoriaBD = _db.Categorias.FirstOrDefault(c => c.Id == categoria.Id);
            if(categoriaBD != null)
            {
                categoriaBD.Nombre= categoria.Nombre;
                categoriaBD.Estado=categoria.Estado;
                _db.SaveChanges();
            }
        }
    }
}
