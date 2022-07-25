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
    public class UsuarioAplicacionRepositorio:Repositorio<UsuarioAplicacion>, IUsuarioAplicacionRepositorio
    {
        private readonly ApplicationDbContext _db;

        public UsuarioAplicacionRepositorio(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
    }
}
