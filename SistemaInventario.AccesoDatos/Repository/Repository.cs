using Microsoft.EntityFrameworkCore;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repository.IRepository;
using SistemaInventario.Modelos.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public async Task Agregar(T entidad)
        {
            await dbSet.AddAsync(entidad); //Insert into table
        }

        public async Task<T> Obtener(int id)
        {
            return await dbSet.FindAsync(id); //Select *From (Solo por id)
        }

        public async Task<IEnumerable<T>> ObtenerTodos(Expression<Func<T, bool>> filtro = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            
            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            if(incluirPropiedades != null)
            {
                foreach (var incluirProp in incluirPropiedades.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp); // Join con demas entidades
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public PagedList<T> ObtenerTodosPaginado(Parametros parametros, Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;

            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            if (incluirPropiedades != null)
            {
                foreach (var incluirProp in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp); // Join con demas entidades
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return PagedList<T>.ToPagedList(query, parametros.PageNumber, parametros.PageSize);
        }

        public async Task<T> ObtenerPrimero(Expression<Func<T, bool>> filtro = null, 
            string incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;

            if (filtro != null)
            {
                query = query.Where(filtro);
            }

            if (incluirPropiedades != null)
            {
                foreach (var incluirProp in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp); // Join con demas entidades
                }
            }

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync();
        }

        public void Remover(T entidad)
        {
            dbSet.Remove(entidad); //Delete *From Table
        }

        public void RemoverRango(IEnumerable<T> entidad)
        {
            dbSet.RemoveRange(entidad); 
        }

        
    }
}
