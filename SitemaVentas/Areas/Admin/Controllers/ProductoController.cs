using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaVentas.Datos.Repositorio.IRespositorio;
using SistemaVentas.Modelos;
using SistemaVentas.Modelos.ViewModels;
using SistemaVentas.Utilidades;

namespace SistemaVentas.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin+","+DS.Role_Inventario)]
    public class ProductoController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductoController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment hostEnvironment)
        {
            _unidadTrabajo = unidadTrabajo;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UpdInsert(int? id)
        {
            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                CategoriaLista = _unidadTrabajo.Categoria.ObtenerTodos().Select(c => new SelectListItem {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                }),
                MarcaLista = _unidadTrabajo.Marca.ObtenerTodos().Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                }),
            };
            if (id == null)
            {
                return View(productoVM);
            }
            productoVM.Producto = _unidadTrabajo.Producto.Obtener(id.GetValueOrDefault());
            if (productoVM.Producto == null)
            {
                return NotFound();
            }
            return View(productoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdInsert(ProductoVM productoVM)
        {

            if (ModelState.IsValid)
            {

                //Cargr imagenes
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string filename = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"imagenes\productos");
                    var extension = Path.GetExtension(files[0].FileName);
                    if (productoVM.Producto.ImagenUrl != null)
                    {
                        var imagenPath = Path.Combine(webRootPath, productoVM.Producto.ImagenUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagenPath))
                        {
                            System.IO.File.Delete(imagenPath);
                        }
                    }

                    using (var filesStreams = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create))
                    {
                        files[0].CopyTo(filesStreams);
                    }
                    productoVM.Producto.ImagenUrl = @"\imagenes\productos\" + filename + extension;
                }
                else
                {
                    if (productoVM.Producto.Id != 0)
                    {
                        Producto productoDb = _unidadTrabajo.Producto.Obtener(productoVM.Producto.Id);
                        productoVM.Producto.ImagenUrl = productoDb.ImagenUrl;
                    }
                }

                if (productoVM.Producto.Id == 0)
                {
                    _unidadTrabajo.Producto.Agregar(productoVM.Producto);
                }
                else
                {
                    _unidadTrabajo.Producto.Actualizar(productoVM.Producto);
                }
                _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            else
            {


                productoVM.CategoriaLista = _unidadTrabajo.Categoria.ObtenerTodos().Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
                productoVM.MarcaLista = _unidadTrabajo.Marca.ObtenerTodos().Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
                if (productoVM.Producto.Id != 0)
                {
                    productoVM.Producto = _unidadTrabajo.Producto.Obtener(productoVM.Producto.Id);
                }
            }
            
            return View(productoVM);
        }


        #region API
        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            var todos = _unidadTrabajo.Producto.ObtenerTodos(incluirPropiedad:"Categoria,Marca");
            return Json(new { data = todos });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var productoDB = _unidadTrabajo.Producto.Obtener(id);
            if (productoDB == null)
            {
                return Json(new { success = false, message = "Error al borrar" });
            }
            //Eliminar la imagen 
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagenPath = Path.Combine(webRootPath, productoDB.ImagenUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagenPath))
            {
                System.IO.File.Delete(imagenPath);
            }

            _unidadTrabajo.Producto.Remover(productoDB);
            _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Producto borrada exitosamente!" });
        }
        #endregion

    }
}
