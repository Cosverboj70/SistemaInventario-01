using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;
using System.Data;

namespace Sistema_Inventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin)]

    public class TipoController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public TipoController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Tipo tipo = new Tipo();

            if (id == null)
            {
                // Crear una nueva Bodega
                tipo.Estado = true;
                return View(tipo);
            }
            // Actualizamos Bodega
            tipo = await _unidadTrabajo.Tipo.Obtener(id.GetValueOrDefault());
            if (tipo == null)
            {
                return NotFound();
            }
            return View(tipo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Tipo tipo)
        {
            if (ModelState.IsValid)
            {
                if (tipo.Id == 0)
                {
                    await _unidadTrabajo.Tipo.Agregar(tipo);
                    TempData[DS.Exitosa] = "Tipo creada Exitosamente";
                }
                else
                {
                    _unidadTrabajo.Tipo.Actualizar(tipo);
                    TempData[DS.Exitosa] = "Tipo actualizada Exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al grabar Tipo";
            return View(tipo);
        }


        #region API
        [HttpGet]
        public async Task<ActionResult> Obtenertodos()
        {
            var todos = await _unidadTrabajo.Tipo.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var tipoDb = await _unidadTrabajo.Tipo.Obtener(id);
            if (tipoDb   == null)
            {
                return Json(new { success = false, message = "Error al borrar Tipo" });
            }
            _unidadTrabajo.Tipo.Remover(tipoDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Tipo borrada exitosamente" });
        }

        //ActionName==>Para poder ser llamado desde JS
        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Tipo.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim() && b.Id != id);
            }
            if (valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });

        }


        #endregion

    }
}
