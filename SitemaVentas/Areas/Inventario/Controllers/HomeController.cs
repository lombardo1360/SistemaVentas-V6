﻿using Microsoft.AspNetCore.Mvc;
using SistemaVentas.Datos.Repositorio.IRespositorio;
using SistemaVentas.Modelos;
using SistemaVentas.Modelos.ViewModels;
using System.Diagnostics;

namespace SistemaVentas.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnidadTrabajo _unidadTrabajo;

        public HomeController(ILogger<HomeController> logger, IUnidadTrabajo unidadTrabajo)
        {
            _logger = logger;
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            IEnumerable<Producto> productoLista = _unidadTrabajo.Producto.ObtenerTodos();
            return View(productoLista);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}