using SistemaVentas.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.Datos.Repositorio.IRespositorio
{
    public interface IMarcaRepositorio:IRepositorio<Marca>
    {
        void Actualizar(Marca marca);
    }
}
