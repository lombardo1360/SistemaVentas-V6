using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.Datos.Repositorio.IRespositorio;
using SistemaVentas.Modelos;
using SistemaVentas.Utilidades;

namespace SistemaVentas.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =DS.Role_Admin)]
    public class BodegaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public BodegaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UpdInsert(int? id)
        {
            Bodega bodega = new Bodega();
            if (id == null)
            {
                return View(bodega);
            }
            bodega = _unidadTrabajo.Bodega.Obtener(id.GetValueOrDefault());
            if (bodega == null)
            {
                return NotFound();
            }
            return View(bodega);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdInsert(Bodega bodega)
        {
            if (ModelState.IsValid)
            {
                if (bodega.Id == 0)
                {
                    _unidadTrabajo.Bodega.Agregar(bodega);
                }
                else
                {
                    _unidadTrabajo.Bodega.Actualizar(bodega);
                }
                _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            return View(bodega);
        }


        #region API
        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            var todos = _unidadTrabajo.Bodega.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var bodegaDB = _unidadTrabajo.Bodega.Obtener(id);
            if (bodegaDB == null)
            {
                return Json(new { success = false, message = "Error al borrar" });
            }
            _unidadTrabajo.Bodega.Remover(bodegaDB);
            _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Bodega borrada exitosamente!" });
        }
        #endregion

    }
}
