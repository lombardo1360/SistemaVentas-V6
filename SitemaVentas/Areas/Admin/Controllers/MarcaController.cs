using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.Datos.Repositorio.IRespositorio;
using SistemaVentas.Modelos;
using SistemaVentas.Utilidades;

namespace SistemaVentas.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin)]
    public class MarcaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public MarcaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UpdInsert(int? id)
        {
            Marca marca = new Marca();
            if (id == null)
            {
                return View(marca);
            }
            marca = _unidadTrabajo.Marca.Obtener(id.GetValueOrDefault());
            if (marca == null)
            {
                return NotFound();
            }
            return View(marca);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdInsert(Marca marca)
        {
            Console.WriteLine(ModelState.ToList());
            if (ModelState.IsValid)
            {
                if (marca.Id == 0)
                {
                    _unidadTrabajo.Marca.Agregar(marca);
                }
                else
                {
                    _unidadTrabajo.Marca.Actualizar(marca);
                }
                _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            return View(marca);
        }


        #region API
        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            var todos = _unidadTrabajo.Marca.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var marcaDB = _unidadTrabajo.Marca.Obtener(id);
            if (marcaDB == null)
            {
                return Json(new { success = false, message = "Error al borrar" });
            }
            _unidadTrabajo.Marca.Remover(marcaDB);
            _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Marca borrada exitosamente!" });
        }
        #endregion

    }
}
