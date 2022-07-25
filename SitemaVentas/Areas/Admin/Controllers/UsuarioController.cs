﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaVentas.Utilidades;
using SitemaVentas.SistemaVentasDatos.Data;

namespace SistemaVentas.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin)]
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UsuarioController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API

        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            var usuarioLista = _db.UsuarioAplicacion.ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach (var usuario in usuarioLista)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == usuario.Id).RoleId;
                usuario.Role = roles.FirstOrDefault(r => r.Id == roleId).Name;
            }
            return Json(new { data = usuarioLista });
        }

        [HttpPost]
        public IActionResult BloquearDesbloquear([FromBody] string id)
        {
            var usuario = _db.UsuarioAplicacion.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                return Json(new { success = false, message = "Error de usuario" });
            }

            if (usuario.LockoutEnd != null && usuario.LockoutEnd> DateTime.Now)
            {
                usuario.LockoutEnd = DateTime.Now;
            }
            else
            {
                usuario.LockoutEnd = DateTime.Now.AddYears(100);
            }
            _db.SaveChanges();
            return Json(new { success = true, message = "Operacion Exitosa!" });
        }

        #endregion
    }
}
