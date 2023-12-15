using Microsoft.AspNetCore.Mvc;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos;
using SistemaInventario.Utilidades;

namespace Sistema_Inventario.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoordinacionController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public CoordinacionController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Coordinacion coordinacion = new Coordinacion();

            if (id == null)
            {
                // Crear una nueva Coordinacion
                coordinacion.Estado = true;
                return View(coordinacion);
            }
            // Actualizamos Coordinacion
            coordinacion = await _unidadTrabajo.Coordinacion.Obtener(id.GetValueOrDefault());
            if (coordinacion == null)
            {
                return NotFound();
            }
            return View(coordinacion);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Coordinacion coordinacion)
        {
            if (ModelState.IsValid)
            {
                if (coordinacion.Id == 0)
                {
                    await _unidadTrabajo.Coordinacion.Agregar(coordinacion);
                    TempData[DS.Exitosa] = "Coordinacion creada Exitosamente";
                }
                else
                {
                    _unidadTrabajo.Coordinacion.Actualizar(coordinacion);
                    TempData[DS.Exitosa] = "Coordinacion actualizada Exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al grabar Coordinacion";
            return View(coordinacion);
        }


        #region API
        [HttpGet]
        public async Task<ActionResult> Obtenertodos()
        {
            var todos = await _unidadTrabajo.Coordinacion.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var coordinacionDb = await _unidadTrabajo.Coordinacion.Obtener(id);
            if (coordinacionDb == null)
            {
                return Json(new { success = false, message = "Error al borrar Coordinacion" });
            }
            _unidadTrabajo.Coordinacion.Remover(coordinacionDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Coordinacion borrada exitosamente" });
        }

        //ActionName==>Para poder ser llamado desde JS
        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Coordinacion.ObtenerTodos();
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
