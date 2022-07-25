using SistemaVentas.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVentas.Datos.Repositorio.IRespositorio
{
    public interface IBodegaRepositorio: IRepositorio<Bodega>
    {
        void Actualizar(Bodega bodega);
    }
}
