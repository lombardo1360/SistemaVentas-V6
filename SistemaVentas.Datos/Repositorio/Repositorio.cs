using Microsoft.EntityFrameworkCore;
using SistemaVentas.Datos.Repositorio.IRespositorio;
using SitemaVentas.SistemaVentasDatos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.Datos.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;


        public Repositorio(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public void Agregar(T entidad)
        {
            dbSet.Add(entidad); //Inset Into
        }

        public T Obtener(int id)
        {
            return dbSet.Find(id);
        }

        public T ObtenerPrimero(Expression<Func<T, bool>> filter = null, string incluirPropiedad = null)
        {

            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (incluirPropiedad != null)
            {
                foreach (var incluirProp in incluirPropiedad.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp);
                }
            }

            return query.FirstOrDefault();

        }

        public IEnumerable<T> ObtenerTodos(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string incluirPropiedad = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (incluirPropiedad != null)
            {
                foreach (var incluirProp in incluirPropiedad.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }

            return query.ToList();
        }

        public void Remover(int id)
        {
            T entidad = dbSet.Find(id);
            Remover(entidad);
        }

        public void Remover(T entidad)
        {
            dbSet.Remove(entidad);
        }

        public void RemoverRango(IEnumerable<T> entidad)
        {
            dbSet.RemoveRange(entidad);
        }
    }
}
