using SistemaVentas.Datos.Repositorio.IRespositorio;
using SistemaVentas.Modelos;
using SitemaVentas.SistemaVentasDatos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.Datos.Repositorio
{
    public class MarcaRepositorio:Repositorio<Marca>, IMarcaRepositorio
    {
        private readonly ApplicationDbContext _db;

        public MarcaRepositorio(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public void Actualizar(Marca marca)
        {
            var marcaDb = _db.Marca.FirstOrDefault(b => b.Id == marca.Id);
            if (marcaDb != null)
            {
                marcaDb.Nombre = marca.Nombre;
                marcaDb.Estado = marca.Estado;

            }
        }

    }
}
