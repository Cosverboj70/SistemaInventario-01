using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;

namespace Sistema_Inventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin)]

    public class TareaController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public TareaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Tareas tarea = new Tareas();

            if (id == null)
            {
                // Crear una nueva Bodega
                tarea.Estado = true;
                return View(tarea);
            }
            // Actualizamos Bodega
            tarea = await _unidadTrabajo.Tarea.Obtener(id.GetValueOrDefault());
            if (tarea == null)
            {
                return NotFound();
            }
            return View(tarea);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Tareas tarea)
        {
            //checar si funciona, eta linea la adicione yo
            tarea.FechaCreacion = DateTime.Now;

            if (ModelState.IsValid)
            {
                if (tarea.Id == 0)
                {
                    await _unidadTrabajo.Tarea.Agregar(tarea);
                    TempData[DS.Exitosa] = "Tarea creada Exitosamente";
                }
                else
                {
                    _unidadTrabajo.Tarea.Actualizar(tarea);
                    TempData[DS.Exitosa] = "Tarea actualizada Exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al grabar Tarea";
            return View(tarea);
        }


        #region API
        [HttpGet]
        public async Task<ActionResult> Obtenertodos()
        {
            var todos = await _unidadTrabajo.Tarea.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var tareaDb = await _unidadTrabajo.Tarea.Obtener(id);
            if (tareaDb == null)
            {
                return Json(new { success = false, message = "Error al borrar Tarea" });
            }
            _unidadTrabajo.Tarea.Remover(tareaDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Tarea borrada exitosamente" });
        }

        //ActionName==>Para poder ser llamado desde JS
        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Tarea.ObtenerTodos();
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
